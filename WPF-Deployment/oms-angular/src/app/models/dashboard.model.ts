export interface DashboardDto {
  TotalCustomers: number;
  TotalOrders: number;
  TotalProducts: number;
  TotalEmployees: number;
  TotalRevenue: number;
  PendingOrders: number;
  LowStockProducts: number;
  AverageOrderValue: number;
  MonthlyRevenue: MonthlyRevenueDto[];
  TopProducts: TopProductDto[];
  TopCustomers: TopCustomerDto[];
  RecentOrders: RecentOrderDto[];
}

export interface OrderDto {
  OrderID: number;
  CustomerID: string;
  EmployeeID?: number;
  OrderDate?: string;
  RequiredDate?: string;
  ShippedDate?: string;
  ShipVia?: number;
  Freight?: number;
  ShipName: string;
  ShipAddress: string;
  ShipCity: string;
  ShipRegion: string;
  ShipPostalCode: string;
  ShipCountry: string;
  CustomerName: string;
  EmployeeName: string;
  ShipperName: string;
  TotalAmount: number;
  ItemCount: number;
}

export interface OrderItemDto {
  orderID: number;
  productID: number;
  unitPrice: number;
  quantity: number;
  discount: number;
  productName?: string;
  subTotal: number;
}

export interface CustomerDto {
  CustomerID: string;
  CompanyName: string;
  ContactName: string;
  ContactTitle: string;
  Address: string;
  City: string;
  Region: string;
  PostalCode: string;
  Country: string;
  Phone: string;
  Fax: string;
  OrderCount: number;
  TotalSpent: number;
}

export interface CreateCustomerDto {
  CustomerID: string;
  CompanyName: string;
  ContactName: string;
  ContactTitle: string;
  Address: string;
  City: string;
  Region: string;
  PostalCode: string;
  Country: string;
  Phone: string;
  Fax: string;
}

export interface UpdateCustomerDto {
  CompanyName: string;
  ContactName: string;
  ContactTitle: string;
  Address: string;
  City: string;
  Region: string;
  PostalCode: string;
  Country: string;
  Phone: string;
  Fax: string;
}

export interface EmployeeDto {
  EmployeeID: number;
  LastName: string;
  FirstName: string;
  Title: string;
  TitleOfCourtesy: string;
  BirthDate?: string;
  HireDate?: string;
  Address: string;
  City: string;
  Region: string;
  PostalCode: string;
  Country: string;
  HomePhone: string;
  Extension: string;
  Notes: string;
  ReportsTo?: number;
  PhotoPath: string;
  ManagerName: string;
  OrderCount: number;
  TotalSales: number;
}

export interface CreateEmployeeDto {
  LastName: string;
  FirstName: string;
  Title: string;
  TitleOfCourtesy: string;
  BirthDate?: string;
  HireDate?: string;
  Address: string;
  City: string;
  Region: string;
  PostalCode: string;
  Country: string;
  HomePhone: string;
  Extension: string;
  Notes: string;
  ReportsTo?: number;
  PhotoPath: string;
}

export interface ProductDto {
  ProductID: number;
  ProductName: string;
  SupplierID?: number;
  CategoryID?: number;
  QuantityPerUnit: string;
  UnitPrice?: number;
  UnitsInStock?: number;
  UnitsOnOrder?: number;
  ReorderLevel?: number;
  Discontinued: boolean;
  CategoryName: string;
  SupplierName: string;
  TotalSales: number;
  OrderCount: number;
}

export interface CreateProductDto {
  ProductName: string;
  SupplierID?: number;
  CategoryID?: number;
  QuantityPerUnit: string;
  UnitPrice?: number;
  UnitsInStock?: number;
  UnitsOnOrder?: number;
  ReorderLevel?: number;
  Discontinued: boolean;
}

export interface UpdateProductDto {
  ProductName: string;
  SupplierID?: number;
  CategoryID?: number;
  QuantityPerUnit: string;
  UnitPrice?: number;
  UnitsInStock?: number;
  UnitsOnOrder?: number;
  ReorderLevel?: number;
  Discontinued: boolean;
}

export interface TopProductDto {
  ProductID: number;
  ProductName: string;
  TotalSales: number;
  OrderCount: number;
}

export interface TopCustomerDto {
  CustomerID: string;
  CompanyName: string;
  TotalSpent: number;
  OrderCount: number;
}

export interface RecentOrderDto {
  OrderID: number;
  CustomerName: string;
  OrderDate: string;
  TotalAmount: number;
  Status: string;
}

export interface MonthlyRevenueDto {
  Month: string;
  Revenue: number;
  OrderCount: number;
}

export interface CategoryDto {
  categoryID: number;
  categoryName: string;
  description: string;
  picture: number[];
  productCount: number;
}

export interface CreateCategoryDto {
  categoryName: string;
  description: string;
  picture: number[];
}

export interface UpdateCategoryDto {
  categoryName: string;
  description: string;
  picture: number[];
}

export interface SupplierDto {
  supplierID: number;
  companyName: string;
  contactName: string;
  contactTitle: string;
  address: string;
  city: string;
  region: string;
  postalCode: string;
  country: string;
  phone: string;
  fax: string;
  homePage: string;
  productCount: number;
}

export interface CreateSupplierDto {
  companyName: string;
  contactName: string;
  contactTitle: string;
  address: string;
  city: string;
  region: string;
  postalCode: string;
  country: string;
  phone: string;
  fax: string;
  homePage: string;
}

export interface UpdateSupplierDto {
  companyName: string;
  contactName: string;
  contactTitle: string;
  address: string;
  city: string;
  region: string;
  postalCode: string;
  country: string;
  phone: string;
  fax: string;
  homePage: string;
}

export interface ShipperDto {
  shipperID: number;
  companyName: string;
  phone: string;
  orderCount: number;
}

export interface CreateShipperDto {
  companyName: string;
  phone: string;
}

export interface UpdateShipperDto {
  companyName: string;
  phone: string;
}

export interface CountryOrdersDto {
  Country: string;
  OrderCount: number;
}

export interface CategorySalesDto {
  CategoryName: string;
  TotalSales: number;
  ProductCount: number;
}

export interface CountrySalesDto {
  Country: string;
  TotalSales: number;
  OrderCount: number;
}

export interface CustomerPurchasesDto {
  CustomerID: string;
  CompanyName: string;
  TotalPurchases: number;
  OrderCount: number;
}

export interface EmployeeSalesDto {
  EmployeeID: number;
  EmployeeName: string;
  TotalSales: number;
  OrderCount: number;
}

export interface CustomerCountryDto {
  Country: string;
  CustomerCount: number;
}

export interface ProductCategoryDto {
  CategoryName: string;
  ProductCount: number;
} 