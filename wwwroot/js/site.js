async function requestJson(url, options = {}) {
  const headers = {
    ...(options.body ? { "Content-Type": "application/json" } : {}),
    ...options.headers
  };

  const response = await fetch(url, {
    ...options,
    headers
  });

  const text = await response.text();
  const data = text ? JSON.parse(text) : null;

  if (!response.ok) {
    const error = new Error(data?.message || "حدث خطأ غير متوقع");
    error.status = response.status;
    error.payload = data;
    throw error;
  }

  return data;
}

function updateNavCartCount(count) {
  document.querySelectorAll("#nav-cart-count").forEach((element) => {
    element.textContent = count;
  });
}

function showToast(message) {
  const toast = document.createElement("div");
  toast.className = "toast";
  toast.textContent = message;

  document.body.appendChild(toast);

  setTimeout(() => {
    toast.classList.add("show");
  }, 50);

  setTimeout(() => {
    toast.classList.remove("show");

    setTimeout(() => {
      toast.remove();
    }, 300);
  }, 2000);
}

async function fetchCart() {
  return requestJson("/api/cart");
}

async function refreshNavCartCount() {
  try {
    const cart = await fetchCart();
    updateNavCartCount(cart.count || 0);
    return cart;
  } catch {
    return null;
  }
}

async function addToCart(productId) {
  const data = await requestJson("/api/cart/items", {
    method: "POST",
    body: JSON.stringify({ productId })
  });

  updateNavCartCount(data.cart?.count || 0);

  if (data.message) {
    showToast(data.message);
  }

  return data.cart;
}

window.halabsk = {
  requestJson,
  updateNavCartCount,
  showToast,
  fetchCart,
  refreshNavCartCount,
  addToCart
};

document.addEventListener("DOMContentLoaded", () => {
  window.halabsk.refreshNavCartCount();
});
