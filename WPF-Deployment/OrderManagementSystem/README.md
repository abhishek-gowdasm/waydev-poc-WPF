# Order Management System API

A comprehensive ASP.NET Core Web API for managing orders, customers, products, and employees in a monolithic architecture.

## Features

- **Customer Management**: CRUD operations for customers with search and analytics
- **Order Management**: Complete order lifecycle management with status tracking
- **Product Management**: Inventory management with low stock alerts
- **Employee Management**: Employee hierarchy and territory management
- **Dashboard Analytics**: Real-time business intelligence and reporting
- **RESTful API**: Full REST API with comprehensive documentation
- **Swagger UI**: Interactive API documentation

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Architecture**: Monolithic with Clean Architecture principles
- **Mapping**: AutoMapper for object mapping
- **Documentation**: Swagger/OpenAPI
- **Patterns**: Repository Pattern, Unit of Work Pattern

## Project Structure

```
OrderManagementSystem/
├── Controllers/          # API Controllers
├── Data/                # Data Access Layer
│   ├── OrderManagementDbContext.cs
│   ├── Repository.cs
│   ├── UnitOfWork.cs
│   └── DatabaseInitializer.cs
├── DTOs/                # Data Transfer Objects
├── Interfaces/          # Service Interfaces
├── Mapping/             # AutoMapper Profiles
├── Models/              # Entity Models
├── Services/            # Business Logic Services
└── Program.cs           # Application Entry Point
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (Express or higher)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd OrderManagementSystem
   ```

2. **Update Connection String**
   - Open `appsettings.json`
   - Update the `DefaultConnection` string to match your SQL Server instance

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - Swagger UI: `https://localhost:7001` or `http://localhost:5001`
   - API Base URL: `https://localhost:7001/api`

## API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/customers/search?searchTerm={term}` - Search customers
- `GET /api/customers/country/{country}` - Get customers by country
- `GET /api/customers/top/{count}` - Get top customers
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Delete customer
- `GET /api/customers/statistics` - Get customer statistics

### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/customer/{customerId}` - Get orders by customer
- `GET /api/orders/employee/{employeeId}` - Get orders by employee
- `GET /api/orders/daterange?startDate={date}&endDate={date}` - Get orders by date range
- `GET /api/orders/search?searchTerm={term}` - Search orders
- `GET /api/orders/pending` - Get pending orders
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}` - Update order
- `DELETE /api/orders/{id}` - Delete order
- `GET /api/orders/statistics` - Get order statistics

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/supplier/{supplierId}` - Get products by supplier
- `GET /api/products/search?searchTerm={term}` - Search products
- `GET /api/products/discontinued` - Get discontinued products
- `GET /api/products/lowstock/{threshold}` - Get products with low stock
- `GET /api/products/top/{count}` - Get top products
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/products/statistics` - Get product statistics

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `GET /api/employees/territory/{territoryId}` - Get employees by territory
- `GET /api/employees/region/{regionId}` - Get employees by region
- `GET /api/employees/hiredaterange?startDate={date}&endDate={date}` - Get employees by hire date range
- `GET /api/employees/active` - Get active employees
- `GET /api/employees/title/{title}` - Get employees by title
- `GET /api/employees/search?searchTerm={term}` - Search employees
- `GET /api/employees/top/{count}` - Get top employees
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee
- `GET /api/employees/statistics` - Get employee statistics
- `GET /api/employees/hierarchy` - Get employee hierarchy

### Dashboard
- `GET /api/dashboard` - Get complete dashboard data
- `GET /api/dashboard/monthly-revenue/{year}` - Get monthly revenue
- `GET /api/dashboard/top-products/{count}` - Get top products
- `GET /api/dashboard/top-customers/{count}` - Get top customers
- `GET /api/dashboard/recent-orders/{count}` - Get recent orders

## Database Schema

The application uses the following main entities:

- **Customers**: Customer information and contact details
- **Orders**: Order management with shipping information
- **Order_Details**: Order line items with quantities and prices
- **Products**: Product catalog with inventory tracking
- **Employees**: Employee information with hierarchy support
- **Categories**: Product categorization
- **Suppliers**: Supplier information
- **Shippers**: Shipping company information

## Development

### Adding New Features

1. **Create Model**: Add entity model in `Models/` folder
2. **Create DTOs**: Add DTOs in `DTOs/` folder
3. **Create Interface**: Add service interface in `Interfaces/` folder
4. **Implement Service**: Add service implementation in `Services/` folder
5. **Create Controller**: Add API controller in `Controllers/` folder
6. **Update DbContext**: Add DbSet in `OrderManagementDbContext.cs`
7. **Add Mapping**: Update AutoMapper profile in `Mapping/AutoMapperProfile.cs`

### Code Quality

- Follow SOLID principles
- Use async/await for all database operations
- Implement proper error handling
- Add XML documentation for all public APIs
- Use meaningful variable and method names

## Deployment

### Production Deployment

1. **Update Connection String**: Use production database connection
2. **Configure Environment**: Set appropriate environment variables
3. **Build Application**: `dotnet build --configuration Release`
4. **Publish Application**: `dotnet publish --configuration Release`
5. **Deploy**: Copy published files to web server

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderManagementSystem.csproj", "./"]
RUN dotnet restore "OrderManagementSystem.csproj"
COPY . .
RUN dotnet build "OrderManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderManagementSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementSystem.dll"]
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions, please contact:
- Email: support@ordermanagementsystem.com
- Documentation: Available in Swagger UI when running the application 