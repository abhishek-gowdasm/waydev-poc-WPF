import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { Subject, takeUntil } from 'rxjs';
import { ApiService } from '../services/api.service';
import { CustomerService } from '../services/customer.service';
import { 
  DashboardDto, 
  CustomerDto, 
  ProductDto, 
  EmployeeDto, 
  TopProductDto, 
  TopCustomerDto, 
  RecentOrderDto, 
  MonthlyRevenueDto,
  CountryOrdersDto,
  CategorySalesDto,
  CountrySalesDto,
  CustomerPurchasesDto,
  EmployeeSalesDto,
  CustomerCountryDto,
  ProductCategoryDto,
  OrderDto // Added OrderDto import
} from '../models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    BaseChartDirective
  ],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Data
  dashboardData: DashboardDto | null = null;
  customers: CustomerDto[] = [];
  products: ProductDto[] = [];
  employees: EmployeeDto[] = [];
  orders: OrderDto[] = []; // Add orders array
  topProducts: TopProductDto[] = [];
  topCustomers: TopCustomerDto[] = [];
  recentOrders: RecentOrderDto[] = [];
  monthlyRevenue: MonthlyRevenueDto[] = [];
  countryOrders: CountryOrdersDto[] = [];
  categorySales: CategorySalesDto[] = [];
  countrySales: CountrySalesDto[] = [];
  customerPurchases: CustomerPurchasesDto[] = [];
  employeeSales: EmployeeSalesDto[] = [];
  customerCountries: CustomerCountryDto[] = [];
  productCategories: ProductCategoryDto[] = [];

  // Loading states
  loading = true;
  customersLoading = false;
  productsLoading = false;
  employeesLoading = false;

  // Charts
  public ordersChartData: ChartConfiguration<'pie'>['data'] = {
    labels: ['Pending', 'Completed'],
    datasets: [
      {
        data: [0, 0],
        backgroundColor: ['#ff9800', '#4caf50'],
        borderColor: ['#f57c00', '#388e3c'],
        borderWidth: 2
      }
    ]
  };

  public ordersChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom'
      },
      title: {
        display: true,
        text: 'Order Status Distribution'
      }
    }
  };

  public productsChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Revenue',
        backgroundColor: 'rgba(54, 162, 235, 0.8)',
        borderColor: 'rgba(54, 162, 235, 1)',
        borderWidth: 1
      }
    ]
  };

  public productsChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true
      },
      title: {
        display: true,
        text: 'Top Products by Revenue'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          callback: function(value) {
            return '$' + value;
          }
        }
      }
    }
  };

  public customersChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#ff6384', '#36a2eb', '#cc65fe', '#ffce56', '#4bc0c0',
          '#9966ff', '#ff9f40', '#ff6384', '#c9cbcf', '#4bc0c0'
        ],
        borderWidth: 2
      }
    ]
  };

  public customersChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom'
      },
      title: {
        display: true,
        text: 'Top Customers by Orders'
      }
    }
  };

  public employeesChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Orders',
        backgroundColor: 'rgba(255, 99, 132, 0.8)',
        borderColor: 'rgba(255, 99, 132, 1)',
        borderWidth: 1
      },
      {
        data: [],
        label: 'Revenue',
        backgroundColor: 'rgba(75, 192, 192, 0.8)',
        borderColor: 'rgba(75, 192, 192, 1)',
        borderWidth: 1
      }
    ]
  };

  public employeesChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true
      },
      title: {
        display: true,
        text: 'Top Employees Performance'
      }
    },
    scales: {
      y: {
        beginAtZero: true
      }
    }
  };

  public countryOrdersChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#ff6384', '#36a2eb', '#cc65fe', '#ffce56', '#4bc0c0',
          '#9966ff', '#ff9f40', '#ff6384', '#c9cbcf', '#4bc0c0'
        ],
        borderWidth: 2
      }
    ]
  };

  public countryOrdersChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom'
      },
      title: {
        display: true,
        text: 'Orders by Countries (Top 10)'
      }
    }
  };

  public categorySalesChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#ff6384', '#36a2eb', '#cc65fe', '#ffce56', '#4bc0c0',
          '#9966ff', '#ff9f40', '#ff6384', '#c9cbcf', '#4bc0c0'
        ],
        borderWidth: 2
      }
    ]
  };

  public categorySalesChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom'
      },
      title: {
        display: true,
        text: 'Sales by Categories'
      }
    }
  };

  public countrySalesChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Sales',
        backgroundColor: 'rgba(54, 162, 235, 0.8)',
        borderColor: 'rgba(54, 162, 235, 1)',
        borderWidth: 1
      }
    ]
  };

  public countrySalesChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true
      },
      title: {
        display: true,
        text: 'Sales by Countries'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          callback: function(value) {
            return '$' + value;
          }
        }
      }
    }
  };

  public customerPurchasesChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Purchases',
        backgroundColor: 'rgba(255, 99, 132, 0.8)',
        borderColor: 'rgba(255, 99, 132, 1)',
        borderWidth: 1
      }
    ]
  };

  public customerPurchasesChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true
      },
      title: {
        display: true,
        text: 'Purchases by Customers (Top 10)'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          callback: function(value) {
            return '$' + value;
          }
        }
      }
    }
  };

  public employeeSalesChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        label: 'Sales',
        backgroundColor: 'rgba(75, 192, 192, 0.8)',
        borderColor: 'rgba(75, 192, 192, 1)',
        borderWidth: 1
      }
    ]
  };

  public employeeSalesChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true
      },
      title: {
        display: true,
        text: 'Sales by Employee'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          callback: function(value) {
            return '$' + value;
          }
        }
      }
    }
  };

  public customerCountriesChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#ff6384', '#36a2eb', '#cc65fe', '#ffce56', '#4bc0c0',
          '#9966ff', '#ff9f40', '#ff6384', '#c9cbcf', '#4bc0c0'
        ],
        borderWidth: 2
      }
    ]
  };

  public customerCountriesChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom'
      },
      title: {
        display: true,
        text: 'Customers by Country'
      }
    }
  };

  public productCategoriesChartData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#ff6384', '#36a2eb', '#cc65fe', '#ffce56', '#4bc0c0',
          '#9966ff', '#ff9f40', '#ff6384', '#c9cbcf', '#4bc0c0'
        ],
        borderWidth: 2
      }
    ]
  };

  public productCategoriesChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom'
      },
      title: {
        display: true,
        text: 'Products by Categories'
      }
    }
  };

  constructor(
    private apiService: ApiService,
    private customerService: CustomerService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
    this.loadCustomers();
    this.loadProducts();
    this.loadEmployees();
    this.loadOrders(); // Add orders loading
    this.loadCountryOrders();
    this.loadCategorySales();
    this.loadCountrySales();
    this.loadCustomerPurchases();
    this.loadEmployeeSales();
    this.loadCustomerCountries();
    this.loadProductCategories();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadDashboardData(): void {
    this.apiService.getDashboardData()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.dashboardData = data;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading dashboard data:', error);
          this.loading = false;
        }
      });
  }

  loadCustomers(): void {
    this.customersLoading = true;
    this.customerService.customers$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (customers) => {
          this.customers = customers || [];
          this.updateCharts();
          this.customersLoading = false;
        },
        error: (error) => {
          console.error('Error loading customers:', error);
          this.customersLoading = false;
        }
      });
  }

  loadProducts(): void {
    this.productsLoading = true;
    this.apiService.getProducts()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (products) => {
          this.products = products || [];
          this.updateCharts();
          this.productsLoading = false;
        },
        error: (error) => {
          console.error('Error loading products:', error);
          this.productsLoading = false;
        }
      });
  }

  loadEmployees(): void {
    this.employeesLoading = true;
    this.apiService.getEmployees()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (employees) => {
          this.employees = employees || [];
          this.updateCharts();
          this.employeesLoading = false;
        },
        error: (error) => {
          console.error('Error loading employees:', error);
          this.employeesLoading = false;
        }
      });
  }

  loadOrders(): void {
    this.apiService.getOrders()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (orders) => {
          this.orders = orders || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading orders:', error);
          this.orders = [];
          this.updateCharts();
        }
      });
  }

  loadCountryOrders(): void {
    this.apiService.getCountryOrders(10)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (countryOrders) => {
          this.countryOrders = countryOrders || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading country orders:', error);
          this.countryOrders = [];
          this.updateCharts();
        }
      });
  }

  loadCategorySales(): void {
    this.apiService.getCategorySales()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (categorySales) => {
          this.categorySales = categorySales || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading category sales:', error);
          this.categorySales = [];
          this.updateCharts();
        }
      });
  }

  loadCountrySales(): void {
    this.apiService.getCountrySales()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (countrySales) => {
          this.countrySales = countrySales || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading country sales:', error);
          this.countrySales = [];
          this.updateCharts();
        }
      });
  }

  loadCustomerPurchases(): void {
    this.apiService.getCustomerPurchases(10)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (customerPurchases) => {
          this.customerPurchases = customerPurchases || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading customer purchases:', error);
          this.customerPurchases = [];
          this.updateCharts();
        }
      });
  }

  loadEmployeeSales(): void {
    this.apiService.getEmployeeSales()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (employeeSales) => {
          this.employeeSales = employeeSales || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading employee sales:', error);
          this.employeeSales = [];
          this.updateCharts();
        }
      });
  }

  loadCustomerCountries(): void {
    this.apiService.getCustomerCountries()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (customerCountries) => {
          this.customerCountries = customerCountries || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading customer countries:', error);
          this.customerCountries = [];
          this.updateCharts();
        }
      });
  }

  loadProductCategories(): void {
    this.apiService.getProductCategories()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (productCategories) => {
          this.productCategories = productCategories || [];
          this.updateCharts();
        },
        error: (error) => {
          console.error('Error loading product categories:', error);
          this.productCategories = [];
          this.updateCharts();
        }
      });
  }

  updateCharts(): void {
    this.updateOrdersChart();
    this.updateProductsChart();
    this.updateCustomersChart();
    this.updateEmployeesChart();
    this.updateCountryOrdersChart();
    this.updateCategorySalesChart();
    this.updateCountrySalesChart();
    this.updateCustomerPurchasesChart();
    this.updateEmployeeSalesChart();
    this.updateCustomerCountriesChart();
    this.updateProductCategoriesChart();
  }

  updateOrdersChart(): void {
    if (this.orders && this.orders.length > 0) {
      // Calculate order status based on ShippedDate
      const pendingOrders = this.orders.filter(order => !order.ShippedDate || order.ShippedDate === '').length;
      const completedOrders = this.orders.filter(order => order.ShippedDate && order.ShippedDate !== '').length;
      const totalOrders = this.orders.length;
      
      console.log('Order Status Calculation:', {
        TotalOrders: totalOrders,
        PendingOrders: pendingOrders,
        CompletedOrders: completedOrders,
        OrdersWithShippedDate: this.orders.filter(o => o.ShippedDate).length,
        OrdersWithoutShippedDate: this.orders.filter(o => !o.ShippedDate).length
      });
      
      this.ordersChartData.datasets[0].data = [
        pendingOrders,
        completedOrders
      ];
      
      // Update chart labels to show actual values
      this.ordersChartData.labels = [
        `Pending (${pendingOrders})`,
        `Completed (${completedOrders})`
      ];
    } else if (this.dashboardData) {
      // Fallback to dashboard data if orders not available
      console.log('Using Dashboard Data for Order Chart:', {
        TotalOrders: this.dashboardData.TotalOrders,
        PendingOrders: this.dashboardData.PendingOrders
      });
      
      const pendingOrders = this.dashboardData.PendingOrders || 0;
      const totalOrders = this.dashboardData.TotalOrders || 0;
      const completedOrders = Math.max(0, totalOrders - pendingOrders);
      
      this.ordersChartData.datasets[0].data = [
        pendingOrders,
        completedOrders
      ];
      
      this.ordersChartData.labels = [
        `Pending (${pendingOrders})`,
        `Completed (${completedOrders})`
      ];
    } else {
      // Fallback data if no data available
      this.ordersChartData.datasets[0].data = [2, 3];
      this.ordersChartData.labels = ['Pending (2)', 'Completed (3)'];
    }
  }

  updateProductsChart(): void {
    const topProducts = this.products
      .sort((a, b) => (b.TotalSales || 0) - (a.TotalSales || 0))
      .slice(0, 5);

    if (topProducts.length > 0) {
      this.productsChartData.labels = topProducts.map(product => product.ProductName);
      this.productsChartData.datasets[0].data = topProducts.map(product => product.TotalSales || 0);
    } else {
      // Fallback data if no products available
      this.productsChartData.labels = ['Product A', 'Product B', 'Product C'];
      this.productsChartData.datasets[0].data = [5000, 3000, 2000];
    }
  }

  updateCustomersChart(): void {
    const topCustomers = this.customers
      .sort((a, b) => (b.OrderCount || 0) - (a.OrderCount || 0))
      .slice(0, 5);

    if (topCustomers.length > 0) {
      this.customersChartData.labels = topCustomers.map(customer => customer.CompanyName);
      this.customersChartData.datasets[0].data = topCustomers.map(customer => customer.OrderCount || 0);
    } else {
      // Fallback data if no customers available
      this.customersChartData.labels = ['Customer A', 'Customer B', 'Customer C'];
      this.customersChartData.datasets[0].data = [10, 8, 6];
    }
  }

  updateEmployeesChart(): void {
    const topEmployees = this.employees
      .sort((a, b) => (b.TotalSales || 0) - (a.TotalSales || 0))
      .slice(0, 5);

    if (topEmployees.length > 0) {
      this.employeesChartData.labels = topEmployees.map(emp => `${emp.FirstName} ${emp.LastName}`);
      this.employeesChartData.datasets[0].data = topEmployees.map(emp => emp.OrderCount || 0);
      this.employeesChartData.datasets[1].data = topEmployees.map(emp => emp.TotalSales || 0);
    } else {
      // Fallback data if no employees available
      this.employeesChartData.labels = ['John Doe', 'Jane Smith', 'Bob Johnson'];
      this.employeesChartData.datasets[0].data = [15, 12, 10];
      this.employeesChartData.datasets[1].data = [25000, 20000, 15000];
    }
  }

  updateCountryOrdersChart(): void {
    if (this.countryOrders.length > 0) {
      this.countryOrdersChartData.labels = this.countryOrders.map(co => co.Country);
      this.countryOrdersChartData.datasets[0].data = this.countryOrders.map(co => co.OrderCount);
    } else {
      // Fallback data if no country orders available
      this.countryOrdersChartData.labels = ['Germany', 'USA', 'Brazil', 'UK', 'Austria'];
      this.countryOrdersChartData.datasets[0].data = [122, 122, 83, 77, 56];
    }
  }

  updateCategorySalesChart(): void {
    if (this.categorySales.length > 0) {
      this.categorySalesChartData.labels = this.categorySales.map(cs => cs.CategoryName);
      this.categorySalesChartData.datasets[0].data = this.categorySales.map(cs => cs.TotalSales);
    } else {
      // Fallback data if no category sales available
      this.categorySalesChartData.labels = ['Beverages', 'Dairy Products', 'Meat/Poultry', 'Confections', 'Condiments'];
      this.categorySalesChartData.datasets[0].data = [286527, 251331, 178189, 177099, 141623];
    }
  }

  updateCountrySalesChart(): void {
    if (this.countrySales.length > 0) {
      this.countrySalesChartData.labels = this.countrySales.map(cs => cs.Country);
      this.countrySalesChartData.datasets[0].data = this.countrySales.map(cs => cs.TotalSales);
    } else {
      this.countrySalesChartData.labels = ['No Data'];
      this.countrySalesChartData.datasets[0].data = [0];
    }
  }

  updateCustomerPurchasesChart(): void {
    if (this.customerPurchases.length > 0) {
      this.customerPurchasesChartData.labels = this.customerPurchases.map(cp => cp.CompanyName);
      this.customerPurchasesChartData.datasets[0].data = this.customerPurchases.map(cp => cp.TotalPurchases);
    } else {
      this.customerPurchasesChartData.labels = ['No Data'];
      this.customerPurchasesChartData.datasets[0].data = [0];
    }
  }

  updateEmployeeSalesChart(): void {
    if (this.employeeSales.length > 0) {
      this.employeeSalesChartData.labels = this.employeeSales.map(es => es.EmployeeName);
      this.employeeSalesChartData.datasets[0].data = this.employeeSales.map(es => es.TotalSales);
    } else {
      this.employeeSalesChartData.labels = ['No Data'];
      this.employeeSalesChartData.datasets[0].data = [0];
    }
  }

  updateCustomerCountriesChart(): void {
    if (this.customerCountries.length > 0) {
      this.customerCountriesChartData.labels = this.customerCountries.map(cc => cc.Country);
      this.customerCountriesChartData.datasets[0].data = this.customerCountries.map(cc => cc.CustomerCount);
    } else {
      this.customerCountriesChartData.labels = ['No Data'];
      this.customerCountriesChartData.datasets[0].data = [0];
    }
  }

  updateProductCategoriesChart(): void {
    if (this.productCategories.length > 0) {
      this.productCategoriesChartData.labels = this.productCategories.map(pc => pc.CategoryName);
      this.productCategoriesChartData.datasets[0].data = this.productCategories.map(pc => pc.ProductCount);
    } else {
      this.productCategoriesChartData.labels = ['No Data'];
      this.productCategoriesChartData.datasets[0].data = [0];
    }
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending':
        return '#ff9800';
      case 'completed':
        return '#4caf50';
      case 'cancelled':
        return '#f44336';
      default:
        return '#9e9e9e';
    }
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  }

  getTotalRevenue(): number {
    return this.products.reduce((sum, product) => sum + (product.TotalSales || 0), 0) +
           this.employees.reduce((sum, employee) => sum + (employee.TotalSales || 0), 0);
  }

  getTotalOrders(): number {
    return this.customers.reduce((sum, customer) => sum + (customer.OrderCount || 0), 0) +
           this.employees.reduce((sum, employee) => sum + (employee.OrderCount || 0), 0);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString();
  }
}