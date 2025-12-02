using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all orders with related data and computed fields
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetAllWithIncludes(o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
                orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
                orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
                orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                orderDto.ItemCount = order.Order_Details.Count;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(o => o.OrderID == id, o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details)
                .FirstOrDefaultAsync();

            if (order == null)
                return null;

            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
            orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
            orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
            orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
            orderDto.ItemCount = order.Order_Details.Count;

            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(string customerId)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(o => o.CustomerID == customerId, o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
                orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
                orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
                orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                orderDto.ItemCount = order.Order_Details.Count;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByEmployeeAsync(int employeeId)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(o => o.EmployeeID == employeeId, o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
                orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
                orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
                orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                orderDto.ItemCount = order.Order_Details.Count;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(
                    o => o.OrderDate >= startDate && o.OrderDate <= endDate,
                    o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details
                )
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
                orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
                orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
                orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                orderDto.ItemCount = order.Order_Details.Count;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> SearchOrdersAsync(string searchTerm)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(
                    o => o.ShipName.Contains(searchTerm) || 
                         o.ShipCity.Contains(searchTerm) || 
                         o.ShipCountry.Contains(searchTerm),
                    o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details
                )
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
                orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
                orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
                orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                orderDto.ItemCount = order.Order_Details.Count;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetPendingOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(
                    o => o.ShippedDate == null,
                    o => o.Customers, o => o.Employees, o => o.Shippers, o => o.Order_Details
                )
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                orderDto.CustomerName = order.Customers?.CompanyName ?? string.Empty;
                orderDto.EmployeeName = order.Employees != null ? $"{order.Employees.FirstName} {order.Employees.LastName}" : string.Empty;
                orderDto.ShipperName = order.Shippers?.CompanyName ?? string.Empty;
                orderDto.TotalAmount = order.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                orderDto.ItemCount = order.Order_Details.Count;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(createOrderDto.CustomerID))
            {
                throw new InvalidOperationException("Customer ID is required.");
            }

            if (createOrderDto.EmployeeID <= 0)
            {
                throw new InvalidOperationException("Employee ID is required.");
            }

            if (createOrderDto.OrderDate == default)
            {
                createOrderDto.OrderDate = DateTime.Now;
            }

            if (createOrderDto.RequiredDate <= createOrderDto.OrderDate)
            {
                throw new InvalidOperationException("Required date must be after order date.");
            }

            // Validate field lengths
            if (createOrderDto.CustomerID?.Length > 5)
            {
                throw new InvalidOperationException("Customer ID cannot exceed 5 characters.");
            }

            if (createOrderDto.ShipName?.Length > 40)
            {
                throw new InvalidOperationException("Ship name cannot exceed 40 characters.");
            }

            if (createOrderDto.ShipAddress?.Length > 60)
            {
                throw new InvalidOperationException("Ship address cannot exceed 60 characters.");
            }

            if (createOrderDto.ShipCity?.Length > 15)
            {
                throw new InvalidOperationException("Ship city cannot exceed 15 characters.");
            }

            if (createOrderDto.ShipRegion?.Length > 15)
            {
                throw new InvalidOperationException("Ship region cannot exceed 15 characters.");
            }

            if (createOrderDto.ShipPostalCode?.Length > 10)
            {
                throw new InvalidOperationException("Ship postal code cannot exceed 10 characters.");
            }

            if (createOrderDto.ShipCountry?.Length > 15)
            {
                throw new InvalidOperationException("Ship country cannot exceed 15 characters.");
            }

            // Validate that customer exists
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(createOrderDto.CustomerID);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer with ID '{createOrderDto.CustomerID}' not found.");
            }

            // Validate that employee exists
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(createOrderDto.EmployeeID);
            if (employee == null)
            {
                throw new InvalidOperationException($"Employee with ID '{createOrderDto.EmployeeID}' not found.");
            }

            // Validate that shipper exists if ShipVia is provided
            if (createOrderDto.ShipVia.HasValue && createOrderDto.ShipVia.Value > 0)
            {
                var shipper = await _unitOfWork.Repository<Shipper>().GetByIdAsync(createOrderDto.ShipVia.Value);
                if (shipper == null)
                {
                    throw new InvalidOperationException($"Shipper with ID '{createOrderDto.ShipVia.Value}' not found.");
                }
            }

            var order = _mapper.Map<Order>(createOrderDto);
            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException("Order not found");

            _mapper.Map(updateOrderDto, order);
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(id);
            if (order == null)
                return false;

            _unitOfWork.Repository<Order>().Delete(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<OrderStatisticsDto> GetOrderStatisticsAsync()
        {
            var totalOrders = await _unitOfWork.Repository<Order>().CountAsync();
            var pendingOrders = await _unitOfWork.Repository<Order>().CountAsync(o => o.ShippedDate == null);
            var shippedOrders = await _unitOfWork.Repository<Order>().CountAsync(o => o.ShippedDate != null);
            var totalRevenue = await _unitOfWork.Repository<Order>()
                .GetAll()
                .SumAsync(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));

            return new OrderStatisticsDto
            {
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                ShippedOrders = shippedOrders,
                TotalRevenue = totalRevenue,
                AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0
            };
        }
    }
} 