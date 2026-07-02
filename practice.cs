<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Product List and Cart Management</title>
    <link rel="stylesheet" href="style.css">
</head>
<body>

    <h1>Product List</h1>

    <div id="productContainer"></div>

    <h2>Cart</h2>

    <ul id="cartList"></ul>

    <script src="script.js"></script>

</body>
</html>

.product {
    margin-bottom: 20px;
}

.cart-item {
    margin-bottom: 10px;
}

button {
    margin-top: 5px;
}

interface Product {
    id: number;
    name: string;
    price: number;
}

interface CartItem {
    product: Product;
    quantity: number;
}

const products: Product[] = [
    { id: 1, name: "Product 1", price: 10 },
    { id: 2, name: "Product 2", price: 20 },
    { id: 3, name: "Product 3", price: 30 }
];

let cart: CartItem[] = [];

const productContainer = document.getElementById("productContainer") as HTMLDivElement;
const cartList = document.getElementById("cartList") as HTMLUListElement;

function displayProducts(): void {

    productContainer.innerHTML = "";

    products.forEach(function (product) {

        const productDiv = document.createElement("div");
        productDiv.className = "product";

        productDiv.innerHTML =
            `<p>${product.name}</p>
             <p>$${product.price.toFixed(2)}</p>`;

        const button = document.createElement("button");
        button.textContent = "Add to Cart";

        button.addEventListener("click", function () {
            addToCart(product.id);
        });

        productDiv.appendChild(button);
        productContainer.appendChild(productDiv);
    });
}

function addToCart(productId: number): void {

    const existingItem = cart.find(function (item) {
        return item.product.id === productId;
    });

    if (existingItem) {
        existingItem.quantity++;
    } else {

        const product = products.find(function (p) {
            return p.id === productId;
        });

        if (product) {
            cart.push({
                product: product,
                quantity: 1
            });
        }
    }

    displayCart();
}

function removeFromCart(productId: number): void {

    cart = cart.filter(function (item) {
        return item.product.id !== productId;
    });

    displayCart();
}

function displayCart(): void {

    cartList.innerHTML = "";

    cart.forEach(function (item) {

        const li = document.createElement("li");
        li.className = "cart-item";

        li.innerHTML =
            `${item.product.name} - $${item.product.price.toFixed(2)} x ${item.quantity}`;

        const removeButton = document.createElement("button");
        removeButton.textContent = "Remove";

        removeButton.addEventListener("click", function () {
            removeFromCart(item.product.id);
        });

        li.appendChild(document.createElement("br"));
        li.appendChild(removeButton);

        cartList.appendChild(li);
    });
}

displayProducts();
