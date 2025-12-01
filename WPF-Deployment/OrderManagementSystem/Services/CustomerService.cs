using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            try
            {
                Console.WriteLine("CustomerService: Starting GetAllCustomersAsync");
                
                var customers = await _unitOfWork.Repository<Customer>()
                    .GetAllWithIncludes(c => c.Orders)
                    .ToListAsync();

                Console.WriteLine($"CustomerService: Retrieved {customers.Count} customers from database");

                var customerDtos = new List<CustomerDto>();

                foreach (var customer in customers)
                {
                    try
                    {
                        var customerDto = _mapper.Map<CustomerDto>(customer);
                        customerDto.OrderCount = customer.Orders?.Count ?? 0;
                        customerDto.TotalSpent = customer.Orders?.Sum(o => o.Order_Details?.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)) ?? 0) ?? 0;
                        customerDtos.Add(customerDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"CustomerService: Error mapping customer {customer.CustomerID}: {ex.Message}");
                        // Continue with other customers instead of failing completely
                    }
                }

                Console.WriteLine($"CustomerService: Successfully mapped {customerDtos.Count} customers");
                return customerDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CustomerService: Critical error in GetAllCustomersAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Return empty list instead of throwing to prevent 500 errors
                return new List<CustomerDto>();
            }
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(string id)
        {
            var customer = await _unitOfWork.Repository<Customer>()
                .GetByWithIncludes(c => c.CustomerID == id, c => c.Orders)
                .FirstOrDefaultAsync();

            if (customer == null)
                return null;

            var customerDto = _mapper.Map<CustomerDto>(customer);
            customerDto.OrderCount = customer.Orders.Count;
            customerDto.TotalSpent = customer.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));

            return customerDto;
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm)
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetByWithIncludes(
                    c => c.CompanyName.Contains(searchTerm) || 
                         c.ContactName.Contains(searchTerm) || 
                         c.City.Contains(searchTerm) || 
                         c.Country.Contains(searchTerm),
                    c => c.Orders
                )
                .ToListAsync();

            var customerDtos = new List<CustomerDto>();

            foreach (var customer in customers)
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);
                customerDto.OrderCount = customer.Orders.Count;
                customerDto.TotalSpent = customer.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                customerDtos.Add(customerDto);
            }

            return customerDtos;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country)
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetByWithIncludes(c => c.Country == country, c => c.Orders)
                .ToListAsync();

            var customerDtos = new List<CustomerDto>();

            foreach (var customer in customers)
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);
                customerDto.OrderCount = customer.Orders.Count;
                customerDto.TotalSpent = customer.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                customerDtos.Add(customerDto);
            }

            return customerDtos;
        }

        public async Task<IEnumerable<CustomerDto>> GetTopCustomersAsync(int count)
        {
            var customers = await _unitOfWork.Repository<Customer>()
                .GetAllWithIncludes(c => c.Orders)
                .ToListAsync();

            var customerDtos = new List<CustomerDto>();

            foreach (var customer in customers)
            {
                var customerDto = _mapper.Map<CustomerDto>(customer);
                customerDto.OrderCount = customer.Orders.Count;
                customerDto.TotalSpent = customer.Orders.Sum(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));
                customerDtos.Add(customerDto);
            }

            return customerDtos.OrderByDescending(c => c.TotalSpent).Take(count);
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            try
            {
                Console.WriteLine($"Creating customer with ID: {createCustomerDto.CustomerID}");
                
                // Validate CustomerID length
                if (createCustomerDto.CustomerID.Length > 5)
                {
                    throw new InvalidOperationException($"CustomerID '{createCustomerDto.CustomerID}' is too long. CustomerID must be 5 characters or less.");
                }
                
                // Check if customer with the same CustomerID already exists
                var existingCustomer = await _unitOfWork.Repository<Customer>().GetByIdAsync(createCustomerDto.CustomerID);
                if (existingCustomer != null)
                {
                    throw new InvalidOperationException($"Customer with ID '{createCustomerDto.CustomerID}' already exists. Use PUT method to update existing customer.");
                }

                Console.WriteLine("Customer ID is unique, proceeding with creation...");

                var customer = _mapper.Map<Customer>(createCustomerDto);
                Console.WriteLine($"Mapped customer: {customer.CustomerID} - {customer.CompanyName}");
                
                await _unitOfWork.Repository<Customer>().AddAsync(customer);
                Console.WriteLine("Customer added to repository");
                
                var result = await _unitOfWork.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync returned: {result} affected rows");

                var createdCustomer = _mapper.Map<CustomerDto>(customer);
                Console.WriteLine($"Created customer DTO: {createdCustomer.CustomerID} - {createdCustomer.CompanyName}");
                
                return createdCustomer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateCustomerAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<CustomerDto> UpdateCustomerAsync(string id, UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);
            if (customer == null)
                throw new ArgumentException("Customer not found");

            _mapper.Map(updateCustomerDto, customer);
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);
            if (customer == null)
                return false;

            _unitOfWork.Repository<Customer>().Delete(customer);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerStatisticsDto> GetCustomerStatisticsAsync()
        {
            var totalCustomers = await _unitOfWork.Repository<Customer>().CountAsync();
            var activeCustomers = await _unitOfWork.Repository<Customer>()
                .GetAllWithIncludes(c => c.Orders)
                .CountAsync(c => c.Orders.Any());

            var newCustomersThisMonth = await _unitOfWork.Repository<Customer>()
                .CountAsync(c => c.Orders.Any(o => o.OrderDate >= DateTime.Now.AddMonths(-1)));

            var totalRevenue = await _unitOfWork.Repository<Order>()
                .GetAll()
                .SumAsync(o => o.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)));

            var averageOrderValue = totalRevenue / await _unitOfWork.Repository<Order>().CountAsync();

            var topCountry = await _unitOfWork.Repository<Customer>()
                .GetAll()
                .GroupBy(c => c.Country)
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync();

            return new CustomerStatisticsDto
            {
                TotalCustomers = totalCustomers,
                ActiveCustomers = activeCustomers,
                NewCustomersThisMonth = newCustomersThisMonth,
                TotalRevenue = totalRevenue,
                AverageOrderValue = averageOrderValue,
                TopCountry = topCountry?.Country ?? string.Empty,
                TopCountryCount = topCountry?.Count ?? 0
            };
        }

        //public async Task<bool> DeleteCustomerAsync(string id)
        //{
        //    var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);
        //    if (customer == null)
        //        return false;

        //    _unitOfWork.Repository<Customer>().Delete(customer);
        //    await _unitOfWork.SaveChangesAsync();
        //    return true;
        //}
    }
} 