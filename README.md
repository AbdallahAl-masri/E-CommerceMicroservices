# Project Name
E-Commerce Microservices

## Overview
This project is a .NET 8 application that includes various services and middleware components. It is designed to handle authentication, product management, order management, and API gateway functionalities.

## Projects
- **APIGateway**: Manages routing and request forwarding using Ocelot.
- **Authentication.API**: Handles user authentication and authorization.
- **Product.API**: Manages product-related operations.
- **Order.API**: Manages order-related operations.

## Features
- **Global Exception Handling**: Middleware to handle exceptions and provide meaningful error messages.
- **JWT Authentication**: Secure authentication using JSON Web Tokens.
- **Product Management**: API endpoints to manage products, including CRUD operations.
- **Order Management**: API endpoints to manage orders, including CRUD operations.
- **API Gateway**: Routes and forwards requests to appropriate services.

## Installation
1. Clone the repository:
    git clone https://github.com/your-repo/project-name.git
2. Navigate to the project directory:
    cd project-name
3. Restore the dependencies:
    dotnet restore

## Usage
1. Build the project:
    dotnet build
2. Run the project:
    dotnet run --project APIGateway
    dotnet run --project Authentication.API
    dotnet run --project Product.API
    dotnet run --project Order.API

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

    
