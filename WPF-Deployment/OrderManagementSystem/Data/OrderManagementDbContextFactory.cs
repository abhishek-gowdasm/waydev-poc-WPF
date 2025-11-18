using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderManagementSystem.Data
{
    public class OrderManagementDbContextFactory : IDesignTimeDbContextFactory<OrderManagementDbContext>
    {
        public OrderManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderManagementDbContext>();
            optionsBuilder.UseSqlServer("data source=.;Initial Catalog=NORTHWIND;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework");

            return new OrderManagementDbContext(optionsBuilder.Options);
        }
    }
} 