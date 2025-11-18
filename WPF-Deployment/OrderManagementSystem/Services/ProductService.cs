using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllWithIncludes(p => p.Categories, p => p.Suppliers, p => p.Order_Details)
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>()
                .GetByWithIncludes(p => p.ProductID == id, p => p.Categories, p => p.Suppliers, p => p.Order_Details)
                .FirstOrDefaultAsync();

            if (product == null)
                return null;

            var productDto = _mapper.Map<ProductDto>(product);
            productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
            productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
            productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
            productDto.OrderCount = product.Order_Details.Count;

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetByWithIncludes(p => p.CategoryID == categoryId, p => p.Categories, p => p.Suppliers, p => p.Order_Details)
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierAsync(int supplierId)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetByWithIncludes(p => p.SupplierID == supplierId, p => p.Categories, p => p.Suppliers, p => p.Order_Details)
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetByWithIncludes(
                    p => p.ProductName.Contains(searchTerm) || 
                         p.QuantityPerUnit.Contains(searchTerm),
                    p => p.Categories, p => p.Suppliers, p => p.Order_Details
                )
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetDiscontinuedProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetByWithIncludes(p => p.Discontinued, p => p.Categories, p => p.Suppliers, p => p.Order_Details)
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsWithLowStockAsync(int threshold)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetByWithIncludes(
                    p => p.UnitsInStock <= threshold && !p.Discontinued,
                    p => p.Categories, p => p.Suppliers, p => p.Order_Details
                )
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetTopProductsAsync(int count)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllWithIncludes(p => p.Categories, p => p.Suppliers, p => p.Order_Details)
                .ToListAsync();

            var productDtos = new List<ProductDto>();

            foreach (var product in products)
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.CategoryName = product.Categories?.CategoryName ?? string.Empty;
                productDto.SupplierName = product.Suppliers?.CompanyName ?? string.Empty;
                productDto.TotalSales = product.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount));
                productDto.OrderCount = product.Order_Details.Count;
                productDtos.Add(productDto);
            }

            return productDtos.OrderByDescending(p => p.TotalSales).Take(count);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(createProductDto.ProductName))
            {
                throw new InvalidOperationException("Product name is required.");
            }

            if (createProductDto.UnitPrice <= 0)
            {
                throw new InvalidOperationException("Unit price must be greater than zero.");
            }

            // Validate that category exists
            if (createProductDto.CategoryID <= 0)
            {
                throw new InvalidOperationException("Category ID is required and must be greater than zero.");
            }

            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(createProductDto.CategoryID);
            if (category == null)
            {
                throw new InvalidOperationException($"Category with ID '{createProductDto.CategoryID}' not found.");
            }

            // Validate that supplier exists
            if (createProductDto.SupplierID <= 0)
            {
                throw new InvalidOperationException("Supplier ID is required and must be greater than zero.");
            }

            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(createProductDto.SupplierID);
            if (supplier == null)
            {
                throw new InvalidOperationException($"Supplier with ID '{createProductDto.SupplierID}' not found.");
            }

            // Check if product with same name already exists
            var existingProduct = await _unitOfWork.Repository<Product>()
                .GetBy(p => p.ProductName.ToLower() == createProductDto.ProductName.ToLower())
                .FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Product with name '{createProductDto.ProductName}' already exists.");
            }

            var product = _mapper.Map<Product>(createProductDto);
            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
                throw new ArgumentException("Product not found");

            _mapper.Map(updateProductDto, product);
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (product == null)
                return false;

            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ProductStatisticsDto> GetProductStatisticsAsync()
        {
            var totalProducts = await _unitOfWork.Repository<Product>().CountAsync();
            var discontinuedProducts = await _unitOfWork.Repository<Product>().CountAsync(p => p.Discontinued);
            var lowStockProducts = await _unitOfWork.Repository<Product>().CountAsync(p => p.UnitsInStock <= 10 && !p.Discontinued);
            
            var totalInventoryValue = await _unitOfWork.Repository<Product>()
                .GetAll()
                .SumAsync(p => (p.UnitPrice ?? 0) * (p.UnitsInStock ?? 0));

            var averageUnitPrice = await _unitOfWork.Repository<Product>()
                .GetAll()
                .AverageAsync(p => p.UnitPrice ?? 0);

            return new ProductStatisticsDto
            {
                TotalProducts = totalProducts,
                DiscontinuedProducts = discontinuedProducts,
                LowStockProducts = lowStockProducts,
                TotalInventoryValue = totalInventoryValue,
                AverageUnitPrice = averageUnitPrice
            };
        }
    }
} 