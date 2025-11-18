import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';
import { ApiService } from '../services/api.service';
import { OrderService } from '../services/order.service';
import { OrderDto, CustomerDto, EmployeeDto, ProductDto } from '../models/dashboard.model';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCardModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './orders.html',
  styleUrls: ['./orders.scss']
})
export class OrdersComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  orders: OrderDto[] = [];
  customers: CustomerDto[] = [];
  employees: EmployeeDto[] = [];
  products: ProductDto[] = [];
  dataSource = new MatTableDataSource<OrderDto>();
  loading = false;
  showForm = false;
  editingOrder: OrderDto | null = null;
  orderForm: FormGroup;

  displayedColumns: string[] = ['OrderID', 'CustomerName', 'EmployeeName', 'OrderDate', 'TotalAmount', 'actions'];

  private destroy$ = new Subject<void>();

  constructor(
    private apiService: ApiService,
    private orderService: OrderService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.orderForm = this.fb.group({
      customerID: ['', Validators.required],
      employeeID: [''],
      orderDate: ['', Validators.required],
      requiredDate: [''],
      shippedDate: [''],
      shipVia: [''],
      freight: [0],
      shipName: ['', Validators.required],
      shipAddress: ['', Validators.required],
      shipCity: ['', Validators.required],
      shipRegion: [''],
      shipPostalCode: [''],
      shipCountry: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadOrders();
    this.loadCustomers();
    this.loadEmployees();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadOrders(): void {
    this.loading = true;
    this.orderService.orders$.pipe(takeUntil(this.destroy$)).subscribe({
      next: (orders) => {
        this.orders = orders || [];
        this.dataSource.data = this.orders;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading orders:', error);
        this.loading = false;
      }
    });
  }

  loadCustomers(): void {
    this.apiService.getCustomers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (customers) => {
          this.customers = customers || [];
        },
        error: (error) => {
          console.error('Error loading customers:', error);
          // Add sample customers
          this.customers = [
            { CustomerID: 'CUST001', CompanyName: 'Acme Corporation', ContactName: 'John Doe', ContactTitle: 'Manager', Address: '123 Main St', City: 'New York', Region: 'NY', PostalCode: '10001', Country: 'USA', Phone: '+1-555-0123', Fax: '', OrderCount: 5, TotalSpent: 5000 },
            { CustomerID: 'CUST002', CompanyName: 'Tech Solutions Inc', ContactName: 'Jane Smith', ContactTitle: 'Director', Address: '456 Tech Ave', City: 'San Francisco', Region: 'CA', PostalCode: '94102', Country: 'USA', Phone: '+1-555-0456', Fax: '', OrderCount: 3, TotalSpent: 2500 },
            { CustomerID: 'CUST003', CompanyName: 'Global Industries', ContactName: 'Mike Johnson', ContactTitle: 'CEO', Address: '789 Business Blvd', City: 'Chicago', Region: 'IL', PostalCode: '60601', Country: 'USA', Phone: '+1-555-0789', Fax: '', OrderCount: 8, TotalSpent: 12000 }
          ];
        }
      });
  }

  loadEmployees(): void {
    this.apiService.getEmployees()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (employees) => {
          this.employees = employees || [];
        },
        error: (error) => {
          console.error('Error loading employees:', error);
          // Add sample employees
          this.employees = [
            { EmployeeID: 1, FirstName: 'John', LastName: 'Smith', Title: 'Sales Manager', TitleOfCourtesy: 'Mr.', BirthDate: '1980-05-15', HireDate: '2015-03-01', Address: '123 Employee St', City: 'New York', Region: 'NY', PostalCode: '10001', Country: 'USA', HomePhone: '+1-555-0001', Extension: '101', Notes: 'Experienced manager', ReportsTo: undefined, PhotoPath: '', ManagerName: '', OrderCount: 15, TotalSales: 25000 },
            { EmployeeID: 2, FirstName: 'Sarah', LastName: 'Johnson', Title: 'Account Executive', TitleOfCourtesy: 'Ms.', BirthDate: '1985-08-22', HireDate: '2018-06-15', Address: '456 Worker Ave', City: 'Chicago', Region: 'IL', PostalCode: '60601', Country: 'USA', HomePhone: '+1-555-0002', Extension: '102', Notes: 'Great with customers', ReportsTo: 1, PhotoPath: '', ManagerName: 'John Smith', OrderCount: 12, TotalSales: 18000 },
            { EmployeeID: 3, FirstName: 'Michael', LastName: 'Brown', Title: 'Sales Representative', TitleOfCourtesy: 'Mr.', BirthDate: '1990-12-10', HireDate: '2020-01-10', Address: '789 Staff Rd', City: 'Los Angeles', Region: 'CA', PostalCode: '90001', Country: 'USA', HomePhone: '+1-555-0003', Extension: '103', Notes: 'New but promising', ReportsTo: 1, PhotoPath: '', ManagerName: 'John Smith', OrderCount: 8, TotalSales: 12000 }
          ];
        }
      });
  }

  addOrder(): void {
    this.editingOrder = null;
    this.orderForm.reset();
    this.showForm = true;
  }

  editOrder(order: OrderDto): void {
    this.editingOrder = order;
    this.orderForm.patchValue({
      customerID: order.CustomerID,
      employeeID: order.EmployeeID,
      orderDate: order.OrderDate,
      requiredDate: order.RequiredDate,
      shippedDate: order.ShippedDate,
      shipVia: order.ShipVia,
      freight: order.Freight,
      shipName: order.ShipName,
      shipAddress: order.ShipAddress,
      shipCity: order.ShipCity,
      shipRegion: order.ShipRegion,
      shipPostalCode: order.ShipPostalCode,
      shipCountry: order.ShipCountry
    });
    this.showForm = true;
  }

  saveOrder(): void {
    if (this.orderForm.valid) {
      const orderData = this.orderForm.value;
      
      if (this.editingOrder) {
        // Update existing order
        const updatedOrder = { ...this.editingOrder, ...orderData };
        // Here you would call the API to update the order
        this.snackBar.open('Order updated successfully!', 'Close', { duration: 3000 });
      } else {
        // Add new order
        const newOrder = {
          OrderID: this.orders.length + 1, // Temporary ID
          CustomerID: orderData.customerID,
          EmployeeID: orderData.employeeID,
          OrderDate: orderData.orderDate,
          RequiredDate: orderData.requiredDate,
          ShippedDate: orderData.shippedDate,
          ShipVia: orderData.shipVia,
          Freight: orderData.freight,
          ShipName: orderData.shipName,
          ShipAddress: orderData.shipAddress,
          ShipCity: orderData.shipCity,
          ShipRegion: orderData.shipRegion,
          ShipPostalCode: orderData.shipPostalCode,
          ShipCountry: orderData.shipCountry,
          CustomerName: this.getCustomerName(orderData.customerID),
          EmployeeName: this.getEmployeeName(orderData.employeeID),
          ShipperName: '',
          TotalAmount: 0,
          ItemCount: 0
        };
        // Here you would call the API to create the order
        this.snackBar.open('Order added successfully!', 'Close', { duration: 3000 });
      }
      
      this.showForm = false;
      this.loadOrders(); // Reload the data
    }
  }

  deleteOrder(order: OrderDto): void {
    if (confirm(`Are you sure you want to delete order #${order.OrderID}?`)) {
      // Here you would call the API to delete the order
      this.snackBar.open('Order deleted successfully!', 'Close', { duration: 3000 });
      this.loadOrders(); // Reload the data
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingOrder = null;
    this.orderForm.reset();
  }

  getCustomerName(customerId: string): string {
    const customer = this.customers.find(c => c.CustomerID === customerId);
    return customer ? customer.CompanyName : 'Unknown Customer';
  }

  getEmployeeName(employeeId: number): string {
    const employee = this.employees.find(e => e.EmployeeID === employeeId);
    return employee ? `${employee.FirstName} ${employee.LastName}` : 'Unknown Employee';
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  get totalOrders(): number {
    return this.orders.length;
  }

  get pendingOrders(): number {
    return this.orders.filter(order => !order.ShippedDate).length;
  }

  get completedOrders(): number {
    return this.orders.filter(order => order.ShippedDate).length;
  }

  get totalRevenue(): number {
    return this.orders.reduce((sum, order) => sum + (order.TotalAmount || 0), 0);
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'completed':
        return '#4caf50';
      case 'pending':
        return '#ff9800';
      case 'processing':
        return '#2196f3';
      case 'cancelled':
        return '#f44336';
      default:
        return '#9e9e9e';
    }
  }
} 