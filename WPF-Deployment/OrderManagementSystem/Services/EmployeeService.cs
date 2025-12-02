using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Get all employees with their manager names, order counts, and total sales
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetAllWithIncludes(e => e.Employees2, e => e.Orders, e => e.Territories)
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>()
                .GetByWithIncludes(e => e.EmployeeID == id, e => e.Employees2, e => e.Orders, e => e.Territories)
                .FirstOrDefaultAsync();

            if (employee == null)
                return null;

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
            employeeDto.OrderCount = employee.Orders.Count;
            employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));

            return employeeDto;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByTerritoryAsync(string territoryId)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetByWithIncludes(e => e.Territories.Any(t => t.TerritoryID == territoryId), e => e.Employees2, e => e.Orders, e => e.Territories)
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByRegionAsync(int regionId)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetByWithIncludes(e => e.Territories.Any(t => t.RegionID == regionId), e => e.Employees2, e => e.Orders, e => e.Territories)
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByHireDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetByWithIncludes(
                    e => e.HireDate >= startDate && e.HireDate <= endDate,
                    e => e.Employees2, e => e.Orders, e => e.Territories
                )
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetAllWithIncludes(e => e.Employees2, e => e.Orders, e => e.Territories)
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByTitleAsync(string title)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetByWithIncludes(e => e.Title == title, e => e.Employees2, e => e.Orders, e => e.Territories)
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetByWithIncludes(
                    e => e.FirstName.Contains(searchTerm) || 
                         e.LastName.Contains(searchTerm) || 
                         e.Title.Contains(searchTerm) || 
                         e.City.Contains(searchTerm) || 
                         e.Country.Contains(searchTerm),
                    e => e.Employees2, e => e.Orders, e => e.Territories
                )
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }

        public async Task<IEnumerable<EmployeeDto>> GetTopEmployeesAsync(int count)
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetAllWithIncludes(e => e.Employees2, e => e.Orders, e => e.Territories)
                .ToListAsync();

            var employeeDtos = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                employeeDto.ManagerName = employee.Employees2 != null ? $"{employee.Employees2.FirstName} {employee.Employees2.LastName}" : string.Empty;
                employeeDto.OrderCount = employee.Orders.Count;
                employeeDto.TotalSales = employee.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                employeeDtos.Add(employeeDto);
            }

            return employeeDtos.OrderByDescending(e => e.TotalSales).Take(count);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(createEmployeeDto.FirstName))
            {
                throw new InvalidOperationException("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(createEmployeeDto.LastName))
            {
                throw new InvalidOperationException("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(createEmployeeDto.Title))
            {
                throw new InvalidOperationException("Title is required.");
            }

            // Validate field lengths
            if (createEmployeeDto.FirstName?.Length > 10)
            {
                throw new InvalidOperationException("First name cannot exceed 10 characters.");
            }

            if (createEmployeeDto.LastName?.Length > 20)
            {
                throw new InvalidOperationException("Last name cannot exceed 20 characters.");
            }

            if (createEmployeeDto.Title?.Length > 30)
            {
                throw new InvalidOperationException("Title cannot exceed 30 characters.");
            }

            if (createEmployeeDto.TitleOfCourtesy?.Length > 25)
            {
                throw new InvalidOperationException("Title of courtesy cannot exceed 25 characters.");
            }

            if (createEmployeeDto.Address?.Length > 60)
            {
                throw new InvalidOperationException("Address cannot exceed 60 characters.");
            }

            if (createEmployeeDto.City?.Length > 15)
            {
                throw new InvalidOperationException("City cannot exceed 15 characters.");
            }

            if (createEmployeeDto.Region?.Length > 15)
            {
                throw new InvalidOperationException("Region cannot exceed 15 characters.");
            }

            if (createEmployeeDto.PostalCode?.Length > 10)
            {
                throw new InvalidOperationException("Postal code cannot exceed 10 characters.");
            }

            if (createEmployeeDto.Country?.Length > 15)
            {
                throw new InvalidOperationException("Country cannot exceed 15 characters.");
            }

            if (createEmployeeDto.HomePhone?.Length > 24)
            {
                throw new InvalidOperationException("Home phone cannot exceed 24 characters.");
            }

            if (createEmployeeDto.Extension?.Length > 4)
            {
                throw new InvalidOperationException("Extension cannot exceed 4 characters.");
            }

            if (createEmployeeDto.PhotoPath?.Length > 255)
            {
                throw new InvalidOperationException("Photo path cannot exceed 255 characters.");
            }

            // Check if employee with same name already exists
            var existingEmployee = await _unitOfWork.Repository<Employee>()
                .GetBy(e => e.FirstName.ToLower() == createEmployeeDto.FirstName.ToLower() && 
                           e.LastName.ToLower() == createEmployeeDto.LastName.ToLower())
                .FirstOrDefaultAsync();

            if (existingEmployee != null)
            {
                throw new InvalidOperationException($"Employee with name '{createEmployeeDto.FirstName} {createEmployeeDto.LastName}' already exists.");
            }

            var employee = _mapper.Map<Employee>(createEmployeeDto);
            await _unitOfWork.Repository<Employee>().AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            _mapper.Map(updateEmployeeDto, employee);
            _unitOfWork.Repository<Employee>().Update(employee);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (employee == null)
                return false;

            _unitOfWork.Repository<Employee>().Delete(employee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<EmployeeStatisticsDto> GetEmployeeStatisticsAsync()
        {
            var totalEmployees = await _unitOfWork.Repository<Employee>().CountAsync();
            var managers = await _unitOfWork.Repository<Employee>().CountAsync(e => e.Employees1.Any());
            var salesRepresentatives = await _unitOfWork.Repository<Employee>().CountAsync(e => e.Title == "Sales Representative");
            
            var totalSales = await _unitOfWork.Repository<Order>()
                .GetAll()
                .SumAsync(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));

            var averageSalesPerEmployee = totalSales / totalEmployees;

            return new EmployeeStatisticsDto
            {
                TotalEmployees = totalEmployees,
                Managers = managers,
                SalesRepresentatives = salesRepresentatives,
                TotalSales = totalSales,
                AverageSalesPerEmployee = averageSalesPerEmployee
            };
        }

        public async Task<IEnumerable<EmployeeHierarchyDto>> GetEmployeeHierarchyAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetAllWithIncludes(e => e.Employees1, e => e.Employees2)
                .ToListAsync();

            var hierarchy = new List<EmployeeHierarchyDto>();

            // Get top-level employees (those without managers)
            var topLevelEmployees = employees.Where(e => e.ReportsTo == null).ToList();

            foreach (var employee in topLevelEmployees)
            {
                var hierarchyDto = new EmployeeHierarchyDto
                {
                    EmployeeID = employee.EmployeeID,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}",
                    Title = employee.Title ?? string.Empty,
                    Subordinates = await BuildSubordinatesAsync(employee.EmployeeID, employees)
                };
                hierarchy.Add(hierarchyDto);
            }

            return hierarchy;
        }

        private async Task<List<EmployeeHierarchyDto>> BuildSubordinatesAsync(int managerId, List<Employee> allEmployees)
        {
            var subordinates = allEmployees.Where(e => e.ReportsTo == managerId).ToList();
            var result = new List<EmployeeHierarchyDto>();

            foreach (var subordinate in subordinates)
            {
                var hierarchyDto = new EmployeeHierarchyDto
                {
                    EmployeeID = subordinate.EmployeeID,
                    EmployeeName = $"{subordinate.FirstName} {subordinate.LastName}",
                    Title = subordinate.Title ?? string.Empty,
                    Subordinates = await BuildSubordinatesAsync(subordinate.EmployeeID, allEmployees)
                };
                result.Add(hierarchyDto);
            }

            return result;
        }
    }
} 