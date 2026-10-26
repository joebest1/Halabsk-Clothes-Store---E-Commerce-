const cartItemsContainer = document.querySelector("#cart-items");
const cartCountElement = document.querySelector("#cart-count");
const cartTotalElement = document.querySelector("#cart-total");
const checkoutButton = document.querySelector("#proceed-to-payment");

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
          <td>
            <div class="cart-product">
              <img src="${item.image}" alt="${item.name}" class="cart-product-image" loading="lazy" />
              <span class="cart-product-name">${item.name}</span>
            </div>
          </td>
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

  if (checkoutButton) {
    const isEmpty = !cart.items.length;
    checkoutButton.classList.toggle("is-disabled", isEmpty);
    checkoutButton.setAttribute("aria-disabled", isEmpty ? "true" : "false");
    checkoutButton.dataset.empty = isEmpty ? "true" : "false";
  }

  window.halabsk.updateNavCartCount(cart.count);
}

checkoutButton?.addEventListener("click", (event) => {
  if (checkoutButton.dataset.empty === "true") {
    event.preventDefault();
    window.halabsk.showToast("السلة فاضية");
  }
});

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
      response = quantity <= 1
        ? await window.halabsk.requestJson(`/api/cart/items/${productId}`, {
            method: "DELETE"
          })
        : await window.halabsk.requestJson(`/api/cart/items/${productId}`, {
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
