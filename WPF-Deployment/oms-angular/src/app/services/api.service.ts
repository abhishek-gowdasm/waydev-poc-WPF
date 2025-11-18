import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  DashboardDto, 
  OrderDto, 
  CustomerDto, 
  EmployeeDto, 
  ProductDto,
  TopProductDto,
  TopCustomerDto,
  RecentOrderDto,
  MonthlyRevenueDto,
  CreateCustomerDto,
  UpdateCustomerDto,
  CreateEmployeeDto,
  CreateProductDto,
  UpdateProductDto,
  CategoryDto,
  CreateCategoryDto,
  UpdateCategoryDto,
  SupplierDto,
  CreateSupplierDto,
  UpdateSupplierDto,
  ShipperDto,
  CreateShipperDto,
  UpdateShipperDto,
  CountryOrdersDto,
  CategorySalesDto,
  CountrySalesDto,
  CustomerPurchasesDto,
  EmployeeSalesDto,
  CustomerCountryDto,
  ProductCategoryDto
} from '../models/dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://ordermanagementsystem20250812092938-drg6gmcebec5fvbz.centralindia-01.azurewebsites.net/api';

  constructor(private http: HttpClient) { }

  // Dashboard endpoints
  getDashboardData(): Observable<DashboardDto> {
    return this.http.get<DashboardDto>(`${this.baseUrl}/dashboard`);
  }

  getMonthlyRevenue(year: number): Observable<MonthlyRevenueDto[]> {
    return this.http.get<MonthlyRevenueDto[]>(`${this.baseUrl}/dashboard/monthly-revenue/${year}`);
  }

  getTopProducts(count: number): Observable<TopProductDto[]> {
    return this.http.get<TopProductDto[]>(`${this.baseUrl}/dashboard/top-products/${count}`);
  }

  getTopCustomers(count: number): Observable<TopCustomerDto[]> {
    return this.http.get<TopCustomerDto[]>(`${this.baseUrl}/dashboard/top-customers/${count}`);
  }

  getRecentOrders(count: number): Observable<RecentOrderDto[]> {
    return this.http.get<RecentOrderDto[]>(`${this.baseUrl}/dashboard/recent-orders/${count}`);
  }

  getCountryOrders(count: number): Observable<CountryOrdersDto[]> {
    return this.http.get<CountryOrdersDto[]>(`${this.baseUrl}/dashboard/country-orders/${count}`);
  }

  getCategorySales(): Observable<CategorySalesDto[]> {
    return this.http.get<CategorySalesDto[]>(`${this.baseUrl}/dashboard/category-sales`);
  }

  getCountrySales(): Observable<CountrySalesDto[]> {
    return this.http.get<CountrySalesDto[]>(`${this.baseUrl}/dashboard/country-sales`);
  }

  getCustomerPurchases(count: number): Observable<CustomerPurchasesDto[]> {
    return this.http.get<CustomerPurchasesDto[]>(`${this.baseUrl}/dashboard/customer-purchases/${count}`);
  }

  getEmployeeSales(): Observable<EmployeeSalesDto[]> {
    return this.http.get<EmployeeSalesDto[]>(`${this.baseUrl}/dashboard/employee-sales`);
  }

  getCustomerCountries(): Observable<CustomerCountryDto[]> {
    return this.http.get<CustomerCountryDto[]>(`${this.baseUrl}/dashboard/customer-countries`);
  }

  getProductCategories(): Observable<ProductCategoryDto[]> {
    return this.http.get<ProductCategoryDto[]>(`${this.baseUrl}/dashboard/product-categories`);
  }

  // Orders endpoints
  getOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(`${this.baseUrl}/orders`);
  }

  getOrder(id: number): Observable<OrderDto> {
    return this.http.get<OrderDto>(`${this.baseUrl}/orders/${id}`);
  }

  getOrdersByCustomer(customerId: string): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(`${this.baseUrl}/orders/customer/${customerId}`);
  }

  getOrdersByEmployee(employeeId: number): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(`${this.baseUrl}/orders/employee/${employeeId}`);
  }

  getPendingOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(`${this.baseUrl}/orders/pending`);
  }

  // Customers endpoints
  getCustomers(): Observable<CustomerDto[]> {
    return this.http.get<CustomerDto[]>(`${this.baseUrl}/customers`);
  }

  getCustomer(id: string): Observable<CustomerDto> {
    return this.http.get<CustomerDto>(`${this.baseUrl}/customers/${id}`);
  }

  searchCustomers(searchTerm: string): Observable<CustomerDto[]> {
    return this.http.get<CustomerDto[]>(`${this.baseUrl}/customers/search?searchTerm=${searchTerm}`);
  }

  getCustomersByCountry(country: string): Observable<CustomerDto[]> {
    return this.http.get<CustomerDto[]>(`${this.baseUrl}/customers/country/${country}`);
  }

  getTopCustomersBySpending(count: number): Observable<CustomerDto[]> {
    return this.http.get<CustomerDto[]>(`${this.baseUrl}/customers/top/${count}`);
  }

  createCustomer(customer: CreateCustomerDto): Observable<CustomerDto> {
    console.log('API Service: Creating customer:', customer);
    console.log('API Service: Request URL:', `${this.baseUrl}/customers`);
    return this.http.post<CustomerDto>(`${this.baseUrl}/customers`, customer);
  }

  updateCustomer(id: string, customer: UpdateCustomerDto): Observable<CustomerDto> {
    return this.http.put<CustomerDto>(`${this.baseUrl}/customers/${id}`, customer);
  }

  deleteCustomer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/customers/${id}`);
  }

  // Employees endpoints
  getEmployees(): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.baseUrl}/employees`);
  }

  getEmployee(id: number): Observable<EmployeeDto> {
    return this.http.get<EmployeeDto>(`${this.baseUrl}/employees/${id}`);
  }

  searchEmployees(searchTerm: string): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.baseUrl}/employees/search?searchTerm=${searchTerm}`);
  }

  getEmployeesByTitle(title: string): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.baseUrl}/employees/title/${title}`);
  }

  getTopEmployees(count: number): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.baseUrl}/employees/top/${count}`);
  }

  createEmployee(employee: CreateEmployeeDto): Observable<EmployeeDto> {
    return this.http.post<EmployeeDto>(`${this.baseUrl}/employees`, employee);
  }

  updateEmployee(id: number, employee: Partial<CreateEmployeeDto>): Observable<EmployeeDto> {
    return this.http.put<EmployeeDto>(`${this.baseUrl}/employees/${id}`, employee);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/employees/${id}`);
  }

  // Products endpoints
  getProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products`);
  }

  getProduct(id: number): Observable<ProductDto> {
    return this.http.get<ProductDto>(`${this.baseUrl}/products/${id}`);
  }

  searchProducts(searchTerm: string): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products/search?searchTerm=${searchTerm}`);
  }

  getProductsByCategory(categoryId: number): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products/category/${categoryId}`);
  }

  getProductsBySupplier(supplierId: number): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products/supplier/${supplierId}`);
  }

  getDiscontinuedProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products/discontinued`);
  }

  getLowStockProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products/low-stock`);
  }

  getTopProductsBySales(count: number): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.baseUrl}/products/top/${count}`);
  }

  createProduct(product: CreateProductDto): Observable<ProductDto> {
    return this.http.post<ProductDto>(`${this.baseUrl}/products`, product);
  }

  updateProduct(id: number, product: UpdateProductDto): Observable<ProductDto> {
    return this.http.put<ProductDto>(`${this.baseUrl}/products/${id}`, product);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/products/${id}`);
  }

  // Categories endpoints
  getCategories(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(`${this.baseUrl}/categories`);
  }

  getCategory(id: number): Observable<CategoryDto> {
    return this.http.get<CategoryDto>(`${this.baseUrl}/categories/${id}`);
  }

  createCategory(category: CreateCategoryDto): Observable<CategoryDto> {
    return this.http.post<CategoryDto>(`${this.baseUrl}/categories`, category);
  }

  updateCategory(id: number, category: UpdateCategoryDto): Observable<CategoryDto> {
    return this.http.put<CategoryDto>(`${this.baseUrl}/categories/${id}`, category);
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/categories/${id}`);
  }

  // Suppliers endpoints
  getSuppliers(): Observable<SupplierDto[]> {
    return this.http.get<SupplierDto[]>(`${this.baseUrl}/suppliers`);
  }

  getSupplier(id: number): Observable<SupplierDto> {
    return this.http.get<SupplierDto>(`${this.baseUrl}/suppliers/${id}`);
  }

  searchSuppliers(searchTerm: string): Observable<SupplierDto[]> {
    return this.http.get<SupplierDto[]>(`${this.baseUrl}/suppliers/search?searchTerm=${searchTerm}`);
  }

  getSuppliersByCountry(country: string): Observable<SupplierDto[]> {
    return this.http.get<SupplierDto[]>(`${this.baseUrl}/suppliers/country/${country}`);
  }

  createSupplier(supplier: CreateSupplierDto): Observable<SupplierDto> {
    return this.http.post<SupplierDto>(`${this.baseUrl}/suppliers`, supplier);
  }

  updateSupplier(id: number, supplier: UpdateSupplierDto): Observable<SupplierDto> {
    return this.http.put<SupplierDto>(`${this.baseUrl}/suppliers/${id}`, supplier);
  }

  deleteSupplier(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/suppliers/${id}`);
  }

  // Shippers endpoints
  getShippers(): Observable<ShipperDto[]> {
    return this.http.get<ShipperDto[]>(`${this.baseUrl}/shippers`);
  }

  getShipper(id: number): Observable<ShipperDto> {
    return this.http.get<ShipperDto>(`${this.baseUrl}/shippers/${id}`);
  }

  createShipper(shipper: CreateShipperDto): Observable<ShipperDto> {
    return this.http.post<ShipperDto>(`${this.baseUrl}/shippers`, shipper);
  }

  updateShipper(id: number, shipper: UpdateShipperDto): Observable<ShipperDto> {
    return this.http.put<ShipperDto>(`${this.baseUrl}/shippers/${id}`, shipper);
  }

  deleteShipper(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/shippers/${id}`);
  }
} 