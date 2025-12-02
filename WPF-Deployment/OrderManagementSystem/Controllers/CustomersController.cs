using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Controllers
{
    /// <summary>
    /// API controller responsible for handling HTTP requests related to customers.
    /// </summary>
    /// <remarks>
    /// This controller exposes endpoints to perform CRUD operations and a few query
    /// actions (search, filter by country, top customers, and statistics) for the
    /// Customer resource. It delegates the actual business logic to an
    /// <see cref="ICustomerService"/>, keeping the controller thin and focused
    /// on HTTP concerns (model binding, response formatting and status codes).
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        // Service that contains business logic for customers. Injected via DI.
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Creates a new instance of <see cref="CustomersController"/>.
        /// </summary>
        /// <param name="customerService">Service implementing customer business logic.</param>
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieve all customers.
        /// </summary>
        /// <returns>HTTP 200 with a list of <see cref="CustomerDto"/> on success. HTTP 500 on unexpected errors.</returns>
        /// <remarks>
        /// This endpoint returns all customers. The controller logs basic tracing
        /// information to the console and wraps the call to the service in a try/catch
        /// to convert exceptions into a 500 response with a minimal error payload.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            try
            {
                // Log entry to help during debugging and local development.
                Console.WriteLine("GetCustomers endpoint called");

                // Delegate retrieval to the service. The service is responsible for
                // data access and any mapping to DTOs.
                var customers = await _customerService.GetAllCustomersAsync();

                // Log how many customers were returned (defensive against null).
                Console.WriteLine($"Successfully retrieved {customers?.Count() ?? 0} customers");

                // Return 200 OK with the list of customers.
                return Ok(customers);
            }
            catch (Exception ex)
            {
                // Log exception details to the console. In production consider using
                // a structured logging framework (Serilog, NLog, etc.) rather than Console.
                Console.WriteLine($"Error in GetCustomers: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                // Return a 500 Internal Server Error with a small error object to
                // avoid leaking sensitive exception details in the response body.
                return StatusCode(500, new { error = "An error occurred while retrieving customers", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a single customer by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the customer to retrieve.</param>
        /// <returns>HTTP 200 with the customer on success, 404 when not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(string id)
        {
            // Ask the service for the customer. If not found, return 404 Not Found.
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            // Return 200 OK with the customer DTO.
            return Ok(customer);
        }

        /// <summary>
        /// Search customers by a free text search term.
        /// </summary>
        /// <param name="searchTerm">Search term to apply. If empty or null, service decides behavior.</param>
        /// <returns>HTTP 200 with customers that match the search term.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomers([FromQuery] string searchTerm)
        {
            // The service should handle null/empty terms gracefully (e.g. return all or none).
            var customers = await _customerService.SearchCustomersAsync(searchTerm);
            return Ok(customers);
        }

        /// <summary>
        /// Get customers filtered by their country.
        /// </summary>
        /// <param name="country">Name of the country to filter by.</param>
        /// <returns>HTTP 200 with customers from the specified country.</returns>
        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByCountry(string country)
        {
            // Simple pass-through to the service which implements filtering.
            var customers = await _customerService.GetCustomersByCountryAsync(country);
            return Ok(customers);
        }

        /// <summary>
        /// Get top N customers by total spent.
        /// </summary>
        /// <param name="count">Number of top customers to return. Should be a positive integer.</param>
        /// <returns>HTTP 200 with a list of top customers ordered by total spent.</returns>
        [HttpGet("top/{count}")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetTopCustomers(int count)
        {
            // The service is responsible for validating 'count' and enforcing limits if necessary.
            var customers = await _customerService.GetTopCustomersAsync(count);
            return Ok(customers);
        }

        /// <summary>
        /// Create a new customer resource.
        /// </summary>
        /// <param name="createCustomerDto">DTO containing the values for the new customer.</param>
        /// <returns>
        /// HTTP 201 Created with the created customer DTO and Location header on success;
        /// HTTP 400 Bad Request for validation or domain errors.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            try
            {
                // Basic logging of incoming payload properties for debugging.
                // Be careful not to log sensitive data in production.
                Console.WriteLine($"CreateCustomer endpoint called with CustomerID: {createCustomerDto.CustomerID}");
                Console.WriteLine($"CompanyName: {createCustomerDto.CompanyName}");
                Console.WriteLine($"ContactName: {createCustomerDto.ContactName}");

                // Delegate creation to service which will validate and persist the entity.
                var customer = await _customerService.CreateCustomerAsync(createCustomerDto);

                // Log success and return 201 Created with a Location header pointing to GET /api/customers/{id}
                Console.WriteLine($"Customer created successfully: {customer.CustomerID}");
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customer);
            }
            catch (InvalidOperationException ex)
            {
                // InvalidOperationException typically used by service to indicate a business rule violation.
                Console.WriteLine($"InvalidOperationException in CreateCustomer: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Fallback for other unexpected exceptions. Return BadRequest to avoid exposing server details.
                Console.WriteLine($"Exception in CreateCustomer: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { error = "An error occurred while creating the customer. Please try again." });
            }
        }

        /// <summary>
        /// Update an existing customer's details.
        /// </summary>
        /// <param name="id">Identifier of the customer to update.</param>
        /// <param name="updateCustomerDto">DTO with updated values. Only supplied fields should be changed by the service.</param>
        /// <returns>HTTP 200 with the updated customer or 404 when the customer does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(string id, UpdateCustomerDto updateCustomerDto)
        {
            try
            {
                // The service throws ArgumentException when the entity is not found.
                var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                // Return 404 Not Found with the service-provided message.
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete a customer by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the customer to delete.</param>
        /// <returns>HTTP 204 No Content on success or 404 Not Found if the customer does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            // Service returns a boolean indicating whether the deletion occurred.
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result)
                return NotFound();

            // 204 No Content indicates successful delete with no response body.
            return NoContent();
        }

        /// <summary>
        /// Retrieve aggregated customer statistics used for dashboards and reporting.
        /// </summary>
        /// <returns>HTTP 200 with <see cref="CustomerStatisticsDto"/> containing various aggregated metrics.</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<CustomerStatisticsDto>> GetCustomerStatistics()
        {
            // Delegate the computation of statistics to the service so that the controller
            // remains focused on HTTP concerns and not aggregation logic.
            var statistics = await _customerService.GetCustomerStatisticsAsync();
            return Ok(statistics);
        }
    }
}