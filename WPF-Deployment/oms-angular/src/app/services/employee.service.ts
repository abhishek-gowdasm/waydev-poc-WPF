import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { EmployeeDto, CreateEmployeeDto } from '../models/dashboard.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private employeesSubject = new BehaviorSubject<EmployeeDto[]>([]);
  public employees$ = this.employeesSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.apiService.getEmployees().subscribe({
      next: (employees) => {
        if (employees && employees.length > 0) {
          // Map backend properties to frontend format if needed
          const mappedEmployees = employees.map((employee: any) => ({
            EmployeeID: employee.EmployeeID || 0,
            LastName: employee.LastName || '',
            FirstName: employee.FirstName || '',
            Title: employee.Title || '',
            TitleOfCourtesy: employee.TitleOfCourtesy || '',
            BirthDate: employee.BirthDate || '',
            HireDate: employee.HireDate || '',
            Address: employee.Address || '',
            City: employee.City || '',
            Region: employee.Region || '',
            PostalCode: employee.PostalCode || '',
            Country: employee.Country || '',
            HomePhone: employee.HomePhone || '',
            Extension: employee.Extension || '',
            Notes: employee.Notes || '',
            ReportsTo: employee.ReportsTo || null,
            PhotoPath: employee.PhotoPath || '',
            ManagerName: employee.ManagerName || '',
            OrderCount: employee.OrderCount || 0,
            TotalSales: employee.TotalSales || 0
          }));
          this.employeesSubject.next(mappedEmployees);
        }
      },
      error: (error) => {
        console.error('Error loading employees:', error);
      }
    });
  }

  getEmployees(): EmployeeDto[] {
    return this.employeesSubject.value;
  }

  getEmployee(id: number): Observable<EmployeeDto> {
    return this.apiService.getEmployee(id);
  }

  addEmployee(employeeData: CreateEmployeeDto): void {
    this.apiService.createEmployee(employeeData).subscribe({
      next: (newEmployee) => {
        const currentEmployees = this.employeesSubject.value;
        this.employeesSubject.next([...currentEmployees, newEmployee]);
      },
      error: (error) => {
        console.error('Error creating employee:', error);
      }
    });
  }

  updateEmployee(id: number, employeeData: Partial<CreateEmployeeDto>): void {
    this.apiService.updateEmployee(id, employeeData).subscribe({
      next: (updatedEmployee) => {
        const currentEmployees = this.employeesSubject.value;
        const updatedEmployees = currentEmployees.map(employee =>
          employee.EmployeeID === id ? updatedEmployee : employee
        );
        this.employeesSubject.next(updatedEmployees);
      },
      error: (error) => {
        console.error('Error updating employee:', error);
      }
    });
  }

  deleteEmployee(id: number): void {
    this.apiService.deleteEmployee(id).subscribe({
      next: () => {
        const currentEmployees = this.employeesSubject.value;
        const filteredEmployees = currentEmployees.filter(employee => employee.EmployeeID !== id);
        this.employeesSubject.next(filteredEmployees);
      },
      error: (error) => {
        console.error('Error deleting employee:', error);
      }
    });
  }

  refreshEmployees(): void {
    this.loadEmployees();
  }

  searchEmployees(searchTerm: string): Observable<EmployeeDto[]> {
    return this.apiService.searchEmployees(searchTerm);
  }

  getEmployeesByTitle(title: string): Observable<EmployeeDto[]> {
    return this.apiService.getEmployeesByTitle(title);
  }

  getTopEmployees(count: number): Observable<EmployeeDto[]> {
    return this.apiService.getTopEmployees(count);
  }
} 