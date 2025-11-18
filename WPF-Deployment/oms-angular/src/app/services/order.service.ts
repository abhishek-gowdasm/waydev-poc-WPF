import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrderDto } from '../models/dashboard.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private ordersSubject = new BehaviorSubject<OrderDto[]>([]);
  public orders$ = this.ordersSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.loadOrders();
  }

  loadOrders(): void {
    this.apiService.getOrders().subscribe({
      next: (orders) => {
        if (orders && orders.length > 0) {
          // Map backend properties to frontend format if needed
          const mappedOrders = orders.map((order: any) => ({
            OrderID: order.OrderID || 0,
            CustomerID: order.CustomerID || '',
            EmployeeID: order.EmployeeID || null,
            OrderDate: order.OrderDate || '',
            RequiredDate: order.RequiredDate || '',
            ShippedDate: order.ShippedDate || '',
            ShipVia: order.ShipVia || null,
            Freight: order.Freight || null,
            ShipName: order.ShipName || '',
            ShipAddress: order.ShipAddress || '',
            ShipCity: order.ShipCity || '',
            ShipRegion: order.ShipRegion || '',
            ShipPostalCode: order.ShipPostalCode || '',
            ShipCountry: order.ShipCountry || '',
            CustomerName: order.CustomerName || '',
            EmployeeName: order.EmployeeName || '',
            ShipperName: order.ShipperName || '',
            TotalAmount: order.TotalAmount || 0,
            ItemCount: order.ItemCount || 0
          }));
          this.ordersSubject.next(mappedOrders);
        }
      },
      error: (error) => {
        console.error('Error loading orders:', error);
      }
    });
  }

  getOrders(): OrderDto[] {
    return this.ordersSubject.value;
  }

  getOrder(id: number): Observable<OrderDto> {
    return this.apiService.getOrder(id);
  }

  getOrdersByCustomer(customerId: string): Observable<OrderDto[]> {
    return this.apiService.getOrdersByCustomer(customerId);
  }

  getOrdersByEmployee(employeeId: number): Observable<OrderDto[]> {
    return this.apiService.getOrdersByEmployee(employeeId);
  }

  getPendingOrders(): Observable<OrderDto[]> {
    return this.apiService.getPendingOrders();
  }

  refreshOrders(): void {
    this.loadOrders();
  }
} 