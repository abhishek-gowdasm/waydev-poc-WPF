using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Data
{
    public static class CompletedOrdersSeeder
    {
        public static async Task SeedCompletedOrdersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderManagementDbContext>();

            // Check if we already have completed orders
            var existingCompletedOrders = await context.Orders
                .Where(o => o.ShippedDate != null)
                .CountAsync();

            if (existingCompletedOrders > 0)
            {
                Console.WriteLine($"Database already has {existingCompletedOrders} completed orders. Skipping seeding.");
                return;
            }

            Console.WriteLine("Seeding completed order distributions...");

            // Get existing data for relationships
            var customers = await context.Customers.ToListAsync();
            var employees = await context.Employees.ToListAsync();
            var products = await context.Products.ToListAsync();
            var shippers = await context.Shippers.ToListAsync();

            if (!customers.Any() || !employees.Any() || !products.Any() || !shippers.Any())
            {
                Console.WriteLine("Required data (customers, employees, products, shippers) not found. Please run the main seeder first.");
                return;
            }

            // Create completed orders with various distribution scenarios
            var completedOrders = new List<Order>
            {
                // Order 1: Recently completed distribution
                new Order
                {
                    CustomerID = customers[0].CustomerID, // ALFKI
                    EmployeeID = employees[0].EmployeeID, // Nancy Davolio
                    OrderDate = DateTime.Now.AddDays(-30),
                    RequiredDate = DateTime.Now.AddDays(-15),
                    ShippedDate = DateTime.Now.AddDays(-10), // Completed 10 days ago
                    ShipVia = shippers[0].ShipperID, // Speedy Express
                    Freight = 25.50m,
                    ShipName = "Alfreds Futterkiste",
                    ShipAddress = "Obere Str. 57",
                    ShipCity = "Berlin",
                    ShipCountry = "Germany"
                },

                // Order 2: Completed distribution with premium shipping
                new Order
                {
                    CustomerID = customers[1].CustomerID, // ANATR
                    EmployeeID = employees[1].EmployeeID, // Andrew Fuller
                    OrderDate = DateTime.Now.AddDays(-45),
                    RequiredDate = DateTime.Now.AddDays(-30),
                    ShippedDate = DateTime.Now.AddDays(-25), // Completed 25 days ago
                    ShipVia = shippers[1].ShipperID, // United Package
                    Freight = 45.75m,
                    ShipName = "Ana Trujillo Emparedados y helados",
                    ShipAddress = "Avda. de la Constitución 2222",
                    ShipCity = "México D.F.",
                    ShipCountry = "Mexico"
                },

                // Order 3: International distribution completed
                new Order
                {
                    CustomerID = customers[2].CustomerID, // ANTON
                    EmployeeID = employees[2].EmployeeID, // Janet Leverling
                    OrderDate = DateTime.Now.AddDays(-60),
                    RequiredDate = DateTime.Now.AddDays(-45),
                    ShippedDate = DateTime.Now.AddDays(-40), // Completed 40 days ago
                    ShipVia = shippers[2].ShipperID, // Federal Shipping
                    Freight = 67.20m,
                    ShipName = "Antonio Moreno Taquería",
                    ShipAddress = "Mataderos  2312",
                    ShipCity = "México D.F.",
                    ShipCountry = "Mexico"
                },

                // Order 4: Bulk order distribution completed
                new Order
                {
                    CustomerID = customers[0].CustomerID, // ALFKI (repeat customer)
                    EmployeeID = employees[0].EmployeeID, // Nancy Davolio
                    OrderDate = DateTime.Now.AddDays(-20),
                    RequiredDate = DateTime.Now.AddDays(-10),
                    ShippedDate = DateTime.Now.AddDays(-5), // Completed 5 days ago
                    ShipVia = shippers[0].ShipperID, // Speedy Express
                    Freight = 89.30m,
                    ShipName = "Alfreds Futterkiste",
                    ShipAddress = "Obere Str. 57",
                    ShipCity = "Berlin",
                    ShipCountry = "Germany"
                },

                // Order 5: Express distribution completed
                new Order
                {
                    CustomerID = customers[1].CustomerID, // ANATR (repeat customer)
                    EmployeeID = employees[1].EmployeeID, // Andrew Fuller
                    OrderDate = DateTime.Now.AddDays(-15),
                    RequiredDate = DateTime.Now.AddDays(-7),
                    ShippedDate = DateTime.Now.AddDays(-3), // Completed 3 days ago
                    ShipVia = shippers[1].ShipperID, // United Package
                    Freight = 120.00m,
                    ShipName = "Ana Trujillo Emparedados y helados",
                    ShipAddress = "Avda. de la Constitución 2222",
                    ShipCity = "México D.F.",
                    ShipCountry = "Mexico"
                }
            };

            await context.Orders.AddRangeAsync(completedOrders);
            await context.SaveChangesAsync();

            // Create order details for the completed orders
            var orderDetails = new List<Order_Details>
            {
                // Order 1 details
                new Order_Details { OrderID = 3, ProductID = products[0].ProductID, UnitPrice = 18.00m, Quantity = 10, Discount = 0.05f },
                new Order_Details { OrderID = 3, ProductID = products[1].ProductID, UnitPrice = 19.00m, Quantity = 5, Discount = 0.0f },

                // Order 2 details
                new Order_Details { OrderID = 4, ProductID = products[2].ProductID, UnitPrice = 10.00m, Quantity = 15, Discount = 0.1f },
                new Order_Details { OrderID = 4, ProductID = products[0].ProductID, UnitPrice = 18.00m, Quantity = 8, Discount = 0.0f },

                // Order 3 details
                new Order_Details { OrderID = 5, ProductID = products[1].ProductID, UnitPrice = 19.00m, Quantity = 12, Discount = 0.15f },
                new Order_Details { OrderID = 5, ProductID = products[2].ProductID, UnitPrice = 10.00m, Quantity = 20, Discount = 0.05f },

                // Order 4 details
                new Order_Details { OrderID = 6, ProductID = products[0].ProductID, UnitPrice = 18.00m, Quantity = 25, Discount = 0.2f },
                new Order_Details { OrderID = 6, ProductID = products[1].ProductID, UnitPrice = 19.00m, Quantity = 15, Discount = 0.1f },
                new Order_Details { OrderID = 6, ProductID = products[2].ProductID, UnitPrice = 10.00m, Quantity = 30, Discount = 0.0f },

                // Order 5 details
                new Order_Details { OrderID = 7, ProductID = products[0].ProductID, UnitPrice = 18.00m, Quantity = 20, Discount = 0.0f },
                new Order_Details { OrderID = 7, ProductID = products[1].ProductID, UnitPrice = 19.00m, Quantity = 18, Discount = 0.05f }
            };

            await context.Order_Details.AddRangeAsync(orderDetails);
            await context.SaveChangesAsync();

            Console.WriteLine($"Successfully seeded {completedOrders.Count} completed order distributions with {orderDetails.Count} order details.");
            Console.WriteLine("Distribution completion dates range from 3 to 40 days ago.");

            //Console.WriteLine($"Successfully seeded {completedOrders.Count} completed order distributions with {orderDetails.Count} order details.");
            //Console.WriteLine("Distribution completion dates range from 3 to 40 days ago.");
        }
    }
} 