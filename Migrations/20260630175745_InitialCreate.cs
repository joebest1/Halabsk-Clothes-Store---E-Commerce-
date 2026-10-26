using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Halabsk.Net.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ContactMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SentAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ContactMessages",
                columns: new[] { "Id", "ContactEmail", "ContactMessage", "ContactName", "SentAtUtc" },
                values: new object[] { "a3a3e35f9b034f9184392bc6a0526f31", "qa@example.com", "Testing the migrated ASP.NET contact form.", "Codex QA", new DateTime(2026, 5, 25, 9, 59, 59, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "pants", "ستايل عملي وسهل التنسيق.", "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80", "بنطلون كاجوال بيج", 520m },
                    { 2, "pants", "قصة عصرية ولمسة streetwear.", "https://images.unsplash.com/photo-1506629905607-d9c297d2bf5c?auto=format&fit=crop&w=900&q=80", "بنطلون أسود أوفر سايز", 610m },
                    { 3, "pants", "جينز يومي مريح للمشاوير والخروجات.", "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80", "بنطلون جينز أزرق فاتح", 580m },
                    { 4, "pants", "قصة slim تعطي مظهرًا أنيقًا.", "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=900&q=80", "بنطلون جينز غامق slim", 640m },
                    { 5, "pants", "خفيف ومناسب للأجواء الصيفية.", "https://images.unsplash.com/photo-1507679799987-c73779587ccf?auto=format&fit=crop&w=900&q=80", "بنطلون لينن أبيض", 690m },
                    { 6, "pants", "طابع عملي وجيوب متعددة.", "https://images.unsplash.com/photo-1514996937319-344454492b37?auto=format&fit=crop&w=900&q=80", "بنطلون كارغو زيتوني", 730m },
                    { 7, "pants", "مظهر قوي ومناسب للوك streetwear.", "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80", "بنطلون كارغو أسود", 760m },
                    { 8, "pants", "مريح للاستخدام اليومي والرياضة.", "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=900&q=80", "بنطلون رياضي رمادي", 430m },
                    { 9, "pants", "خامة ناعمة مع استرتش عملي.", "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80", "بنطلون رياضي أسود", 450m },
                    { 10, "pants", "مناسب للجامعة والعمل والخروجات.", "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80", "بنطلون تشينو كحلي", 620m },
                    { 11, "pants", "لون هادئ وسهل التنسيق.", "https://images.unsplash.com/photo-1551232864-3f0890e580d9?auto=format&fit=crop&w=900&q=80", "بنطلون تشينو بيج فاتح", 610m },
                    { 12, "pants", "إطلالة رسمية أنيقة بقصة مستقيمة.", "https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=900&q=80", "بنطلون formal أسود", 710m },
                    { 13, "pants", "مناسب للمناسبات والاجتماعات.", "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80", "بنطلون formal رمادي", 705m },
                    { 14, "pants", "قصة واسعة بلمسة عصرية.", "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80", "بنطلون wide leg بني", 670m },
                    { 15, "pants", "مظهر ناعم مع راحة طوال اليوم.", "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80", "بنطلون wide leg أوف وايت", 675m },
                    { 16, "pants", "تفاصيل ripped لستايل شبابي.", "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?auto=format&fit=crop&w=900&q=80", "بنطلون ripped denim", 650m },
                    { 17, "pants", "قصة مريحة ولوك trendy.", "https://images.unsplash.com/photo-1554412933-514a83d2f3c8?auto=format&fit=crop&w=900&q=80", "بنطلون mom fit أزرق", 620m },
                    { 18, "pants", "مقاس slim بخامة خفيفة.", "https://images.unsplash.com/photo-1487412947147-5cebf100ffc2?auto=format&fit=crop&w=900&q=80", "بنطلون slim بيج", 595m },
                    { 19, "pants", "ستايل أنيق ومناسب لكل الأوقات.", "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80", "بنطلون slim كحلي", 605m },
                    { 20, "pants", "خامة تنفسية للمواسم الحارة.", "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80", "بنطلون summer relaxed", 560m },
                    { 21, "jackets", "مناسب للخروجات واللوك الأنيق.", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=900&q=80", "جاكيت أسود خفيف", 780m },
                    { 22, "jackets", "قطعة أساسية لستايل كاجوال جريء.", "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=900&q=80", "جاكيت دنيم أزرق", 860m },
                    { 23, "jackets", "تفاصيل بسيطة بلمسة فخمة.", "https://images.unsplash.com/photo-1523398002811-999ca8dec234?auto=format&fit=crop&w=900&q=80", "جاكيت أوفر سايز بني", 910m },
                    { 24, "jackets", "ستايل bomber عصري ومريح.", "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80", "جاكيت bomber أسود", 840m },
                    { 25, "jackets", "لون مميز يناسب الخروجات اليومية.", "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80", "جاكيت bomber زيتي", 845m },
                    { 26, "jackets", "خامة قوية ولمعة أنيقة.", "https://images.unsplash.com/photo-1507679799987-c73779587ccf?auto=format&fit=crop&w=900&q=80", "جاكيت leather أسود", 1150m },
                    { 27, "jackets", "مظهر كلاسيكي بلمسة عصرية.", "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80", "جاكيت leather بني", 1180m },
                    { 28, "jackets", "دافئ وخفيف في نفس الوقت.", "https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=900&q=80", "جاكيت padded رمادي", 980m },
                    { 29, "jackets", "مثالي للبرد الخفيف.", "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=900&q=80", "جاكيت padded كحلي", 995m },
                    { 30, "jackets", "ستايل عملي مع جيوب متعددة.", "https://images.unsplash.com/photo-1551232864-3f0890e580d9?auto=format&fit=crop&w=900&q=80", "جاكيت utility بيج", 890m },
                    { 31, "jackets", "عملي ومرن للتنسيقات اليومية.", "https://images.unsplash.com/photo-1487412947147-5cebf100ffc2?auto=format&fit=crop&w=900&q=80", "جاكيت utility أسود", 900m },
                    { 32, "jackets", "قصة قصيرة بستايل شبابي.", "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?auto=format&fit=crop&w=900&q=80", "جاكيت cropped denim", 870m },
                    { 33, "jackets", "روح شبابية مستوحاة من varsity.", "https://images.unsplash.com/photo-1554412933-514a83d2f3c8?auto=format&fit=crop&w=900&q=80", "جاكيت varsity أسود", 940m },
                    { 34, "jackets", "مريح ومميز للجامعة والخروجات.", "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=900&q=80", "جاكيت varsity رمادي", 950m },
                    { 35, "jackets", "خامة صوفية بلمسة ناعمة.", "https://images.unsplash.com/photo-1514996937319-344454492b37?auto=format&fit=crop&w=900&q=80", "جاكيت صوفي بيج", 1020m },
                    { 36, "jackets", "إحساس دافئ ومظهر هادئ.", "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80", "جاكيت صوفي زيتي", 1030m },
                    { 37, "jackets", "لون ناعم وخامة عملية.", "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80", "جاكيت خفيف كريمي", 790m },
                    { 38, "jackets", "سهل التنسيق مع معظم الإطلالات.", "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80", "جاكيت خفيف أزرق غامق", 800m },
                    { 39, "jackets", "أنيق وخفيف للمظهر الراقي.", "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=900&q=80", "جاكيت formal رمادي", 1120m },
                    { 40, "jackets", "مناسب للمشاوير المهمة والمناسبات.", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=900&q=80", "جاكيت formal أسود", 1140m },
                    { 41, "tshirts", "قماش مريح ولمسة عصرية مناسبة لكل يوم.", "https://images.unsplash.com/photo-1523398002811-999ca8dec234?auto=format&fit=crop&w=900&q=80", "تيشيرت أبيض أوفر سايز", 450m },
                    { 42, "tshirts", "ستايل نظيف وسهل التنسيق.", "https://images.unsplash.com/photo-1576566588028-4147f3842f27?auto=format&fit=crop&w=900&q=80", "تيشيرت أسود سادة", 430m },
                    { 43, "tshirts", "قطعة يومية بخامة ناعمة.", "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?auto=format&fit=crop&w=900&q=80", "تيشيرت بيج basic", 420m },
                    { 44, "tshirts", "لون هادئ وسهل مع الجينز.", "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=900&q=80", "تيشيرت كحلي basic", 425m },
                    { 45, "tshirts", "طباعة أمامية لستايل شبابي.", "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=900&q=80", "تيشيرت مطبوع front", 470m },
                    { 46, "tshirts", "طباعة خلفية بلمسة streetwear.", "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=900&q=80", "تيشيرت مطبوع back", 480m },
                    { 47, "tshirts", "خامة أثقل ومظهر premium.", "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=900&q=80", "تيشيرت أوف وايت heavy", 510m },
                    { 48, "tshirts", "لون مميز للمشاوير اليومية.", "https://images.unsplash.com/photo-1517841905240-472988babdf9?auto=format&fit=crop&w=900&q=80", "تيشيرت أخضر زيتوني", 455m },
                    { 49, "tshirts", "عملي ومريح طوال اليوم.", "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=900&q=80", "تيشيرت رمادي ملانج", 440m },
                    { 50, "tshirts", "قصة slim لإطلالة أنظف.", "https://images.unsplash.com/photo-1554412933-514a83d2f3c8?auto=format&fit=crop&w=900&q=80", "تيشيرت fit slim أبيض", 460m },
                    { 51, "tshirts", "ستايل بسيط بمظهر مرتب.", "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?auto=format&fit=crop&w=900&q=80", "تيشيرت fit slim أسود", 465m },
                    { 52, "tshirts", "خطوط كلاسيكية ولمسة عصرية.", "https://images.unsplash.com/photo-1495385794356-15371f348c31?auto=format&fit=crop&w=900&q=80", "تيشيرت striped أبيض وأسود", 490m },
                    { 53, "tshirts", "خطوط ناعمة وتنسيق سهل.", "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=900&q=80", "تيشيرت striped أزرق", 495m },
                    { 54, "tshirts", "ستايل واسع مناسب للوك casual.", "https://images.unsplash.com/photo-1507679799987-c73779587ccf?auto=format&fit=crop&w=900&q=80", "تيشيرت oversize بني", 500m },
                    { 55, "tshirts", "خامة ناعمة وقصة مريحة.", "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?auto=format&fit=crop&w=900&q=80", "تيشيرت oversize رمادي", 505m },
                    { 56, "tshirts", "لون صيفي ولمسة مرحة.", "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=900&q=80", "تيشيرت summer yellow", 435m },
                    { 57, "tshirts", "خفيف ومبهج للأجواء الصيفية.", "https://images.unsplash.com/photo-1445205170230-053b83016050?auto=format&fit=crop&w=900&q=80", "تيشيرت summer pink", 438m },
                    { 58, "tshirts", "لوجو بسيط على الصدر.", "https://images.unsplash.com/photo-1514996937319-344454492b37?auto=format&fit=crop&w=900&q=80", "تيشيرت minimal logo", 470m },
                    { 59, "tshirts", "طابع vintage بطباعة مميزة.", "https://images.unsplash.com/photo-1551232864-3f0890e580d9?auto=format&fit=crop&w=900&q=80", "تيشيرت vintage print", 520m },
                    { 60, "tshirts", "قطعة أساسية للبس اليومي.", "https://images.unsplash.com/photo-1487412947147-5cebf100ffc2?auto=format&fit=crop&w=900&q=80", "تيشيرت daily essential", 410m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category_Id",
                table: "Products",
                columns: new[] { "Category", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
