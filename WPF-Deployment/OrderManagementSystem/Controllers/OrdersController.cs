using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Data;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of all orders</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error in GetOrders: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message, details = ex.StackTrace });
            }
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Get orders by customer
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>Orders for specified customer</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(string customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }

        /// <summary>
        /// Get orders by employee
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Orders handled by specified employee</returns>
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByEmployee(int employeeId)
        {
            var orders = await _orderService.GetOrdersByEmployeeAsync(employeeId);
            return Ok(orders);
        }

        /// <summary>
        /// Get orders by date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Orders within date range</returns>
        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Matching orders</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> SearchOrders([FromQuery] string searchTerm)
        {
            var orders = await _orderService.SearchOrdersAsync(searchTerm);
            return Ok(orders);
        }

        /// <summary>
        /// Get pending orders
        /// </summary>
        /// <returns>Orders that haven't been shipped</returns>
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPendingOrders()
        {
            var orders = await _orderService.GetPendingOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="createOrderDto">Order data</param>
        /// <returns>Created order</returns>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);
                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { error = "An error occurred while creating the order. Please try again." });
            }
        }

        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="updateOrderDto">Updated order data</param>
        /// <returns>Updated order</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, UpdateOrderDto updateOrderDto)
        {
            try
            {
                var order = await _orderService.UpdateOrderAsync(id, updateOrderDto);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get order statistics
        /// </summary>
        /// <returns>Order statistics</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<OrderStatisticsDto>> GetOrderStatistics()
        {
            var statistics = await _orderService.GetOrderStatisticsAsync();
            return Ok(statistics);
        }

        /// <summary>
        /// Seed completed order distributions (for testing purposes)
        /// </summary>
        /// <returns>Seeding result</returns>
        [HttpPost("seed-completed")]
        public async Task<ActionResult> SeedCompletedOrders()
        {
            try
            {
                await CompletedOrdersSeeder.SeedCompletedOrdersAsync(HttpContext.RequestServices);
                return Ok(new { message = "Completed order distributions seeded successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 