# BookStore

Description
This project is a BookStore application built with MVC .NET. It provides various features for both users and administrators, including user registration, login, book browsing, shopping cart, order management, and more. The system also allows the admin to manage books, categories, authors, and orders through an intuitive dashboard.

Features
User Features:
User Registration & Login: Secure user registration and login functionality.

Book Browsing: Browse available books by categories, authors, or directly from the home page.

Search: Users can search for books by title, author, or category.

Book Details Page: View detailed information about each book.

Shopping Cart: Add books to the shopping cart, view cart items, and modify quantities.

Checkout: Proceed with checkout, providing shipping information and payment options.

Order Management: Users can view their order history and order status.

Wish List: Save books to the wish list for future purchases.

Favorites: Add books to favorites for easy access later.

User Profile: Edit and view user profile information.

Admin Features:
Admin Dashboard: A comprehensive dashboard to view overall statistics and manage the store.

Book Management: Add, update, or delete books from the store catalog.

Category Management: Create and manage book categories.

Author Management: Manage authors associated with books.

Order Management: View and manage customer orders, update order statuses.

Technologies Used
ASP.NET MVC for the main structure.

Entity Framework for database management.

SQL Server for the database.

Bootstrap for responsive design.

Setup
Prerequisites
.NET SDK (version 8 or higher)

SQL Server

Visual Studio (or Visual Studio Code)

A database setup for the BookStore (use the provided scripts for initial setup).

Installation
Clone this repository:

bash
Copy
Edit
git clone <repository_url>
Restore the NuGet packages:

bash
Copy
Edit
dotnet restore
Update the connection string in appsettings.json to match your database configuration.

Apply the migrations to set up the database schema:

bash
Copy
Edit
dotnet ef database update
Run the application:

bash
Copy
Edit
dotnet run
Access the application via http://localhost:5000.

Contributing
Feel free to fork the repository and submit pull requests. All contributions are welcome!
