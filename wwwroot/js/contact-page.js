const contactForm = document.querySelector("#contact-form");

if (contactForm) {
  const messageElement = document.querySelector("#contact-form-message");

  function setContactMessage(text, type) {
    if (!messageElement) return;
    messageElement.textContent = text;
    messageElement.className = `form-message ${type === "success" ? "is-success" : "is-error"}`;
  }

  contactForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const payload = {
      contactName: contactForm.querySelector("#contact-name")?.value.trim(),
      contactEmail: contactForm.querySelector("#contact-email")?.value.trim(),
      contactMessage: contactForm.querySelector("#contact-message")?.value.trim()
    };

    if (!payload.contactName || !payload.contactEmail || !payload.contactMessage) {
      setContactMessage("اكمل البيانات", "error");
      return;
    }

    try {
      const result = await window.halabsk.requestJson("/api/contact", {
        method: "POST",
        body: JSON.stringify(payload)
      });

      setContactMessage(result.message, "success");
      contactForm.reset();
    } catch (error) {
      setContactMessage(error.message || "تعذر إرسال الرسالة", "error");
    }
  });
}
