using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of all customers</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                Console.WriteLine("GetCustomers endpoint called");
                var customers = await _customerService.GetAllCustomersAsync();
                Console.WriteLine($"Successfully retrieved {customers?.Count() ?? 0} customers");
                return Ok(customers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCustomers: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "An error occurred while retrieving customers", details = ex.Message });
            }
        }

        /// <summary>
        /// Get customer by ID
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Customer details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(string id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        /// <summary>
        /// Search customers
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Matching customers</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomers([FromQuery] string searchTerm)
        {
            var customers = await _customerService.SearchCustomersAsync(searchTerm);
            return Ok(customers);
        }

        /// <summary>
        /// Get customers by country
        /// </summary>
        /// <param name="country">Country name</param>
        /// <returns>Customers in specified country</returns>
        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByCountry(string country)
        {
            var customers = await _customerService.GetCustomersByCountryAsync(country);
            return Ok(customers);
        }

        /// <summary>
        /// Get top customers
        /// </summary>
        /// <param name="count">Number of customers to return</param>
        /// <returns>Top customers by total spent</returns>
        [HttpGet("top/{count}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetTopCustomers(int count)
        {
            var customers = await _customerService.GetTopCustomersAsync(count);
            return Ok(customers);
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="createCustomerDto">Customer data</param>
        /// <returns>Created customer</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            try
            {
                Console.WriteLine($"CreateCustomer endpoint called with CustomerID: {createCustomerDto.CustomerID}");
                Console.WriteLine($"CompanyName: {createCustomerDto.CompanyName}");
                Console.WriteLine($"ContactName: {createCustomerDto.ContactName}");
                
                var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
                Console.WriteLine($"Customer created successfully: {customer.CustomerID}");
                
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customer);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"InvalidOperationException in CreateCustomer: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CreateCustomer: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { error = "An error occurred while creating the customer. Please try again." });
            }
        }

        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <param name="updateCustomerDto">Updated customer data</param>
        /// <returns>Updated customer</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(string id, UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="id">Customer ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get customer statistics
        /// </summary>
        /// <returns>Customer statistics</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<CustomerStatisticsDto>> GetCustomerStatistics()
        {
            var statistics = await _customerService.GetCustomerStatisticsAsync();
            return Ok(statistics);
        }


        //[HttpGet("statistics")]
        //public async Task<ActionResult<CustomerStatisticsDto>> GetCustomerStatistics()
        //{
        //    var statistics = await _customerService.GetCustomerStatisticsAsync();
        //    return Ok(statistics);
        //}
    }
} 