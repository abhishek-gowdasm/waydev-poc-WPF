using OrderManagementSystem.DTOs;

namespace OrderManagementSystem.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(string customerId);
        Task<IEnumerable<OrderDto>> GetOrdersByEmployeeAsync(int employeeId);
        Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderDto>> SearchOrdersAsync(string searchTerm);
        Task<IEnumerable<OrderDto>> GetPendingOrdersAsync();
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto);
        Task<bool> DeleteOrderAsync(int id);
        Task<OrderStatisticsDto> GetOrderStatisticsAsync();
    }
} 