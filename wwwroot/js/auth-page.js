const authForm = document.querySelector("#auth-form");

if (authForm) {
  const mode = authForm.dataset.mode;
  const message = document.querySelector("#form-message");
  const submitButton = authForm.querySelector("#submit-button");

  function setMessage(text, type) {
    if (!message) return;
    message.textContent = text;
    message.className = `form-message ${type === "success" ? "is-success" : "is-error"}`;
  }

  authForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const email = authForm.querySelector("#email")?.value.trim();
    const password = authForm.querySelector("#password")?.value;

    if (!email || !password) {
      setMessage("اكمل البيانات", "error");
      return;
    }

    const payload = mode === "login"
      ? {
          email,
          password
        }
      : {
          fullName: authForm.querySelector("#fullName")?.value.trim(),
          email,
          password,
          confirmPassword: authForm.querySelector("#confirmPassword")?.value,
          acceptTerms: authForm.querySelector('input[name="terms"]')?.checked || false
        };

    if (mode === "signup") {
      if (!payload.fullName || !payload.confirmPassword) {
        setMessage("اكمل البيانات", "error");
        return;
      }

      if (payload.password !== payload.confirmPassword) {
        setMessage("كلمتا المرور غير متطابقتين", "error");
        return;
      }
    }

    submitButton.disabled = true;

    try {
      const result = await window.halabsk.requestJson(`/api/account/${mode}`, {
        method: "POST",
        body: JSON.stringify(payload)
      });

      setMessage(result.message, "success");

      if (mode === "login") {
        setTimeout(() => {
          window.location.href = "/";
        }, 800);
      } else {
        authForm.reset();
      }
    } catch (error) {
      setMessage(error.message || "تعذر إرسال البيانات", "error");
    } finally {
      submitButton.disabled = false;
    }
  });
}
