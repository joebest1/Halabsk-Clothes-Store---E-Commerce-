const courseProductsContainer = document.querySelector("#course-products");

if (courseProductsContainer) {
  loadAllProducts();
}

async function loadAllProducts() {
  try {
    const products = await window.halabsk.requestJson("/api/products");
    renderAllProducts(products);
  } catch {
    courseProductsContainer.innerHTML = "<p>تعذر تحميل المنتجات</p>";
  }
}

function renderAllProducts(items) {
  if (!items.length) {
    courseProductsContainer.innerHTML = "<p>لا توجد منتجات متاحة الآن.</p>";
    return;
  }

  courseProductsContainer.innerHTML = items
    .map((item) => `
      <article class="product-card">
        <img src="${item.image}" alt="${item.name}" class="product-image" />
        <h3>${item.name}</h3>
        <p>${item.description}</p>
        <span class="price">${item.price} EGP</span>
        <button class="primary-button add-to-cart-button" data-id="${item.id}">
          أضف للسلة 🛒
        </button>
      </article>
    `)
    .join("");
}

courseProductsContainer?.addEventListener("click", async (event) => {
  const button = event.target.closest(".add-to-cart-button");
  if (!button) return;

  button.disabled = true;

  try {
    await window.halabsk.addToCart(Number(button.dataset.id));
  } catch (error) {
    window.halabsk.showToast(error.message || "تعذر إضافة المنتج");
  } finally {
    button.disabled = false;
  }
});
