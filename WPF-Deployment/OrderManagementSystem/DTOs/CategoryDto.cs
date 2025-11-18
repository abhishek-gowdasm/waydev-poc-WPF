namespace OrderManagementSystem.DTOs
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[] Picture { get; set; } = new byte[0];
        public int ProductCount { get; set; }
    }

    public class CreateCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[] Picture { get; set; } = new byte[0];
    }

    public class UpdateCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[] Picture { get; set; } = new byte[0];
    }
} 