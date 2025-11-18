# PowerShell script to run the Order Management System and seed completed orders
Write-Host "Starting Order Management System..." -ForegroundColor Green

# Change to the project directory
Set-Location -Path "OrderManagementSystem"

# Run the application
Write-Host "Running the application..." -ForegroundColor Yellow
dotnet run

Write-Host "Application started successfully!" -ForegroundColor Green
Write-Host "Visit http://localhost:5000 for the API documentation" -ForegroundColor Cyan
Write-Host "Or visit http://localhost:5000/swagger for Swagger UI" -ForegroundColor Cyan 