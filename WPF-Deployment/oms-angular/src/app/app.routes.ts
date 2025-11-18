import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard';
import { OrdersComponent } from './orders/orders';
import { CustomersComponent } from './customers/customers';
import { ProductsComponent } from './products/products';
import { EmployeesComponent } from './employees/employees';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'customers', component: CustomersComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'employees', component: EmployeesComponent },
  { path: '**', redirectTo: '/dashboard' }
];
