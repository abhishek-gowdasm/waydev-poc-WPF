using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for Order Management System
    /// </summary>
    public class OrderManagementDbContext : DbContext
    {
        public OrderManagementDbContext(DbContextOptions<OrderManagementDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Order_Details> Order_Details { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints
            ConfigureCustomerDemographic(modelBuilder);
            ConfigureCustomer(modelBuilder);
            ConfigureEmployee(modelBuilder);
            ConfigureOrder(modelBuilder);
            ConfigureProduct(modelBuilder);
            ConfigureCategory(modelBuilder);
            ConfigureSupplier(modelBuilder);
            ConfigureRegion(modelBuilder);
            ConfigureShipper(modelBuilder);
            ConfigureTerritory(modelBuilder);
            ConfigureOrderDetails(modelBuilder);
        }

        private void ConfigureCustomerDemographic(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerDemographic>(entity =>
            {
                entity.HasKey(e => e.CustomerTypeID);
                entity.Property(e => e.CustomerTypeID).HasMaxLength(10);
                entity.Property(e => e.CustomerDesc);
            });
        }

        private void ConfigureCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerID);
                entity.Property(e => e.CustomerID).HasMaxLength(5);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.ContactName).HasMaxLength(30);
                entity.Property(e => e.ContactTitle).HasMaxLength(30);
                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.Region).HasMaxLength(15);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Country).HasMaxLength(15);
                entity.Property(e => e.Phone).HasMaxLength(24);
                entity.Property(e => e.Fax).HasMaxLength(24);

                // Many-to-many relationship with CustomerDemographic
                entity.HasMany(e => e.CustomerDemographics)
                      .WithMany(e => e.Customers)
                      .UsingEntity(j => j.ToTable("CustomerCustomerDemo"));
            });
        }

        private void ConfigureEmployee(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.EmployeeID).ValueGeneratedOnAdd();
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Title).HasMaxLength(30);
                entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);
                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.Region).HasMaxLength(15);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Country).HasMaxLength(15);
                entity.Property(e => e.HomePhone).HasMaxLength(24);
                entity.Property(e => e.Extension).HasMaxLength(4);
                entity.Property(e => e.Notes);
                entity.Property(e => e.PhotoPath).HasMaxLength(255);

                // Self-referencing relationship for manager-subordinate
                entity.HasOne(e => e.Employees2)
                      .WithMany(e => e.Employees1)
                      .HasForeignKey(e => e.ReportsTo)
                      .OnDelete(DeleteBehavior.Restrict);

                // Many-to-many relationship with Territory
                entity.HasMany(e => e.Territories)
                      .WithMany(e => e.Employees)
                      .UsingEntity(j => j.ToTable("EmployeeTerritories"));
            });
        }

        private void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.OrderID).ValueGeneratedOnAdd();
                entity.Property(e => e.CustomerID).HasMaxLength(5);
                entity.Property(e => e.Freight).HasPrecision(19, 4);
                entity.Property(e => e.ShipName).HasMaxLength(40);
                entity.Property(e => e.ShipAddress).HasMaxLength(60);
                entity.Property(e => e.ShipCity).HasMaxLength(15);
                entity.Property(e => e.ShipRegion).HasMaxLength(15);
                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);
                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                // Relationships
                entity.HasOne(e => e.Customers)
                      .WithMany(e => e.Orders)
                      .HasForeignKey(e => e.CustomerID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Employees)
                      .WithMany(e => e.Orders)
                      .HasForeignKey(e => e.EmployeeID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Shippers)
                      .WithMany()
                      .HasForeignKey(e => e.ShipVia)
                      .OnDelete(DeleteBehavior.Restrict);

                // Order_Details relationship
                entity.HasMany(e => e.Order_Details)
                      .WithOne(e => e.Order)
                      .HasForeignKey(e => e.OrderID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.Property(e => e.ProductID).ValueGeneratedOnAdd();
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
                entity.Property(e => e.UnitPrice).HasPrecision(19, 4);
                entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
                entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
                entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
                entity.Property(e => e.Discontinued).HasDefaultValue(false);

                // Relationships
                entity.HasOne(e => e.Categories)
                      .WithMany(e => e.Products)
                      .HasForeignKey(e => e.CategoryID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Suppliers)
                      .WithMany(e => e.Products)
                      .HasForeignKey(e => e.SupplierID)
                      .OnDelete(DeleteBehavior.Restrict);

                // Order_Details relationship
                entity.HasMany(e => e.Order_Details)
                      .WithOne(e => e.Product)
                      .HasForeignKey(e => e.ProductID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryID).ValueGeneratedOnAdd();
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(15);
                entity.Property(e => e.Description).HasMaxLength(255);
            });
        }

        private void ConfigureSupplier(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierID);
                entity.Property(e => e.SupplierID).ValueGeneratedOnAdd();
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.ContactName).HasMaxLength(30);
                entity.Property(e => e.ContactTitle).HasMaxLength(30);
                entity.Property(e => e.Address).HasMaxLength(60);
                entity.Property(e => e.City).HasMaxLength(15);
                entity.Property(e => e.Region).HasMaxLength(15);
                entity.Property(e => e.PostalCode).HasMaxLength(10);
                entity.Property(e => e.Country).HasMaxLength(15);
                entity.Property(e => e.Phone).HasMaxLength(24);
                entity.Property(e => e.Fax).HasMaxLength(24);
                entity.Property(e => e.HomePage).HasMaxLength(255);
            });
        }

        private void ConfigureRegion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.RegionID);
                entity.Property(e => e.RegionID).ValueGeneratedOnAdd();
                entity.Property(e => e.RegionDescription).IsRequired().HasMaxLength(50);
            });
        }

        private void ConfigureShipper(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.HasKey(e => e.ShipperID);
                entity.Property(e => e.ShipperID).ValueGeneratedOnAdd();
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(40);
                entity.Property(e => e.Phone).HasMaxLength(24);
            });
        }

        private void ConfigureTerritory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Territory>(entity =>
            {
                entity.HasKey(e => e.TerritoryID);
                entity.Property(e => e.TerritoryID).HasMaxLength(20);
                entity.Property(e => e.TerritoryDescription).IsRequired().HasMaxLength(50);

                // Relationship with Region
                entity.HasOne(e => e.Region)
                      .WithMany(e => e.Territories)
                      .HasForeignKey(e => e.RegionID)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureOrderDetails(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order_Details>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.ProductID });
                entity.Property(e => e.UnitPrice).HasPrecision(19, 4);
            });
        }
    }
} 