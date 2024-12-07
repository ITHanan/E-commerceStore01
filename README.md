# E-Commerce Store Application 🛒

Welcome to the **E-Commerce Store Application**, a simple yet powerful console-based shopping system built with C#. This application enables users to browse products, add them to a cart, checkout, and view order history, all while practicing core software development principles like OOP, generics, and JSON data handling.

---

## Features 🎯

- **User Authentication**: Login functionality using Customer ID and password.
- **Product Browsing**: View a list of available products with descriptions, prices, and stock levels.
- **Shopping Cart**: Add products to the cart, update quantities, and remove items.
- **Checkout**: Process orders, deduct product stock, and generate an order history.
- **Order History**: View past orders and their details.
- **Data Persistence**: Read and write data from/to a JSON file for seamless state management.
- **Error Handling**: Robust exception handling ensures smooth execution.
- **Styling**: Enhanced terminal interface with ASCII banners (Figgle) and vibrant styling (Spectre.Console).

---

## Technology Stack 💻

- **Language**: C#
- **Frameworks/Tools**:
  - **Spectre.Console** for interactive CLI styling.
  - **Figgle** for ASCII art headers.
  - **System.Text.Json** for JSON data handling.
  - **LINQ** for data querying.
- **Data Persistence**: JSON-based storage for products, customers, carts, and orders.

---

## Getting Started 🚀

### Prerequisites
- .NET SDK 8.0 or later.
- A terminal or console to run the application.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/ecommerce-store.git
Navigate to the project directory:
bash
نسخ الكود
cd ecommerce-store
Open the project in your preferred IDE (e.g., Visual Studio, VS Code).
Usage 🛠️
Running the Application
Build and run the project:

bash
نسخ الكود
dotnet run
Login using an existing customer:

Use the Customer ID and Password from the JSON file.
Interact with the menu to:

View products.
Add items to the cart.
View the cart.
Checkout and place orders.
View order history.
Data Structure 📂
JSON File Format
The EShoppingStore.json file stores all application data, including:

Products: List of all available products.
Customers: Customer details (ID, Name, Email, Password, Address, Order History).
Carts: Cart details for logged-in customers.
Orders: Order history for each customer.
Example JSON
json
نسخ الكود
{
  "products": [
    {
      "Id": 1,
      "Name": "Laptop",
      "Description": "High-performance laptop",
      "Price": 1200.50,
      "Stock": 10
    }
  ],
  "customers": [
    {
      "CustomerId": 1,
      "Name": "John Doe",
      "Email": "john.doe@example.com",
      "Password": "12345",
      "Address": "123 Elm Street",
      "OrderHistory": []
    }
  ],
  "carts": [],
  "orders": []
}
Key Functionalities 🔑
1. Login and Authentication
Customers must log in with a valid ID and password before accessing features.
2. Add to Cart
Add products to the cart by selecting the product ID and quantity.
3. Checkout
Confirm orders, deduct product stock, and generate order history.
4. View Order History
Display all past orders for the logged-in customer.
5. JSON Persistence
All updates (e.g., stock changes, cart updates, order creation) are saved back to the EShoppingStore.json file.
Design Principles 🛠️
Object-Oriented Programming (OOP): Core entities like Customer, Product, Cart, and Order encapsulate logic and data.
Clean Code: Methods and classes are well-structured and readable.
Error Handling: Extensive use of try-catch blocks for robustness.
Interactive CLI: Leverages Spectre.Console for intuitive navigation and vibrant UI.
Screenshots 📸
Main Menu

View Products

Contributing 🤝
Contributions are welcome! Feel free to fork this repository, make improvements, and submit a pull request. Ensure your changes adhere to the following guidelines:

Follow clean coding standards.
Write meaningful commit messages.
Test your changes thoroughly.
License 📜
This project is licensed under the MIT License. See the LICENSE file for details.

Acknowledgments 🙌
Spectre.Console for the beautiful CLI styling.
Figgle for ASCII art banners.
System.Text.Json for handling JSON with ease.
