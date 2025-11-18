using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Data;
using OrderManagementSystem.Services;
using OrderManagementSystem.Mapping;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Order Management System API",
        Version = "v1",
        Description = "A comprehensive API for managing orders, customers, products, and employees",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Order Management System",
            Email = "support@ordermanagementsystem.com"
        }
    });
});

// Database Configuration
builder.Services.AddDbContext<OrderManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Database initialization and connection test
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<OrderManagementDbContext>();
        
        Console.WriteLine("=== DATABASE CONNECTION TEST ===");
        Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection string configured: {!string.IsNullOrEmpty(connectionString)}");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("ERROR: Connection string is null or empty!");
            Console.WriteLine("Please check your appsettings.json or environment variables.");
            return;
        }
        
        // Log connection string details (without password for security)
        var connectionParts = connectionString.Split(';');
        foreach (var part in connectionParts)
        {
            if (!part.ToLower().Contains("password") && !part.ToLower().Contains("pwd"))
            {
                Console.WriteLine($"Connection part: {part}");
            }
        }
        
        // Test connection with timeout
        Console.WriteLine("Testing database connection...");
        var canConnect = await context.Database.CanConnectAsync();
        Console.WriteLine($"Can connect to database: {canConnect}");
        
        if (!canConnect)
        {
            Console.WriteLine("ERROR: Cannot connect to database!");
            Console.WriteLine("Possible issues:");
            Console.WriteLine("1. SQL Server is not running or accessible");
            Console.WriteLine("2. Firewall blocking connection");
            Console.WriteLine("3. Incorrect connection string");
            Console.WriteLine("4. Network connectivity issues");
            Console.WriteLine("5. Azure SQL Database server is paused (if using Azure)");
            
            // Don't exit the application, let it continue but log the error
            Console.WriteLine("Application will continue but database operations will fail.");
        }
        else
        {
            Console.WriteLine("SUCCESS: Database connection established!");
            
            // Test if database schema exists
            try
            {
                var hasTables = await context.Database.SqlQueryRaw<int>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES").FirstOrDefaultAsync();
                Console.WriteLine($"Database has {hasTables} tables");
                
                if (hasTables == 0)
                {
                    Console.WriteLine("WARNING: Database appears to be empty. Creating schema...");
                    await context.Database.EnsureCreatedAsync();
                    Console.WriteLine("Database schema created successfully.");
                }
                else
                {
                    // Test basic queries
                    try
                    {
                        var orderCount = await context.Orders.CountAsync();
                        var customerCount = await context.Customers.CountAsync();
                        var productCount = await context.Products.CountAsync();
                        
                        Console.WriteLine($"Database contains: {orderCount} orders, {customerCount} customers, {productCount} products");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WARNING: Error querying data: {ex.Message}");
                        Console.WriteLine("This might indicate a schema mismatch. Consider running migrations.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Database schema test failed: {ex.Message}");
                Console.WriteLine("Attempting to create database schema...");
                
                try
                {
                    await context.Database.EnsureCreatedAsync();
                    Console.WriteLine("Database schema created successfully.");
                }
                catch (Exception createEx)
                {
                    Console.WriteLine($"ERROR: Failed to create database schema: {createEx.Message}");
                }
            }
        }
        
        Console.WriteLine("=== END DATABASE CONNECTION TEST ===");
        
        // Initialize database with sample data
        try
        {
            Console.WriteLine("=== DATABASE INITIALIZATION ===");
            await DatabaseInitializer.InitializeDatabaseAsync(app.Services);
            Console.WriteLine("Database initialization completed successfully.");
            
            // Seed completed order distributions
            await CompletedOrdersSeeder.SeedCompletedOrdersAsync(app.Services);
            Console.WriteLine("=== END DATABASE INITIALIZATION ===");
        }
        catch (Exception initEx)
        {
            Console.WriteLine($"ERROR: Database initialization failed: {initEx.Message}");
            Console.WriteLine($"Stack trace: {initEx.StackTrace}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("=== CRITICAL ERROR ===");
        Console.WriteLine($"Error during database initialization: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        Console.WriteLine("=== END CRITICAL ERROR ===");
        
        // Don't exit the application, let it continue but log the error
        Console.WriteLine("Application will continue but database operations will fail.");
    }
}

// Configure the HTTP request pipeline.
// CORS must be configured FIRST, before any other middleware
app.UseCors("AllowAll");

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Management System API v1");
    c.RoutePrefix = string.Empty; // Set Swagger UI as the default page
    c.DocumentTitle = "Order Management System API Documentation";
});

// Enable HTTPS redirection in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// No redirect - let the API handle the root
app.MapGet("/", () => "Order Management System API is running. Visit /swagger for documentation.");

app.Run();
