# Transaction API

A RESTful API for a transaction data system built with .NET 8, C#, and PostgreSQL. This project implements a layered architecture with API, Services, and Infrastructure layers, following SOLID principles, OOP patterns like Polymorphism and generics. It utilizes the repository pattern with unit of work and the AutoMapper library.

## Project Purpose

This DEMO API provides endpoints for managing users and transactional actions in an transaction data system. It allows for:

Database operations
- User management (CRUD operations)
- Recording financial transactions (debits and credits)

Complex logic operations
- Generating reports on transactions per user and per transaction type as well as identify transactions above a certain threshold amount

The system is designed with clean architecture principles, making it maintainable, testable, and scalable.

## Technologies Used

- .NET 8
- C#
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Swagger/OpenAPI
- xUnit (for testing)
- Moq (for mocking in tests)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- Visual Studio 2022 or another IDE that supports .NET development

## Setup Instructions

### 1. Clone the Repository

\`\`\`bash
git clone https://github.com/devopan/TransactionDataSystem.git
cd transaction-data-system
\`\`\`

### 2. Database Configuration

Update the connection string in `TransactionDataSystem.Web/appsettings.json` with your PostgreSQL credentials:

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=transaction-data-system;Username=your_username;Password=your_password"
  }
}
\`\`\`

### 3. Database Migrations

There are two ways to run the database migrations:

#### Option 1: Using the .NET CLI

Navigate to the Infrastructure project directory and run:

\`\`\`bash
cd TransactionDataSystem.Infrastructure
dotnet ef database update --project TransactionDataSystem.Infrastructure -s TransactionDataSystem.Web
\`\`\`

#### Option 2: Using the Package Manager Console in Visual Studio

1. Open the solution in Visual Studio
2. Select `TransactionDataSystem.Infrastructure` as the Default Project in Package Manager Console
3. Run the following command:

\`\`\`
Update-Database -StartupProject TransactionDataSystem.Web
\`\`\`

#### Option 3: Automatic Migrations

The application is configured to apply pending migrations automatically when it starts. Simply run the application, and the migrations will be applied.

### 4. Building and Running the Application

#### Using .NET CLI

\`\`\`bash
cd TransactionDataSystem.Web
dotnet build
dotnet run
\`\`\`

#### Using Visual Studio

1. Set `TransactionDataSystem.Web` as the startup project
2. Press F5 or click the "Run" button

### 5. Accessing the API

Once the application is running, you can access:

- API endpoints at `https://localhost:7101/api/`
- Swagger documentation at `https://localhost:7101/swagger`

## API Endpoints

### Users

- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get a specific user
- `POST /api/users` - Create a new user
- `PUT /api/users/{id}` - Update a user
- `DELETE /api/users/{id}` - Delete a user

### Transactions

- `GET /api/transactions/{id}` - Get a specific transaction
- `POST /api/transactions` - Create a new transaction

### Reporting

- `GET /api/reporting/users/transactions` - Get total transactions per user
- `GET /api/reporting/transactions/types` - Get total transactions per transactions type
- `GET /api/reporting/high-volume-transactions?from={dd/MM/yyyy}&to={dd/MM/yyyy}&limit={number}&groupBy={user|transactiontype}` - Get transaction IDs that exceed the specified high volume limit within the date range for the grouping specified

## Error Handling

The API implements global exception handling to provide consistent error responses:

- All exceptions are caught by the global exception handling middleware
- Specific error types (like NotFound, BadRequest) return appropriate HTTP status codes
- PostgreSQL database errors are translated to user-friendly messages

## Creating Custom Migrations

If you need to create a new migration after modifying the entity models, use the following commands:

### Using .NET CLI

\`\`\`bash
cd TransactionDataSystem.Infrastructure
dotnet ef migrations add MigrationName --project TransactionDataSystem.Infrastructure -s TransactionDataSystem.Web
dotnet ef database update --project TransactionDataSystem.Infrastructure -s TransactionDataSystem.Web
\`\`\`

### Using Package Manager Console in Visual Studio

\`\`\`
Add-Migration MigrationName -Project TransactionDataSystem.Infrastructure -StartupProject TransactionDataSystem.Web
Update-Database -Project TransactionDataSystem.Infrastructure -StartupProject TransactionDataSystem.Web
\`\`\`

## Testing

The solution includes both unit tests and integration tests to ensure code quality and functionality check.

### Unit Tests

The `TransactionDataSystem.Services.Test.Unit` project contains unit tests for the service layer using xUnit and Moq for mocking dependencies. These tests focus on testing individual components in isolation.

#### Running Unit Tests

\`\`\`bash
cd TransactionDataSystem.Services.Test.Unit
dotnet test
\`\`\`

### Integration Tests

The `TransactionDataSystem.Web.Test.Integration` project contains integration tests that test the API endpoints end-to-end. These tests use the `WebApplicationFactory` to create a test server with an in-memory database.

#### Running Integration Tests

\`\`\`bash
cd TransactionDataSystem.Web.Test.Integration
dotnet test
\`\`\`

#### Integration Test Setup

The integration tests use:
- An in-memory database for testing
- Custom `WebApplicationFactory` to configure the test environment
- Seeded test data for consistent test results

## Design Principles

This project follows:

- **SOLID Principles**: Single responsibility, Open-closed, Liskov substitution, Interface segregation, and Dependency inversion
- **Command Query Segregation Principle (CQRS)**: Separate models for reading and writing data
- **Repository Pattern with Unit of Work**: Abstraction over data access with transaction support
- **Layered Architecture**: Clear separation of concerns between API, Services, and Infrastructure layers
- **Exception Handling**: Global exception handling with specific error types
