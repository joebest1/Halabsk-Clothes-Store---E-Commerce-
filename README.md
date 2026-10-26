# Halabsk

ASP.NET Core 8 MVC storefront backed by SQL Server.

## Authentication

Authentication uses **ASP.NET Core Identity** (EF Core stores, roles) and **JWT bearer tokens**.

- `POST /api/account/signup` creates an Identity user and assigns the `Customer` role.
- `POST /api/account/login` validates the Identity password and returns a signed JWT.
- Browsers also receive the JWT in an `HttpOnly` cookie, so MVC pages and same-origin
  `fetch` calls are authenticated automatically.
- API clients can send the token as `Authorization: Bearer <token>`.
- `GET /api/account/me` requires a valid token.
- Admin pages are protected with `[Authorize(Roles = "Admin")]`.

An admin account is seeded only when `Admin:Email` and `Admin:Password` are
configured. For local development, store them with User Secrets:

```powershell
dotnet user-secrets set "Admin:Email" "admin@example.com"
dotnet user-secrets set "Admin:Password" "<a-strong-password>"
```

## Configuration

JWT issuer, audience, and token lifetimes live in `appsettings.json`:

```json
"Jwt": {
  "Issuer": "Halabsk.Net",
  "Audience": "Halabsk.Net",
  "AccessTokenMinutes": 60,
  "RememberMeDays": 7
}
```

`Jwt:Key` is intentionally not committed. Store it in User Secrets for local
development:

```powershell
dotnet user-secrets set "Jwt:Key" "<at-least-32-character-secret>"
```

User Secrets are loaded only when the app runs in the `Development` environment.
Use `dotnet run` (the launch profile sets `ASPNETCORE_ENVIRONMENT=Development`) or
set the environment explicitly:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --no-launch-profile
```

Visual Studio "Open Folder" does not read `Properties/launchSettings.json`. Select the
checked-in `Halabsk.Net (Development)` configuration from the Start dropdown;
`launch.vs.json` sets `ASPNETCORE_ENVIRONMENT=Development` for that debug session.

For production, provide `Jwt__Key` through the environment or a production secret
store. The key must be at least 32 characters; do not use the development key in
production.

## Database

EF Core migrations are applied automatically at startup. The Identity migration copies
existing rows from the old `Users` table into `AspNetUsers`, preserves existing password
hashes, rehashes legacy plaintext seed passwords, and maps the old `Role` column to
Identity roles.
