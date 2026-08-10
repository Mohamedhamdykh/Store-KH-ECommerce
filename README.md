🛒 Store KH – E-Commerce Platform

A full-stack e-commerce application built with ASP.NET Core Web API and Angular, providing a complete shopping experience with product browsing, authentication, basket management, order processing, and online payments.

🚀 Live Demo

* Frontend: Coming Soon
* Backend API: http://khalifa-store-apis.runasp.net/

✨ Features

* 🛍️ Product browsing and product details
* 🔎 Product search
* 🏷️ Filter products by brand and type
* ↕️ Sort products by name and price
* 📄 Pagination
* 🔐 JWT Authentication & Authorization
* 🛒 Shopping basket management
* 📦 Order creation and order history
* 🚚 Delivery method selection
* 💳 Stripe payment integration
* ⚡ Redis caching
* 📚 Swagger / OpenAPI documentation

🏗️ Architecture

The backend follows a Clean Architecture approach with clear separation of concerns.

Store.KH
│
├── Store.KH.APIs
├── Store.KH.Core
├── Store.KH.Repository
└── client

Backend Layers

* API Layer – Controllers, middleware, filters and API configuration.
* Core Layer – Entities, DTOs, interfaces and specifications.
* Repository Layer – Entity Framework Core, repositories, Unit of Work and database configuration.
* Client – Angular frontend application.

🛠️ Technologies

Backend

* ASP.NET Core 8 Web API
* C#
* Entity Framework Core
* SQL Server
* LINQ
* JWT
* AutoMapper
* Specification Pattern
* Repository Pattern
* Unit of Work
* Redis
* Stripe
* Swagger / OpenAPI

Frontend

* Angular
* TypeScript
* HTML5
* SCSS
* Bootstrap
* RxJS

Tools

* Visual Studio
* Visual Studio Code
* Git & GitHub
* Postman
* SQL Server

🔐 Security

Sensitive configuration such as database credentials, Redis credentials, JWT keys, and Stripe secret keys are managed outside the public repository.

📌 API Endpoints

The API provides endpoints for:

* Products
* Brands
* Product Types
* Accounts
* Basket
* Orders
* Payments

Swagger is available for API documentation and testing.

👨‍💻 Author

Mohamed Hamdy Khalifa

.NET Backend Developer | Information Systems Graduate

* GitHub: MohamedhamdyKh
* LinkedIn: Add your LinkedIn profile here

⸻

⭐ If you find this project useful, feel free to star the repository.
