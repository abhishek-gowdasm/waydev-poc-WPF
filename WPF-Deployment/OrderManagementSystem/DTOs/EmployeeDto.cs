namespace OrderManagementSystem.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleOfCourtesy { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string HomePhone { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int? ReportsTo { get; set; }
        public string PhotoPath { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal TotalSales { get; set; }
    }

    public class CreateEmployeeDto
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleOfCourtesy { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string HomePhone { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int? ReportsTo { get; set; }
        public string PhotoPath { get; set; } = string.Empty;
    }

    public class UpdateEmployeeDto
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }
        public string? Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }
    }

    public class EmployeeStatisticsDto
    {
        public int TotalEmployees { get; set; }
        public int Managers { get; set; }
        public int SalesRepresentatives { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageSalesPerEmployee { get; set; }
    }

    public class EmployeeHierarchyDto
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<EmployeeHierarchyDto> Subordinates { get; set; } = new();
    }
} 