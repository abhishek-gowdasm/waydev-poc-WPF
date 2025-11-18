using OrderManagementSystem.DTOs;

namespace OrderManagementSystem.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> GetProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ProductDto>> GetDiscontinuedProductsAsync();
        Task<IEnumerable<ProductDto>> GetProductsWithLowStockAsync(int threshold);
        Task<IEnumerable<ProductDto>> GetTopProductsAsync(int count);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductStatisticsDto> GetProductStatisticsAsync();
    }
} 