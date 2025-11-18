import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { CustomerDto, CreateCustomerDto, UpdateCustomerDto } from '../models/dashboard.model';
import { ApiService } from './api.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private customersSubject = new BehaviorSubject<CustomerDto[]>([]);
  public customers$ = this.customersSubject.asObservable();

  constructor(private apiService: ApiService, private snackBar: MatSnackBar) {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.apiService.getCustomers().subscribe({
      next: (customers) => {
        if (customers && customers.length > 0) {
          // Map backend properties to frontend format if needed
          const mappedCustomers = customers.map((customer: any) => ({
            CustomerID: customer.CustomerID || '',
            CompanyName: customer.CompanyName || '',
            ContactName: customer.ContactName || '',
            ContactTitle: customer.ContactTitle || '',
            Address: customer.Address || '',
            City: customer.City || '',
            Region: customer.Region || '',
            PostalCode: customer.PostalCode || '',
            Country: customer.Country || '',
            Phone: customer.Phone || '',
            Fax: customer.Fax || '',
            OrderCount: customer.OrderCount || 0,
            TotalSpent: customer.TotalSpent || 0
          }));
          this.customersSubject.next(mappedCustomers);
        }
      },
      error: (error) => {
        console.error('Error loading customers:', error);
      }
    });
  }

  getCustomers(): CustomerDto[] {
    return this.customersSubject.value;
  }

  addCustomer(customerData: CreateCustomerDto): void {
    this.apiService.createCustomer(customerData).subscribe({
      next: (newCustomer) => {
        console.log('Customer created successfully:', newCustomer);
        const currentCustomers = this.customersSubject.value;
        // Map the new customer to match the expected format
        const mappedCustomer: CustomerDto = {
          CustomerID: newCustomer.CustomerID || '',
          CompanyName: newCustomer.CompanyName || '',
          ContactName: newCustomer.ContactName || '',
          ContactTitle: newCustomer.ContactTitle || '',
          Address: newCustomer.Address || '',
          City: newCustomer.City || '',
          Region: newCustomer.Region || '',
          PostalCode: newCustomer.PostalCode || '',
          Country: newCustomer.Country || '',
          Phone: newCustomer.Phone || '',
          Fax: newCustomer.Fax || '',
          OrderCount: newCustomer.OrderCount || 0,
          TotalSpent: newCustomer.TotalSpent || 0
        };
        this.customersSubject.next([...currentCustomers, mappedCustomer]);
        this.snackBar.open('Customer added successfully!', 'Close', { duration: 3000 });
      },
      error: (error) => {
        console.error('Error creating customer:', error);
        // Show error message to user
        this.snackBar.open('Error creating customer: ' + (error.error?.error || error.message || 'Unknown error'), 'Close', { duration: 5000 });
      }
    });
  }

  updateCustomer(id: string, customerData: UpdateCustomerDto): void {
    this.apiService.updateCustomer(id, customerData).subscribe({
      next: (updatedCustomer) => {
        const currentCustomers = this.customersSubject.value;
        const updatedCustomers = currentCustomers.map(customer =>
          customer.CustomerID === id ? updatedCustomer : customer
        );
        this.customersSubject.next(updatedCustomers);
      },
      error: (error) => {
        console.error('Error updating customer:', error);
      }
    });
  }

  deleteCustomer(id: string): void {
    this.apiService.deleteCustomer(id).subscribe({
      next: () => {
        const currentCustomers = this.customersSubject.value;
        const filteredCustomers = currentCustomers.filter(customer => customer.CustomerID !== id);
        this.customersSubject.next(filteredCustomers);
      },
      error: (error) => {
        console.error('Error deleting customer:', error);
      }
    });
  }

  refreshCustomers(): void {
    this.loadCustomers();
  }

  searchCustomers(searchTerm: string): Observable<CustomerDto[]> {
    return this.apiService.searchCustomers(searchTerm);
  }

  getCustomersByCountry(country: string): Observable<CustomerDto[]> {
    return this.apiService.getCustomersByCountry(country);
  }

  getTopCustomers(count: number): Observable<CustomerDto[]> {
    return this.apiService.getTopCustomersBySpending(count);
  }
} 