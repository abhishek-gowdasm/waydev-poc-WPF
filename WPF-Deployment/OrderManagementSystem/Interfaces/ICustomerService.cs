using OrderManagementSystem.DTOs;

namespace OrderManagementSystem.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(string id);
        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm);
        Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country);
        Task<IEnumerable<CustomerDto>> GetTopCustomersAsync(int count);
        Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<CustomerDto> UpdateCustomerAsync(string id, UpdateCustomerDto updateCustomerDto);
        Task<bool> DeleteCustomerAsync(string id);
        Task<CustomerStatisticsDto> GetCustomerStatisticsAsync();
    }
} 