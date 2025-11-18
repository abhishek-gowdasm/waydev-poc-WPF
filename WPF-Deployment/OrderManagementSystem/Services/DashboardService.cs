using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            try
            {
                var totalCustomers = await _unitOfWork.Repository<Customer>().CountAsync();
                var totalOrders = await _unitOfWork.Repository<Order>().CountAsync();
                var totalProducts = await _unitOfWork.Repository<Product>().CountAsync();
                var totalEmployees = await _unitOfWork.Repository<Employee>().CountAsync();
                var pendingOrders = await _unitOfWork.Repository<Order>().CountAsync(o => o.ShippedDate == null);
                var lowStockProducts = await _unitOfWork.Repository<Product>().CountAsync(p => p.UnitsInStock <= 10 && !p.Discontinued);
                
                // Simplified revenue calculation
                var totalRevenue = 0m;
                var averageOrderValue = 0m;

                if (totalOrders > 0)
                {
                    // Get all orders with their details
                    var orderDetails = await _unitOfWork.Repository<Order_Details>()
                        .GetAll()
                        .ToListAsync();

                    totalRevenue = orderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount));
                    averageOrderValue = totalRevenue / totalOrders;
                }

                // Simplified data for now
                var monthlyRevenue = new List<MonthlyRevenueDto>();
                var topProducts = new List<TopProductDto>();
                var topCustomers = new List<TopCustomerDto>();
                var recentOrders = new List<RecentOrderDto>();

                return new DashboardDto
                {
                    TotalCustomers = totalCustomers,
                    TotalOrders = totalOrders,
                    TotalProducts = totalProducts,
                    TotalEmployees = totalEmployees,
                    TotalRevenue = totalRevenue,
                    PendingOrders = pendingOrders,
                    LowStockProducts = lowStockProducts,
                    AverageOrderValue = averageOrderValue,
                    MonthlyRevenue = monthlyRevenue,
                    TopProducts = topProducts,
                    TopCustomers = topCustomers,
                    RecentOrders = recentOrders
                };
            }
            catch (Exception ex)
            {
                // Log the exception and return a basic response
                Console.WriteLine($"Dashboard service error: {ex.Message}");
                return new DashboardDto
                {
                    TotalCustomers = 0,
                    TotalOrders = 0,
                    TotalProducts = 0,
                    TotalEmployees = 0,
                    TotalRevenue = 0,
                    PendingOrders = 0,
                    LowStockProducts = 0,
                    AverageOrderValue = 0,
                    MonthlyRevenue = new List<MonthlyRevenueDto>(),
                    TopProducts = new List<TopProductDto>(),
                    TopCustomers = new List<TopCustomerDto>(),
                    RecentOrders = new List<RecentOrderDto>()
                };
            }
        }

        public async Task<IEnumerable<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(
                    o => o.OrderDate.HasValue && o.OrderDate.Value.Year == year,
                    o => o.Order_Details
                )
                .ToListAsync();

            // Load order details separately to ensure they are loaded
            var orderDetails = await _unitOfWork.Repository<Order_Details>()
                .GetAllWithIncludes(od => od.Order)
                .ToListAsync();

            var monthlyData = orders
                .GroupBy(o => o.OrderDate!.Value.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Revenue = orderDetails
                        .Where(od => od.Order.OrderDate.HasValue && od.Order.OrderDate.Value.Month == g.Key)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                    OrderCount = g.Count()
                })
                .OrderBy(x => DateTime.ParseExact(x.Month, "MMMM", System.Globalization.CultureInfo.CurrentCulture))
                .ToList();

            return monthlyData;
        }

        public async Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int count)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllWithIncludes(p => p.Order_Details)
                .ToListAsync();

            var productStats = products
                .Select(p => new TopProductDto
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    TotalSales = p.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                    OrderCount = p.Order_Details.Count
                })
                .OrderByDescending(p => p.TotalSales)
                .Take(count)
                .ToList();

            return productStats;
        }

        public async Task<IEnumerable<TopCustomerDto>> GetTopCustomersAsync(int count)
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetAllWithIncludes(c => c.Orders)
                .ToListAsync();

            // Load order details separately to ensure they are loaded
            var orderDetails = await _unitOfWork.Repository<Order_Details>()
                .GetAllWithIncludes(od => od.Order, od => od.Order.Customers)
                .ToListAsync();

            var customerStats = customers
                .Select(c => new TopCustomerDto
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName,
                    TotalSpent = orderDetails
                        .Where(od => od.Order.CustomerID == c.CustomerID)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                    OrderCount = c.Orders.Count
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(count)
                .ToList();

            return customerStats;
        }

        public async Task<IEnumerable<RecentOrderDto>> GetRecentOrdersAsync(int count)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetByWithIncludes(
                    o => o.OrderDate.HasValue,
                    o => o.Customers, o => o.Order_Details
                )
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();

            var recentOrders = orders.Select(o => new RecentOrderDto
            {
                OrderID = o.OrderID,
                CustomerName = o.Customers?.CompanyName ?? string.Empty,
                OrderDate = o.OrderDate ?? DateTime.MinValue,
                TotalAmount = o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                Status = o.ShippedDate.HasValue ? "Shipped" : "Pending"
            }).ToList();

            return recentOrders;
        }

        public async Task<IEnumerable<CountryOrdersDto>> GetCountryOrdersAsync(int count)
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetAllWithIncludes(c => c.Orders)
                .ToListAsync();

            var countryStats = customers
                .Where(c => !string.IsNullOrEmpty(c.Country))
                .GroupBy(c => c.Country)
                .Select(g => new CountryOrdersDto
                {
                    Country = g.Key,
                    OrderCount = g.Sum(c => c.Orders.Count)
                })
                .OrderByDescending(c => c.OrderCount)
                .Take(count)
                .ToList();

            return countryStats;
        }

        public async Task<IEnumerable<CategorySalesDto>> GetCategorySalesAsync()
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllWithIncludes(p => p.Order_Details, p => p.Categories)
                .ToListAsync();

            var categoryStats = products
                .Where(p => p.Categories != null)
                .GroupBy(p => p.Categories.CategoryName)
                .Select(g => new CategorySalesDto
                {
                    CategoryName = g.Key,
                    TotalSales = g.Sum(p => p.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount))),
                    ProductCount = g.Count()
                })
                .OrderByDescending(c => c.TotalSales)
                .ToList();

            return categoryStats;
        }

        public async Task<IEnumerable<CountrySalesDto>> GetCountrySalesAsync()
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetAllWithIncludes(c => c.Orders)
                .ToListAsync();

            // Load order details separately to ensure they are loaded
            var orderDetails = await _unitOfWork.Repository<Order_Details>()
                .GetAllWithIncludes(od => od.Order, od => od.Order.Customers)
                .ToListAsync();

            var countrySales = customers
                .Where(c => !string.IsNullOrEmpty(c.Country))
                .GroupBy(c => c.Country)
                .Select(g => new CountrySalesDto
                {
                    Country = g.Key,
                    TotalSales = orderDetails
                        .Where(od => od.Order.Customers.Country == g.Key)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                    OrderCount = g.Sum(c => c.Orders.Count)
                })
                .OrderByDescending(c => c.TotalSales)
                .ToList();

            return countrySales;
        }

        public async Task<IEnumerable<CustomerPurchasesDto>> GetCustomerPurchasesAsync(int count)
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetAllWithIncludes(c => c.Orders)
                .ToListAsync();

            // Load order details separately to ensure they are loaded
            var orderDetails = await _unitOfWork.Repository<Order_Details>()
                .GetAllWithIncludes(od => od.Order, od => od.Order.Customers)
                .ToListAsync();

            var customerPurchases = customers
                .Select(c => new CustomerPurchasesDto
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName,
                    TotalPurchases = orderDetails
                        .Where(od => od.Order.CustomerID == c.CustomerID)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                    OrderCount = c.Orders.Count
                })
                .OrderByDescending(c => c.TotalPurchases)
                .Take(count)
                .ToList();

            return customerPurchases;
        }

        public async Task<IEnumerable<EmployeeSalesDto>> GetEmployeeSalesAsync()
        {
            var employees = await _unitOfWork.Repository<Employee>()
                .GetAllWithIncludes(e => e.Orders)
                .ToListAsync();

            // Load order details separately to ensure they are loaded
            var orderDetails = await _unitOfWork.Repository<Order_Details>()
                .GetAllWithIncludes(od => od.Order)
                .ToListAsync();

            var employeeSales = employees
                .Select(e => new EmployeeSalesDto
                {
                    EmployeeID = e.EmployeeID,
                    EmployeeName = $"{e.FirstName} {e.LastName}",
                    TotalSales = orderDetails
                        .Where(od => od.Order.EmployeeID == e.EmployeeID)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)(double)od.Discount)),
                    OrderCount = e.Orders.Count
                })
                .OrderByDescending(e => e.TotalSales)
                .ToList();

            return employeeSales;
        }

        public async Task<IEnumerable<CustomerCountryDto>> GetCustomerCountriesAsync()
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetAll()
                .ToListAsync();

            var customerCountries = customers
                .Where(c => !string.IsNullOrEmpty(c.Country))
                .GroupBy(c => c.Country)
                .Select(g => new CustomerCountryDto
                {
                    Country = g.Key,
                    CustomerCount = g.Count()
                })
                .OrderByDescending(c => c.CustomerCount)
                .ToList();

            return customerCountries;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategoriesAsync()
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllWithIncludes(p => p.Categories)
                .ToListAsync();

            var productCategories = products
                .Where(p => p.Categories != null)
                .GroupBy(p => p.Categories.CategoryName)
                .Select(g => new ProductCategoryDto
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count()
                })
                .OrderByDescending(c => c.ProductCount)
                .ToList();

            return productCategories;
        }
    }
} 