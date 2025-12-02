using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly OrderManagementDbContext _context;

        public HealthController(OrderManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Health status</returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetHealth()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var connectionString = _context.Database.GetConnectionString();
                
                return Ok(new
                {
                    status = "healthy",
                    timestamp = DateTime.UtcNow,
                    database = new
                    {
                        connected = canConnect,
                        connectionStringConfigured = !string.IsNullOrEmpty(connectionString)
                    },
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "unhealthy",
                    timestamp = DateTime.UtcNow,
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// Database test endpoint
        /// </summary>
        /// <returns>Database test results</returns>
        [HttpGet("database")]
        public async Task<ActionResult<object>> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    return StatusCode(500, new
                    {
                        status = "database_connection_failed",
                        message = "Cannot connect to database"
                    });
                }

                // Try to get some basic data
                var customerCount = await _context.Customers.CountAsync();
                var orderCount = await _context.Orders.CountAsync();
                var productCount = await _context.Products.CountAsync();

                return Ok(new
                {
                    status = "database_connected",
                    data = new
                    {
                        customers = customerCount,
                        orders = orderCount,
                        products = productCount
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "database_error",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        //for waydev 
        //status = "database_error",
        //            error = ex.Message,
        //            stackTrace = ex.StackTrace
    }
} 