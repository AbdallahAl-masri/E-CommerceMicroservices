# Project Name
E-Commerce Microservices

## Overview
This repository contains an e-commerce system built using .NET 8 with a microservices architecture. The system includes several services such as Authentication, Product, and Order services. It features secure communication using JWT authentication, API Gateway (Ocelot), and middleware enhancements for request validation and exception handling. The database is managed with Entity Framework Core, and the system is designed with RESTful APIs.

## Features
- **Microservices Architecture**: Modular services for Authentication, Product, and Order management.
- **Secure Communication**: JWT authentication for secure service-to-service communication.
- **API Gateway**: Ocelot API Gateway for routing and aggregation.
- **Middleware Enhancements**: Improved request validation and exception handling.
- **Database Management**: Entity Framework Core for managing database operations.
- **RESTful APIs**: Designed using REST principles for scalability and ease of use.

  ## 📜 API Documentation

### 🔹 Swagger
Each microservice has a Swagger UI:
- **Authentication**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **Products**: [http://localhost:5001/swagger](http://localhost:5001/swagger)
- **Orders**: [http://localhost:5002/swagger](http://localhost:5002/swagger)

### 🔹 Postman Collection
Import the Postman collection to test APIs:  
[Download Postman Collection](./ecommerce-api.postman_collection.json)

## Services
1. **Authentication Service**:
    Manages user authentication and authorization.
    Uses JWT for secure communication.
2. **Product Service**:
    Handles product-related operations such as adding, updating, and retrieving products.
3. **Order Service**:
    Manages order processing and tracking.

## Technologies
- **.NET 8**: Backend framework.
- **C#**: Programming language.
- **Entity Framework Core**: ORM for database management.
- **Ocelot**: API Gateway for managing API requests.
- **JWT**: JSON Web Tokens for secure authentication.
    
## Installation
1. Clone the repository:
        git clone https://github.com/AbdallahAl-masri/E-CommerceMicroservices.git
        cd E-CommerceMicroservices
2. Restore dependencies:
       dotnet restore
3. Update the database:
       dotnet ef database update
4. Run the services:
        dotnet run

## API Endpoints
### Product API
- **GET /api/product**: Retrieve all products.
- **GET /api/product/{id}**: Retrieve a product by ID.
- **POST /api/product**: Create a new product (Admin only).
- **PUT /api/product**: Update an existing product (Admin only).
- **DELETE /api/product**: Delete a product (Admin only).

### Order API
- **GET /api/order**: Retrieve all orders.
- **GET /api/order/{id}**: Retrieve an order by ID.
- **POST /api/order**: Create a new order.
- **PUT /api/order**: Update an existing order.
- **DELETE /api/order**: Delete an order.

## Middleware
### GlobalException
Handles exceptions and returns appropriate error messages and status codes.

### OnlyListenToAPIGateway
Ensures that requests are only processed if they come from the API Gateway.

### AttachSignatureToRequest
Attaches a signature to the request for validation.

## Dependencies
- .NET 8
- Microsoft.AspNetCore.Http
- Microsoft.EntityFrameworkCore
- Serilog
- JWT Authentication
- Ocelot

## Configuration
Ensure to update the `appsettings.json` file with the appropriate connection strings and configuration settings.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request.

## License
This project is licensed under the MIT License.

    
