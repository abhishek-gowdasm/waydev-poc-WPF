using OrderManagementSystem.DTOs;

namespace OrderManagementSystem.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
        Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year);
        Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int count);
        Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int count);
        Task<IEnumerable<RecentOrderDto>> GetRecentOrdersAsync(int count);
        Task<IEnumerable<CountryOrdersDto>> GetCountryOrdersAsync(int count);
        Task<IEnumerable<CategorySalesDto>> GetCategorySalesAsync();
        Task<IEnumerable<CountrySalesDto>> GetCountrySalesAsync();
        Task<IEnumerable<CustomerPurchasesDto>> GetCustomerPurchasesAsync(int count);
        Task<IEnumerable<EmployeeSalesDto>> GetEmployeeSalesAsync();
        Task<IEnumerable<CustomerCountryDto>> GetCustomerCountriesAsync();
        Task<IEnumerable<ProductCategoryDto>> GetProductCategoriesAsync();
    }
} 