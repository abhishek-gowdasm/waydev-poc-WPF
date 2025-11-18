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
import { EmployeeDto, CreateEmployeeDto } from '../models/dashboard.model';

@Component({
  selector: 'app-employees',
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
  templateUrl: './employees.html',
  styleUrls: ['./employees.scss']
})
export class EmployeesComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  employees: EmployeeDto[] = [];
  dataSource = new MatTableDataSource<EmployeeDto>();
  loading = false;
  showForm = false;
  editingEmployee: EmployeeDto | null = null;
  employeeForm: FormGroup;

  displayedColumns: string[] = ['EmployeeID', 'FirstName', 'LastName', 'Title', 'HireDate', 'OrderCount', 'TotalSales', 'actions'];

  private destroy$ = new Subject<void>();

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.employeeForm = this.fb.group({
      FirstName: ['', [Validators.required, Validators.minLength(2)]],
      LastName: ['', [Validators.required, Validators.minLength(2)]],
      Title: ['', Validators.required],
      TitleOfCourtesy: ['', Validators.required],
      BirthDate: [''],
      HireDate: [''],
      Address: ['', Validators.required],
      City: ['', Validators.required],
      Region: [''],
      PostalCode: [''],
      Country: ['', Validators.required],
      HomePhone: ['', Validators.required],
      Extension: [''],
      Notes: [''],
      ReportsTo: [null],
      PhotoPath: ['']
    });
  }

  ngOnInit(): void {
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

  loadEmployees(): void {
    this.loading = true;
    this.apiService.getEmployees()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (employees) => {
          this.employees = employees || [];
          this.dataSource.data = this.employees;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading employees:', error);
          this.employees = [];
          this.dataSource.data = [];
          this.loading = false;
          this.snackBar.open('Error loading employees', 'Close', { duration: 3000 });
        }
      });
  }

  addEmployee(): void {
    this.editingEmployee = null;
    this.employeeForm.reset();
    this.showForm = true;
  }

  editEmployee(employee: EmployeeDto): void {
    this.editingEmployee = employee;
    this.employeeForm.patchValue({
      FirstName: employee.FirstName,
      LastName: employee.LastName,
      Title: employee.Title,
      TitleOfCourtesy: employee.TitleOfCourtesy,
      BirthDate: employee.BirthDate || '',
      HireDate: employee.HireDate || '',
      Address: employee.Address,
      City: employee.City,
      Region: employee.Region,
      PostalCode: employee.PostalCode,
      Country: employee.Country,
      HomePhone: employee.HomePhone,
      Extension: employee.Extension,
      Notes: employee.Notes,
      ReportsTo: employee.ReportsTo,
      PhotoPath: employee.PhotoPath
    });
    this.showForm = true;
  }

  saveEmployee(): void {
    if (this.employeeForm.valid) {
      const employeeData = this.employeeForm.value;
      
      if (this.editingEmployee) {
        // Update existing employee
        const updatedEmployee = { ...this.editingEmployee, ...employeeData };
        // Here you would call the API to update the employee
        this.snackBar.open('Employee updated successfully!', 'Close', { duration: 3000 });
      } else {
        // Add new employee
        const newEmployee: CreateEmployeeDto = {
          FirstName: employeeData.FirstName,
          LastName: employeeData.LastName,
          Title: employeeData.Title,
          TitleOfCourtesy: employeeData.TitleOfCourtesy,
          BirthDate: employeeData.BirthDate,
          HireDate: employeeData.HireDate,
          Address: employeeData.Address,
          City: employeeData.City,
          Region: employeeData.Region,
          PostalCode: employeeData.PostalCode,
          Country: employeeData.Country,
          HomePhone: employeeData.HomePhone,
          Extension: employeeData.Extension,
          Notes: employeeData.Notes,
          ReportsTo: employeeData.ReportsTo,
          PhotoPath: employeeData.PhotoPath
        };
        // Here you would call the API to create the employee
        this.snackBar.open('Employee added successfully!', 'Close', { duration: 3000 });
      }
      
      this.showForm = false;
      this.loadEmployees(); // Reload the data
    }
  }

  deleteEmployee(employee: EmployeeDto): void {
            if (confirm(`Are you sure you want to delete ${employee.FirstName} ${employee.LastName}?`)) {
      // Here you would call the API to delete the employee
      this.snackBar.open('Employee deleted successfully!', 'Close', { duration: 3000 });
      this.loadEmployees(); // Reload the data
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingEmployee = null;
    this.employeeForm.reset();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  get totalEmployees(): number {
    return this.employees.length;
  }

  get totalTitles(): number {
    return new Set(this.employees.map(e => e.Title)).size;
  }

  get totalOrders(): number {
    return this.employees.reduce((sum, emp) => sum + (emp.OrderCount || 0), 0);
  }

  get totalRevenue(): number {
    return this.employees.reduce((sum, emp) => sum + (emp.TotalSales || 0), 0);
  }
} 