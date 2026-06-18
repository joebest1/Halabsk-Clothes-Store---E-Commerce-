const productsContainer = document.querySelector("#products");
const searchInput = document.querySelector("#searchInput");
const selectedCategory = productsContainer?.dataset.category;

let allProducts = [];
let searchTimeout;

if (productsContainer && selectedCategory) {
  loadCategoryProducts();
}

async function loadCategoryProducts() {
  try {
    allProducts = await window.halabsk.requestJson(
      `/api/products?category=${encodeURIComponent(selectedCategory)}`
    );

    renderProducts(allProducts);
  } catch {
    productsContainer.innerHTML = "<p>تعذر تحميل المنتجات</p>";
  }
}

function renderProducts(items) {
  if (!items.length) {
    productsContainer.innerHTML = "<p>لا توجد نتائج مطابقة.</p>";
    return;
  }

  productsContainer.innerHTML = items
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

productsContainer?.addEventListener("click", async (event) => {
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

searchInput?.addEventListener("input", () => {
  clearTimeout(searchTimeout);

  searchTimeout = setTimeout(() => {
    const query = searchInput.value.trim().toLowerCase();
    const filtered = allProducts.filter((item) =>
      item.name.toLowerCase().includes(query)
    );

    renderProducts(filtered);
  }, 300);
});
