const checkoutForm = document.querySelector("#checkout-form");
const checkoutItems = document.querySelector("#checkout-items");
const checkoutTotal = document.querySelector("#checkout-total");
const checkoutSubmit = document.querySelector("#checkout-submit");
const checkoutMessage = document.querySelector("#checkout-message");

let checkoutCart = null;

if (checkoutForm) {
  loadCheckout();
}

async function loadCheckout() {
  try {
    checkoutCart = await window.halabsk.fetchCart();
    renderCheckout(checkoutCart);
  } catch {
    checkoutItems.innerHTML = '<p class="empty-cart">تعذر تحميل السلة.</p>';
    setCheckoutMessage("تعذر تحميل السلة، حاول تحديث الصفحة.", "error");
    checkoutSubmit.disabled = true;
  }
}

function renderCheckout(cart) {
  if (!cart.items.length) {
    checkoutItems.innerHTML = `
      <p class="empty-cart">
        السلة فاضية.
        <a href="/products">تصفح المنتجات</a>
      </p>
    `;
    checkoutTotal.textContent = "0 EGP";
    checkoutSubmit.disabled = true;
    return;
  }

  checkoutItems.innerHTML = cart.items
    .map((item) => `
      <div class="checkout-item">
        <img src="${item.image}" alt="${item.name}" class="checkout-item-image" loading="lazy" />
        <div class="checkout-item-info">
          <strong>${item.name}</strong>
          <span>${item.quantity} × ${item.price} EGP</span>
        </div>
        <span class="checkout-item-total">${item.lineTotal} EGP</span>
      </div>
    `)
    .join("");

  checkoutTotal.textContent = `${cart.total} EGP`;
  checkoutSubmit.disabled = false;
}

function setCheckoutMessage(text, type) {
  if (!checkoutMessage) return;

  checkoutMessage.textContent = text;
  checkoutMessage.className = `form-message ${type === "success" ? "success-message" : "error-message"}`;
}

checkoutForm?.addEventListener("submit", async (event) => {
  event.preventDefault();

  if (!checkoutCart?.items.length) {
    setCheckoutMessage("السلة فاضية، أضف منتجات الأول.", "error");
    return;
  }

  const name = checkoutForm.querySelector("#checkout-name")?.value.trim() || "";
  const phone = checkoutForm.querySelector("#checkout-phone")?.value.trim() || "";
  const address = checkoutForm.querySelector("#checkout-address")?.value.trim() || "";

  if (name.length < 3) {
    setCheckoutMessage("اكتب الاسم بالكامل.", "error");
    return;
  }

  if (!/^01\d{9}$/.test(phone)) {
    setCheckoutMessage("اكتب رقم موبايل صحيح (11 رقم يبدأ بـ 01).", "error");
    return;
  }

  if (address.length < 5) {
    setCheckoutMessage("اكتب العنوان بالتفصيل.", "error");
    return;
  }

  checkoutSubmit.disabled = true;
  checkoutSubmit.textContent = "جارٍ تأكيد الطلب...";
  setCheckoutMessage("", "success");

  try {
    await window.halabsk.requestJson("/api/cart", { method: "DELETE" });

    window.halabsk.updateNavCartCount(0);
    checkoutCart = null;
    checkoutItems.innerHTML = '<p class="empty-cart">تم تأكيد الطلب.</p>';
    checkoutTotal.textContent = "0 EGP";

    checkoutForm.innerHTML = `
      <div class="checkout-success">
        <span class="checkout-success-icon">✓</span>
        <h3>تم استلام طلبك 🎉</h3>
        <p>هنكلمك على الرقم اللي كتبته لتأكيد التوصيل. الدفع عند الاستلام.</p>
        <a href="/" class="primary-button link-button">متابعة التسوق</a>
      </div>
    `;
  } catch (error) {
    checkoutSubmit.disabled = false;
    checkoutSubmit.textContent = "تأكيد الطلب";
    setCheckoutMessage(error.message || "تعذر تأكيد الطلب، حاول تاني.", "error");
  }
});
