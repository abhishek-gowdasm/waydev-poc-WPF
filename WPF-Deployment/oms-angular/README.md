# Orders Management System - Angular Frontend

This is the Angular frontend for the Orders Management System, designed to replicate the UI from the original WPF application.

## Features

- **Dashboard Overview**: Comprehensive dashboard with charts and statistics
- **Orders Management**: View and manage orders with detailed analytics
- **Customer Analytics**: Customer distribution and purchase analysis
- **Employee Performance**: Sales performance by employee
- **Product Management**: Product categorization and inventory insights
- **Responsive Design**: Works on desktop, tablet, and mobile devices

## Prerequisites

- Node.js (version 18 or higher)
- npm (comes with Node.js)
- Angular CLI (will be installed automatically)
- .NET 8.0 (for the backend API)

## Installation

1. **Clone or navigate to the project directory**:
   ```bash
   cd OMS-V2/oms-angular
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start the development server**:
   ```bash
   ng serve
   ```

4. **Open your browser** and navigate to `http://localhost:4200`

## Backend API Setup

Before running the frontend, ensure the backend API is running:

1. **Navigate to the backend directory**:
   ```bash
   cd ../OrderManagementSystem
   ```

2. **Run the backend API**:
   ```bash
   dotnet run
   ```

3. **Verify the API is running** by visiting `http://localhost:5000` or `http://localhost:5001`

## API Configuration

The frontend is configured to connect to the backend API at `http://localhost:5000/api`. If your backend runs on a different port, update the `baseUrl` in `src/app/services/api.service.ts`.

## Project Structure

```
src/
├── app/
│   ├── dashboard/           # Dashboard component
│   │   ├── dashboard.ts     # Dashboard logic
│   │   ├── dashboard.html   # Dashboard template
│   │   └── dashboard.scss   # Dashboard styles
│   ├── models/              # TypeScript interfaces
│   │   └── dashboard.model.ts
│   ├── services/            # API services
│   │   └── api.service.ts
│   ├── app.ts              # Main app component
│   ├── app.config.ts       # App configuration
│   └── app.routes.ts       # Routing configuration
├── styles.scss             # Global styles
└── main.ts                # Application entry point
```

## Available Endpoints

The application connects to the following backend endpoints:

### Dashboard
- `GET /api/dashboard` - Get complete dashboard data
- `GET /api/dashboard/monthly-revenue/{year}` - Get monthly revenue
- `GET /api/dashboard/top-products/{count}` - Get top products
- `GET /api/dashboard/top-customers/{count}` - Get top customers
- `GET /api/dashboard/recent-orders/{count}` - Get recent orders

### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `GET /api/orders/customer/{customerId}` - Get orders by customer
- `GET /api/orders/employee/{employeeId}` - Get orders by employee
- `GET /api/orders/pending` - Get pending orders

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID

## Technologies Used

- **Angular 17** - Frontend framework
- **Angular Material** - UI component library
- **Chart.js** - Charting library
- **ng2-charts** - Angular wrapper for Chart.js
- **SCSS** - CSS preprocessor
- **TypeScript** - Programming language

## Development

### Building for Production

```bash
ng build --configuration production
```

### Running Tests

```bash
ng test
```

### Code Formatting

```bash
ng lint
```

## Troubleshooting

### CORS Issues
If you encounter CORS errors, ensure the backend has CORS properly configured. The backend should already be configured to allow all origins for development.

### Chart.js Issues
If charts don't render properly, ensure all Chart.js dependencies are installed:
```bash
npm install chart.js ng2-charts
```

### Material Design Issues
If Material Design components don't render properly, ensure Angular Material is properly installed:
```bash
ng add @angular/material
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is part of the Orders Management System and follows the same licensing terms as the main project.
