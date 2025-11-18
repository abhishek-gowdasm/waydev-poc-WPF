namespace OrderManagementSystem.DTOs
{
    public class ShipperDto
    {
        public int ShipperID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int OrderCount { get; set; }
    }

    public class CreateShipperDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class UpdateShipperDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
} 