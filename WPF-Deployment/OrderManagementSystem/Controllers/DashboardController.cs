using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    /// <summary>
    /// Handles API requests related to dashboard operations such as summary data,
    /// analytics, charts, or quick statistics.
    /// </summary>
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Test endpoint to check if controller is working
        /// </summary>
        /// <returns>Simple test response</returns>
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return Ok("Dashboard controller is working!");
        }

        /// <summary>
        /// Get dashboard data
        /// </summary>
        /// <returns>Complete dashboard data</returns>
        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboardData()
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboardDataAsync();
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get monthly revenue for a specific year
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>Monthly revenue data</returns>
        [HttpGet("monthly-revenue/{year}")]
        public async Task<ActionResult<IEnumerable<MonthlyRevenueDto>>> GetMonthlyRevenue(int year)
        {
            var monthlyRevenue = await _dashboardService.GetMonthlyRevenueAsync(year);
            return Ok(monthlyRevenue);
        }

        /// <summary>
        /// Get top products
        /// </summary>
        /// <param name="count">Number of products to return</param>
        /// <returns>Top products by sales</returns>
        [HttpGet("top-products/{count}")]
        public async Task<ActionResult<IEnumerable<TopProductDto>>> GetTopProducts(int count)
        {
            var topProducts = await _dashboardService.GetTopProductsAsync(count);
            return Ok(topProducts);
        }

        /// <summary>
        /// Get top customers
        /// </summary>
        /// <param name="count">Number of customers to return</param>
        /// <returns>Top customers by total spent</returns>
        [HttpGet("top-customers/{count}")]
        public async Task<ActionResult<IEnumerable<TopCustomerDto>>> GetTopCustomers(int count)
        {
            var topCustomers = await _dashboardService.GetTopCustomersAsync(count);
            return Ok(topCustomers);
        }

        /// <summary>
        /// Get recent orders
        /// </summary>
        /// <param name="count">Number of orders to return</param>
        /// <returns>Recent orders</returns>
        [HttpGet("recent-orders/{count}")]
        public async Task<ActionResult<IEnumerable<RecentOrderDto>>> GetRecentOrders(int count)
        {
            var recentOrders = await _dashboardService.GetRecentOrdersAsync(count);
            return Ok(recentOrders);
        }

        /// <summary>
        /// Get orders by countries (top N)
        /// </summary>
        /// <param name="count">Number of countries to return</param>
        /// <returns>Orders distribution by countries</returns>
        [HttpGet("country-orders/{count}")]
        public async Task<ActionResult<IEnumerable<CountryOrdersDto>>> GetCountryOrders(int count)
        {
            var countryOrders = await _dashboardService.GetCountryOrdersAsync(count);
            return Ok(countryOrders);
        }

        /// <summary>
        /// Get sales by categories
        /// </summary>
        /// <returns>Sales distribution by product categories</returns>
        [HttpGet("category-sales")]
        public async Task<ActionResult<IEnumerable<CategorySalesDto>>> GetCategorySales()
        {
            var categorySales = await _dashboardService.GetCategorySalesAsync();
            return Ok(categorySales);
        }

        /// <summary>
        /// Get sales by countries
        /// </summary>
        /// <returns>Sales distribution by countries</returns>
        [HttpGet("country-sales")]
        public async Task<ActionResult<IEnumerable<CountrySalesDto>>> GetCountrySales()
        {
            var countrySales = await _dashboardService.GetCountrySalesAsync();
            return Ok(countrySales);
        }

        /// <summary>
        /// Get purchases by customers (top N)
        /// </summary>
        /// <param name="count">Number of customers to return</param>
        /// <returns>Top customers by purchases</returns>
        [HttpGet("customer-purchases/{count}")]
        public async Task<ActionResult<IEnumerable<CustomerPurchasesDto>>> GetCustomerPurchases(int count)
        {
            var customerPurchases = await _dashboardService.GetCustomerPurchasesAsync(count);
            return Ok(customerPurchases);
        }

        /// <summary>
        /// Get sales by employees
        /// </summary>
        /// <returns>Sales distribution by employees</returns>
        [HttpGet("employee-sales")]
        public async Task<ActionResult<IEnumerable<EmployeeSalesDto>>> GetEmployeeSales()
        {
            var employeeSales = await _dashboardService.GetEmployeeSalesAsync();
            return Ok(employeeSales);
        }

        /// <summary>
        /// Get customers by country
        /// </summary>
        /// <returns>Customer distribution by countries</returns>
        [HttpGet("customer-countries")]
        public async Task<ActionResult<IEnumerable<CustomerCountryDto>>> GetCustomerCountries()
        {
            var customerCountries = await _dashboardService.GetCustomerCountriesAsync();
            return Ok(customerCountries);
        }

        /// <summary>
        /// Get products by categories
        /// </summary>
        /// <returns>Product distribution by categories</returns>
        [HttpGet("product-categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            var productCategories = await _dashboardService.GetProductCategoriesAsync();
            return Ok(productCategories);
        }

        //Dashboard controller changes
        //Dummy commits
    }
} 