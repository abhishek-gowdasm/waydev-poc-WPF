using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            Customers = new HashSet<Customer>();
        }

        [Key]
        [StringLength(10)]
        public string CustomerTypeID { get; set; }

        public string CustomerDesc { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
} 