const cartItemsContainer = document.querySelector("#cart-items");
const cartCountElement = document.querySelector("#cart-count");
const cartTotalElement = document.querySelector("#cart-total");

if (cartItemsContainer) {
  renderCart();
}

async function renderCart() {
  try {
    const cart = await window.halabsk.fetchCart();

    if (!cart.items.length) {
      cartItemsContainer.innerHTML = `
        <tr>
          <td colspan="5" class="empty-cart">
            لم يتم اختيار منتجات بعد.
          </td>
        </tr>
      `;

      updateSummary(cart);
      return;
    }

    cartItemsContainer.innerHTML = cart.items
      .map((item) => `
        <tr>
          <td>${item.name}</td>
          <td>${item.price} EGP</td>
          <td>
            <div class="qty-box">
              <button class="qty-btn minus" data-id="${item.id}" data-quantity="${item.quantity}">-</button>
              <span>${item.quantity}</span>
              <button class="qty-btn plus" data-id="${item.id}" data-quantity="${item.quantity}">+</button>
            </div>
          </td>
          <td>${item.lineTotal} EGP</td>
          <td>
            <button class="remove-btn" data-id="${item.id}">
              ❌
            </button>
          </td>
        </tr>
      `)
      .join("");

    updateSummary(cart);
  } catch {
    cartItemsContainer.innerHTML = `
      <tr>
        <td colspan="5" class="empty-cart">تعذر تحميل السلة.</td>
      </tr>
    `;
  }
}

function updateSummary(cart) {
  if (cartCountElement) {
    cartCountElement.textContent = cart.count;
  }

  if (cartTotalElement) {
    cartTotalElement.textContent = `${cart.total} EGP`;
  }

  window.halabsk.updateNavCartCount(cart.count);
}

cartItemsContainer?.addEventListener("click", async (event) => {
  const button = event.target.closest("button");
  if (!button) return;

  const productId = Number(button.dataset.id);
  const quantity = Number(button.dataset.quantity || 0);

  if (!productId) return;

  button.disabled = true;

  try {
    let response;

    if (button.classList.contains("plus")) {
      response = await window.halabsk.requestJson(`/api/cart/items/${productId}`, {
        method: "PATCH",
        body: JSON.stringify({ quantity: quantity + 1 })
      });
    } else if (button.classList.contains("minus")) {
      response = await window.halabsk.requestJson(`/api/cart/items/${productId}`, {
        method: "PATCH",
        body: JSON.stringify({ quantity: quantity - 1 })
      });
    } else if (button.classList.contains("remove-btn")) {
      response = await window.halabsk.requestJson(`/api/cart/items/${productId}`, {
        method: "DELETE"
      });
    }

    if (response?.cart) {
      updateSummary(response.cart);
      await renderCart();
    }
  } catch (error) {
    window.halabsk.showToast(error.message || "تعذر تحديث السلة");
  } finally {
    button.disabled = false;
  }
});
