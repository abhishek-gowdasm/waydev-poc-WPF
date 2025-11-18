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
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';
import { CustomerService } from '../services/customer.service';
import { CustomerDto, CreateCustomerDto, UpdateCustomerDto } from '../models/dashboard.model';

@Component({
  selector: 'app-customers',
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
    MatDialogModule,
    MatSnackBarModule,
    MatChipsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './customers.html',
  styleUrls: ['./customers.scss']
})
export class CustomersComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  customers: CustomerDto[] = [];
  dataSource = new MatTableDataSource<CustomerDto>();
  loading = false;
  showForm = false;
  editingCustomer: CustomerDto | null = null;
  customerForm: FormGroup;

  displayedColumns: string[] = [
    'companyName',
    'contactName',
    'contactTitle',
    'country',
    'phone',
    'orderCount',
    'totalSpent',
    'actions'
  ];

  private destroy$ = new Subject<void>();

  constructor(
    private customerService: CustomerService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.customerForm = this.fb.group({
      CustomerID: ['', [Validators.required, Validators.maxLength(5), Validators.minLength(1)]],
      CompanyName: ['', [Validators.required, Validators.minLength(2)]],
      ContactName: ['', [Validators.required, Validators.minLength(2)]],
      ContactTitle: ['', Validators.required],
      Address: ['', Validators.required],
      City: ['', Validators.required],
      Region: [''],
      PostalCode: [''],
      Country: ['', Validators.required],
      Phone: ['', Validators.required],
      Fax: ['']
    });
  }

  ngOnInit(): void {
    this.loading = true;
    console.log('Customers component initializing...');
    
    this.customerService.customers$.pipe(takeUntil(this.destroy$)).subscribe({
      next: (customers) => {
        console.log('Received customers:', customers);
        this.customers = customers || [];
        this.dataSource.data = this.customers;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading customers:', error);
        this.loading = false;
      }
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  addCustomer(): void {
    this.editingCustomer = null;
    this.customerForm.reset();
    this.showForm = true;
  }

  editCustomer(customer: CustomerDto): void {
    this.editingCustomer = customer;
    this.customerForm.patchValue({
      CustomerID: customer.CustomerID,
      CompanyName: customer.CompanyName,
      ContactName: customer.ContactName,
      ContactTitle: customer.ContactTitle,
      Address: customer.Address,
      City: customer.City,
      Region: customer.Region,
      PostalCode: customer.PostalCode,
      Country: customer.Country,
      Phone: customer.Phone,
      Fax: customer.Fax
    });
    this.showForm = true;
  }

  saveCustomer(): void {
    if (this.customerForm.valid) {
      const customerData = this.customerForm.value;
      
      if (this.editingCustomer) {
        // Update existing customer
        const updateData: UpdateCustomerDto = {
          CompanyName: customerData.CompanyName,
          ContactName: customerData.ContactName,
          ContactTitle: customerData.ContactTitle,
          Address: customerData.Address,
          City: customerData.City,
          Region: customerData.Region,
          PostalCode: customerData.PostalCode,
          Country: customerData.Country,
          Phone: customerData.Phone,
          Fax: customerData.Fax
        };

        this.customerService.updateCustomer(this.editingCustomer.CustomerID, updateData);
        this.snackBar.open('Customer updated successfully!', 'Close', { duration: 3000 });
        this.showForm = false;
        this.editingCustomer = null;
      } else {
        // Add new customer
        const newCustomerData: CreateCustomerDto = {
          CustomerID: customerData.CustomerID,
          CompanyName: customerData.CompanyName,
          ContactName: customerData.ContactName,
          ContactTitle: customerData.ContactTitle,
          Address: customerData.Address,
          City: customerData.City,
          Region: customerData.Region,
          PostalCode: customerData.PostalCode,
          Country: customerData.Country,
          Phone: customerData.Phone,
          Fax: customerData.Fax
        };

        this.customerService.addCustomer(newCustomerData);
        // Note: Success message will be shown by the service after successful creation
        this.showForm = false;
      }
    }
  }

  deleteCustomer(customer: CustomerDto): void {
    if (confirm(`Are you sure you want to delete ${customer.CompanyName}?`)) {
      this.customerService.deleteCustomer(customer.CustomerID);
      this.snackBar.open('Customer deleted successfully!', 'Close', { duration: 3000 });
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingCustomer = null;
    this.customerForm.reset();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  get totalCustomers(): number {
    return this.customers.length;
  }

  get totalCountries(): number {
    return new Set(this.customers.map(c => c.Country)).size;
  }

  get totalOrders(): number {
    return this.customers.reduce((sum, c) => sum + (c.OrderCount || 0), 0);
  }

  get totalRevenue(): number {
    return this.customers.reduce((sum, c) => sum + (c.TotalSpent || 0), 0);
  }
} 