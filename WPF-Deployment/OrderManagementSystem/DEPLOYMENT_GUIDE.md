# Order Management System - Deployment Guide

## Current Issues and Solutions

### 1. **Frontend Environment Configuration** ✅ FIXED
**Issue**: Production environment still points to localhost
**Solution**: Update `oms-angular/src/environments/environment.prod.ts` with your deployed backend URL

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-deployed-backend-url.com/api' // Replace with actual URL
};
```

### 2. **Database Connection Issues** ✅ ENHANCED
**Issue**: 500 Internal Server Error due to database connection problems
**Solutions Applied**:
- Enhanced error logging in `Program.cs`
- Improved connection string validation
- Better error handling in services
- Graceful degradation when database is unavailable

### 3. **Database Schema Issues** ✅ SCRIPT PROVIDED
**Issue**: Potential schema mismatch between local and production
**Solution**: Use the provided `scripts/deploy-database.sql` script

## Deployment Steps

### Step 1: Database Setup
1. Connect to your Azure SQL Database (or other SQL Server instance)
2. Run the `scripts/deploy-database.sql` script
3. Verify tables are created successfully

### Step 2: Backend Deployment
1. **Azure App Service**:
   - Deploy your .NET application
   - Set environment variables in App Service Configuration:
     ```
     ASPNETCORE_ENVIRONMENT=Production
     ConnectionStrings__DefaultConnection=<your-connection-string>
     ```
   - Enable Application Logging for debugging

2. **Other Hosting**:
   - Set environment variables for connection string
   - Ensure proper firewall rules for database access

### Step 3: Frontend Deployment
1. Update `environment.prod.ts` with correct backend URL
2. Build the Angular application:
   ```bash
   ng build --configuration production
   ```
3. Deploy the `dist` folder to your hosting service

### Step 4: Testing
1. Test the health endpoint: `https://your-backend-url/api/health`
2. Test database connectivity: `https://your-backend-url/api/health/database`
3. Verify frontend can connect to backend

## Troubleshooting

### Database Connection Issues
1. **Check Connection String**: Verify it's correctly set in environment variables
2. **Firewall Rules**: Ensure your hosting service can access the database
3. **Azure SQL**: Check if the server is paused (common in free tier)
4. **Network Connectivity**: Test connection from your hosting environment

### 500 Internal Server Error
1. **Check Application Logs**: Look for detailed error messages
2. **Database Schema**: Ensure tables exist and match the expected schema
3. **Dependencies**: Verify all required packages are installed
4. **Environment Variables**: Confirm all configuration is set correctly

### Frontend Issues
1. **CORS**: Ensure backend CORS policy allows your frontend domain
2. **API URL**: Verify the frontend is pointing to the correct backend URL
3. **Network**: Check if the frontend can reach the backend

## Security Considerations

### Connection String Security
- **NEVER** commit connection strings with passwords to source control
- Use environment variables or Azure Key Vault for production
- Consider using managed identity for Azure deployments

### CORS Configuration
- Restrict CORS to specific domains in production
- Avoid using `AllowAnyOrigin()` in production

## Monitoring and Logging

### Application Insights (Recommended for Azure)
1. Add Application Insights to your project
2. Monitor database connection failures
3. Set up alerts for 500 errors

### Custom Logging
- The application now includes detailed logging for database operations
- Check console logs for connection issues
- Monitor the health endpoints for system status

## Performance Optimization

### Database
1. Add appropriate indexes for frequently queried columns
2. Consider connection pooling settings
3. Monitor query performance

### Application
1. Enable response compression
2. Consider caching strategies
3. Optimize Entity Framework queries

## Rollback Plan

1. **Database**: Keep backups before schema changes
2. **Application**: Use deployment slots for zero-downtime deployments
3. **Configuration**: Document all environment variables and settings

## Support

If you continue to experience issues:
1. Check the application logs for detailed error messages
2. Test the health endpoints to isolate the problem
3. Verify network connectivity between services
4. Consider using Application Insights for detailed monitoring 