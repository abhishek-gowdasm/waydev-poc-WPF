using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public partial class Shipper
    {
        [Key]
        public int ShipperID { get; set; }

        [Required]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }
    }
} 