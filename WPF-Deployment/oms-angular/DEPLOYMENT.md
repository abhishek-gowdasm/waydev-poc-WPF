# Deployment Guide

This guide provides instructions for deploying the Order Management System Angular application to Azure.

## Prerequisites

1. **Azure CLI** - Install from [https://docs.microsoft.com/en-us/cli/azure/install-azure-cli](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
2. **Node.js** - Version 18 or higher
3. **Git** - For version control

## Backend API Configuration

The application is configured to use the Azure-hosted backend API:
- **API URL**: `https://ordermanagementsystem20250812092938-drg6gmcebec5fvbz.centralindia-01.azurewebsites.net/api`
- **Configuration**: Located in `src/app/services/api.service.ts`

## Deployment Options

### Option 1: Azure Static Web Apps (Recommended)

1. **Create Azure Static Web App**:
   ```bash
   az staticwebapp create --name oms-angular-app --resource-group oms-angular-rg --location "Central India"
   ```

2. **Deploy using GitHub Actions**:
   - Push your code to a GitHub repository
   - Add the `AZURE_STATIC_WEB_APPS_API_TOKEN` secret to your GitHub repository
   - The GitHub Actions workflow will automatically deploy on push to main branch

3. **Manual deployment**:
   ```bash
   az staticwebapp deploy --name oms-angular-app --resource-group oms-angular-rg --source-path dist/oms-angular/browser
   ```

### Option 2: Azure Storage Account (Static Website)

1. **Run the deployment script**:
   ```powershell
   .\deploy.ps1
   ```

2. **Or manually**:
   ```bash
   # Create resource group
   az group create --name oms-angular-rg --location "Central India"
   
   # Create storage account
   az storage account create --name omsangularstorage --resource-group oms-angular-rg --location "Central India" --sku Standard_LRS --kind StorageV2
   
   # Enable static website hosting
   az storage blob service-properties update --account-name omsangularstorage --static-website --index-document index.html --404-document index.html
   
   # Upload files
   az storage blob upload-batch --account-name omsangularstorage --source dist/oms-angular/browser --destination '$web'
   ```

### Option 3: Azure App Service

1. **Create App Service**:
   ```bash
   az appservice plan create --name oms-angular-plan --resource-group oms-angular-rg --location "Central India" --sku B1
   az webapp create --name oms-angular-app --resource-group oms-angular-rg --plan oms-angular-plan --runtime "NODE|18-lts"
   ```

2. **Deploy**:
   ```bash
   az webapp deployment source config-zip --resource-group oms-angular-rg --name oms-angular-app --src dist/oms-angular.zip
   ```

## Local Development

1. **Install dependencies**:
   ```bash
   npm install
   ```

2. **Start development server**:
   ```bash
   npm start
   ```

3. **Build for production**:
   ```bash
   npm run build
   ```

## Environment Configuration

The application uses the following configuration:

- **API Base URL**: `https://ordermanagementsystem20250812092938-drg6gmcebec5fvbz.centralindia-01.azurewebsites.net/api`
- **Build Output**: `dist/oms-angular/browser/`
- **Framework**: Angular 20.1.0

## Troubleshooting

### CORS Issues
If you encounter CORS issues, ensure the backend API allows requests from your frontend domain.

### Build Errors
- Ensure Node.js version 18+ is installed
- Clear node_modules and reinstall: `rm -rf node_modules && npm install`

### Deployment Issues
- Check Azure CLI is logged in: `az login`
- Verify resource group and storage account names are unique
- Ensure you have sufficient Azure credits/permissions

## Security Considerations

- The application uses HTTPS for all API communications
- Content Security Policy is configured in `staticwebapp.config.json`
- Sensitive data is not stored in the frontend application

## Monitoring

After deployment, monitor your application using:
- Azure Application Insights (if configured)
- Azure Monitor
- Browser developer tools for client-side errors 