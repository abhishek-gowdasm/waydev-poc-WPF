# PowerShell deployment script for Azure
param(
    [string]$ResourceGroupName = "oms-angular-rg",
    [string]$StorageAccountName = "omsangularstorage",
    [string]$Location = "Central India"
)

Write-Host "Starting deployment to Azure..." -ForegroundColor Green

# Check if Azure CLI is installed
if (!(Get-Command az -ErrorAction SilentlyContinue)) {
    Write-Host "Azure CLI is not installed. Please install it first." -ForegroundColor Red
    exit 1
}

# Login to Azure
Write-Host "Logging in to Azure..." -ForegroundColor Yellow
az login

# Create resource group if it doesn't exist
Write-Host "Creating resource group..." -ForegroundColor Yellow
az group create --name $ResourceGroupName --location $Location

# Create storage account if it doesn't exist
Write-Host "Creating storage account..." -ForegroundColor Yellow
az storage account create --name $StorageAccountName --resource-group $ResourceGroupName --location $Location --sku Standard_LRS --kind StorageV2

# Enable static website hosting
Write-Host "Enabling static website hosting..." -ForegroundColor Yellow
az storage blob service-properties update --account-name $StorageAccountName --static-website --index-document index.html --404-document index.html

# Get the storage account key
$storageKey = az storage account keys list --account-name $StorageAccountName --resource-group $ResourceGroupName --query "[0].value" --output tsv

# Upload the built application
Write-Host "Uploading application files..." -ForegroundColor Yellow
az storage blob upload-batch --account-name $StorageAccountName --account-key $storageKey --source "dist/oms-angular/browser" --destination '$web'

# Get the website URL
$websiteUrl = az storage account show --name $StorageAccountName --resource-group $ResourceGroupName --query "primaryEndpoints.web" --output tsv

Write-Host "Deployment completed successfully!" -ForegroundColor Green
Write-Host "Your application is available at: $websiteUrl" -ForegroundColor Cyan 