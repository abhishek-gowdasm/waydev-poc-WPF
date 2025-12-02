namespace OrderManagementSystem.DTOs
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; } = string.Empty;
        public decimal? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public int? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }

    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; } = string.Empty;
        public decimal? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public int? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }

    public class UpdateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; } = string.Empty;
        public decimal? UnitPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public int? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }

    public class ProductStatisticsDto
    {
        public int TotalProducts { get; set; }
        public int DiscontinuedProducts { get; set; }
        public int LowStockProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public decimal AverageUnitPrice { get; set; }
    }
    //product dto are changed for waydev
} 