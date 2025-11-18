using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Data
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderManagementDbContext>();

            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if data already exists (check multiple tables to be sure)
            if (await context.Customers.AnyAsync() && await context.Categories.AnyAsync() && await context.Suppliers.AnyAsync())
            {
                return; // Database already seeded
            }

            // Clear any existing data to ensure clean seeding
            await ClearDatabaseAsync(context);

            // Seed sample data
            await SeedSampleDataAsync(context);
        }

        private static async Task ClearDatabaseAsync(OrderManagementDbContext context)
        {
            // Clear data in reverse order of dependencies
            context.Order_Details.RemoveRange(context.Order_Details);
            context.Orders.RemoveRange(context.Orders);
            context.Products.RemoveRange(context.Products);
            context.Employees.RemoveRange(context.Employees);
            context.Customers.RemoveRange(context.Customers);
            context.Suppliers.RemoveRange(context.Suppliers);
            context.Categories.RemoveRange(context.Categories);
            context.Shippers.RemoveRange(context.Shippers);
            
            await context.SaveChangesAsync();
        }

        private static async Task SeedSampleDataAsync(OrderManagementDbContext context)
        {
            // Add sample categories
            var categories = new List<Category>
            {
                new Category { CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
                new Category { CategoryName = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
                new Category { CategoryName = "Confections", Description = "Desserts, candies, and sweet breads" },
                new Category { CategoryName = "Dairy Products", Description = "Cheeses" },
                new Category { CategoryName = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
                new Category { CategoryName = "Meat/Poultry", Description = "Prepared meats" },
                new Category { CategoryName = "Produce", Description = "Dried fruit and bean curd" },
                new Category { CategoryName = "Seafood", Description = "Seaweed and fish" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Add sample suppliers
            var suppliers = new List<Supplier>
            {
                new Supplier { CompanyName = "Exotic Liquids", ContactName = "Charlotte Cooper", ContactTitle = "Purchasing Manager", Address = "49 Gilbert St.", City = "London", Country = "UK", Phone = "(171) 555-2222" },
                new Supplier { CompanyName = "New Orleans Cajun Delights", ContactName = "Shelley Burke", ContactTitle = "Order Administrator", Address = "P.O. Box 78934", City = "New Orleans", Country = "USA", Phone = "(100) 555-4822" },
                new Supplier { CompanyName = "Grandma Kelly's Homestead", ContactName = "Regina Murphy", ContactTitle = "Sales Representative", Address = "707 Oxford Rd.", City = "Ann Arbor", Country = "USA", Phone = "(313) 555-5735" }
            };

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();

            // Add sample customers
            var customers = new List<Customer>
            {
                new Customer { CustomerID = "ALFKI", CompanyName = "Alfreds Futterkiste", ContactName = "Maria Anders", ContactTitle = "Sales Representative", Address = "Obere Str. 57", City = "Berlin", Country = "Germany", Phone = "030-0074321" },
                new Customer { CustomerID = "ANATR", CompanyName = "Ana Trujillo Emparedados y helados", ContactName = "Ana Trujillo", ContactTitle = "Owner", Address = "Avda. de la Constitución 2222", City = "México D.F.", Country = "Mexico", Phone = "(5) 555-4729" },
                new Customer { CustomerID = "ANTON", CompanyName = "Antonio Moreno Taquería", ContactName = "Antonio Moreno", ContactTitle = "Owner", Address = "Mataderos  2312", City = "México D.F.", Country = "Mexico", Phone = "(5) 555-3932" }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            // Add sample employees
            var employees = new List<Employee>
            {
                new Employee { FirstName = "Nancy", LastName = "Davolio", Title = "Sales Representative", TitleOfCourtesy = "Ms.", BirthDate = new DateTime(1948, 12, 8), HireDate = new DateTime(1992, 5, 1), Address = "507 - 20th Ave. E. Apt. 2A", City = "Seattle", Country = "USA", HomePhone = "(206) 555-9857" },
                new Employee { FirstName = "Andrew", LastName = "Fuller", Title = "Vice President, Sales", TitleOfCourtesy = "Dr.", BirthDate = new DateTime(1952, 2, 19), HireDate = new DateTime(1992, 8, 14), Address = "908 W. Capital Way", City = "Tacoma", Country = "USA", HomePhone = "(206) 555-9482" },
                new Employee { FirstName = "Janet", LastName = "Leverling", Title = "Sales Representative", TitleOfCourtesy = "Ms.", BirthDate = new DateTime(1963, 8, 30), HireDate = new DateTime(1992, 4, 1), Address = "722 Moss Bay Blvd.", City = "Kirkland", Country = "USA", HomePhone = "(206) 555-3412" }
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();

            // Add sample products
            var products = new List<Product>
            {
                new Product { ProductName = "Chai", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "10 boxes x 20 bags", UnitPrice = 18.00m, UnitsInStock = 39, UnitsOnOrder = 0, ReorderLevel = 10, Discontinued = false },
                new Product { ProductName = "Chang", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 19.00m, UnitsInStock = 17, UnitsOnOrder = 40, ReorderLevel = 25, Discontinued = false },
                new Product { ProductName = "Aniseed Syrup", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "12 - 550 ml bottles", UnitPrice = 10.00m, UnitsInStock = 13, UnitsOnOrder = 70, ReorderLevel = 25, Discontinued = false }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // Add sample shippers
            var shippers = new List<Shipper>
            {
                new Shipper { CompanyName = "Speedy Express", Phone = "(503) 555-9831" },
                new Shipper { CompanyName = "United Package", Phone = "(503) 555-3199" },
                new Shipper { CompanyName = "Federal Shipping", Phone = "(503) 555-9931" }
            };

            await context.Shippers.AddRangeAsync(shippers);
            await context.SaveChangesAsync();

            // Add sample orders
            var orders = new List<Order>
            {
                new Order { CustomerID = "ALFKI", EmployeeID = 1, OrderDate = new DateTime(1997, 8, 25), RequiredDate = new DateTime(1997, 9, 22), ShipVia = 1, Freight = 29.46m, ShipName = "Alfreds Futterkiste", ShipAddress = "Obere Str. 57", ShipCity = "Berlin", ShipCountry = "Germany" },
                new Order { CustomerID = "ANATR", EmployeeID = 2, OrderDate = new DateTime(1997, 8, 26), RequiredDate = new DateTime(1997, 9, 23), ShipVia = 2, Freight = 61.02m, ShipName = "Ana Trujillo Emparedados y helados", ShipAddress = "Avda. de la Constitución 2222", ShipCity = "México D.F.", ShipCountry = "Mexico" }
            };

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();

            // Add sample order details
            var orderDetails = new List<Order_Details>
            {
                new Order_Details { OrderID = 1, ProductID = 1, UnitPrice = 18.00m, Quantity = 5, Discount = 0.0f },
                new Order_Details { OrderID = 1, ProductID = 2, UnitPrice = 19.00m, Quantity = 3, Discount = 0.1f },
                new Order_Details { OrderID = 2, ProductID = 3, UnitPrice = 10.00m, Quantity = 2, Discount = 0.0f }
            };

            await context.Order_Details.AddRangeAsync(orderDetails);
            await context.SaveChangesAsync();
        }
    }
} 