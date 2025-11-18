-- Production Database Deployment Script
-- This script should be run on your production database

PRINT 'Starting database deployment...'

-- Check if database exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'OMS')
BEGIN
    PRINT 'Creating database OMS...'
    CREATE DATABASE OMS;
END
ELSE
BEGIN
    PRINT 'Database OMS already exists.'
END

USE OMS;

-- Create __EFMigrationsHistory table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
    PRINT 'Creating __EFMigrationsHistory table...'
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

-- Check if initial migration is applied
IF NOT EXISTS (SELECT * FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = '20250723130509_InitialCreate')
BEGIN
    PRINT 'Marking initial migration as applied...'
    INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250723130509_InitialCreate', '8.0.0');
END

-- Verify database connectivity
PRINT 'Testing database connectivity...'
SELECT 'Database connection successful' as Status;

-- Check if tables exist
DECLARE @TableCount INT = (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE');
PRINT 'Number of tables in database: ' + CAST(@TableCount AS VARCHAR(10));

-- If no tables exist, create basic structure
IF @TableCount = 0
BEGIN
    PRINT 'No tables found. Creating basic table structure...'
    
    -- Create Categories table
    CREATE TABLE [dbo].[Categories] (
        [CategoryID] int IDENTITY(1,1) NOT NULL,
        [CategoryName] nvarchar(15) NOT NULL,
        [Description] ntext NULL,
        [Picture] varbinary(max) NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryID])
    );
    
    -- Create Customers table
    CREATE TABLE [dbo].[Customers] (
        [CustomerID] nvarchar(5) NOT NULL,
        [CompanyName] nvarchar(40) NOT NULL,
        [ContactName] nvarchar(30) NULL,
        [ContactTitle] nvarchar(30) NULL,
        [Address] nvarchar(60) NULL,
        [City] nvarchar(15) NULL,
        [Region] nvarchar(15) NULL,
        [PostalCode] nvarchar(10) NULL,
        [Country] nvarchar(15) NULL,
        [Phone] nvarchar(24) NULL,
        [Fax] nvarchar(24) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerID])
    );
    
    -- Create Employees table
    CREATE TABLE [dbo].[Employees] (
        [EmployeeID] int IDENTITY(1,1) NOT NULL,
        [LastName] nvarchar(20) NOT NULL,
        [FirstName] nvarchar(10) NOT NULL,
        [Title] nvarchar(30) NULL,
        [TitleOfCourtesy] nvarchar(25) NULL,
        [BirthDate] datetime2 NULL,
        [HireDate] datetime2 NULL,
        [Address] nvarchar(60) NULL,
        [City] nvarchar(15) NULL,
        [Region] nvarchar(15) NULL,
        [PostalCode] nvarchar(10) NULL,
        [Country] nvarchar(15) NULL,
        [HomePhone] nvarchar(24) NULL,
        [Extension] nvarchar(4) NULL,
        [Notes] ntext NULL,
        [PhotoPath] nvarchar(255) NULL,
        [ReportsTo] int NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([EmployeeID])
    );
    
    -- Create Products table
    CREATE TABLE [dbo].[Products] (
        [ProductID] int IDENTITY(1,1) NOT NULL,
        [ProductName] nvarchar(40) NOT NULL,
        [SupplierID] int NULL,
        [CategoryID] int NULL,
        [QuantityPerUnit] nvarchar(20) NULL,
        [UnitPrice] decimal(18,2) NULL,
        [UnitsInStock] smallint NULL,
        [UnitsOnOrder] smallint NULL,
        [ReorderLevel] smallint NULL,
        [Discontinued] bit NOT NULL DEFAULT 0,
        CONSTRAINT [PK_Products] PRIMARY KEY ([ProductID])
    );
    
    -- Create Orders table
    CREATE TABLE [dbo].[Orders] (
        [OrderID] int IDENTITY(1,1) NOT NULL,
        [CustomerID] nvarchar(5) NULL,
        [EmployeeID] int NULL,
        [OrderDate] datetime2 NULL,
        [RequiredDate] datetime2 NULL,
        [ShippedDate] datetime2 NULL,
        [ShipVia] int NULL,
        [Freight] decimal(18,2) NULL,
        [ShipName] nvarchar(40) NULL,
        [ShipAddress] nvarchar(60) NULL,
        [ShipCity] nvarchar(15) NULL,
        [ShipRegion] nvarchar(15) NULL,
        [ShipPostalCode] nvarchar(10) NULL,
        [ShipCountry] nvarchar(15) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderID])
    );
    
    -- Create Order_Details table
    CREATE TABLE [dbo].[Order_Details] (
        [OrderID] int NOT NULL,
        [ProductID] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [Quantity] smallint NOT NULL,
        [Discount] real NOT NULL DEFAULT 0,
        CONSTRAINT [PK_Order_Details] PRIMARY KEY ([OrderID], [ProductID])
    );
    
    PRINT 'Basic table structure created successfully.'
END
ELSE
BEGIN
    PRINT 'Tables already exist. Skipping table creation.'
END

PRINT 'Database deployment completed successfully!' 