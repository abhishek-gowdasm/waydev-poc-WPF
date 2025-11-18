namespace OrderManagementSystem.DTOs
{
    public class DashboardDto
    {
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalEmployees { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int LowStockProducts { get; set; }
        public decimal AverageOrderValue { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
        public List<TopProductDto> TopProducts { get; set; } = new();
        public List<TopCustomerDto> TopCustomers { get; set; } = new();
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class TopProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }

    public class TopCustomerDto
    {
        public string CustomerID { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
        public int OrderCount { get; set; }
    }

    public class RecentOrderDto
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CountryOrdersDto
    {
        public string Country { get; set; } = string.Empty;
        public int OrderCount { get; set; }
    }

    public class CategorySalesDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int ProductCount { get; set; }
    }

    public class CountrySalesDto
    {
        public string Country { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }

    public class CustomerPurchasesDto
    {
        public string CustomerID { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal TotalPurchases { get; set; }
        public int OrderCount { get; set; }
    }

    public class EmployeeSalesDto
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }

    public class CustomerCountryDto
    {
        public string Country { get; set; } = string.Empty;
        public int CustomerCount { get; set; }
    }

    public class ProductCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }
} 