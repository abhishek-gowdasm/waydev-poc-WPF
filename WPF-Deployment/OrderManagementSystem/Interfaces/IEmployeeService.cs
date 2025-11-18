using OrderManagementSystem.DTOs;

namespace OrderManagementSystem.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByTerritoryAsync(string territoryId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByRegionAsync(int regionId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByHireDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync();
        Task<IEnumerable<EmployeeDto>> GetEmployeesByTitleAsync(string title);
        Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm);
        Task<IEnumerable<EmployeeDto>> GetTopEmployeesAsync(int count);
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto);
        Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<EmployeeStatisticsDto> GetEmployeeStatisticsAsync();
        Task<IEnumerable<EmployeeHierarchyDto>> GetEmployeeHierarchyAsync();
    }
} 