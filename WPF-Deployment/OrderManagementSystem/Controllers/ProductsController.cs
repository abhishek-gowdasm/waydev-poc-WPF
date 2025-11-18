using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of all products</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Get products by category
        /// </summary>
        /// <param name="categoryId">Category ID</param>
        /// <returns>Products in specified category</returns>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        /// <summary>
        /// Get products by supplier
        /// </summary>
        /// <param name="supplierId">Supplier ID</param>
        /// <returns>Products from specified supplier</returns>
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySupplier(int supplierId)
        {
            var products = await _productService.GetProductsBySupplierAsync(supplierId);
            return Ok(products);
        }

        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>Matching products</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string searchTerm)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        /// <summary>
        /// Get discontinued products
        /// </summary>
        /// <returns>Discontinued products</returns>
        [HttpGet("discontinued")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetDiscontinuedProducts()
        {
            var products = await _productService.GetDiscontinuedProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get products with low stock
        /// </summary>
        /// <param name="threshold">Stock threshold</param>
        /// <returns>Products with low stock</returns>
        [HttpGet("lowstock/{threshold}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsWithLowStock(int threshold)
        {
            var products = await _productService.GetProductsWithLowStockAsync(threshold);
            return Ok(products);
        }

        /// <summary>
        /// Get top products
        /// </summary>
        /// <param name="count">Number of products to return</param>
        /// <returns>Top products by sales</returns>
        [HttpGet("top/{count}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetTopProducts(int count)
        {
            var products = await _productService.GetTopProductsAsync(count);
            return Ok(products);
        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="createProductDto">Product data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
        {
            try
            {
                var product = await _productService.CreateProductAsync(createProductDto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { error = "An error occurred while creating the product. Please try again." });
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updateProductDto">Updated product data</param>
        /// <returns>Updated product</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(id, updateProductDto);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get product statistics
        /// </summary>
        /// <returns>Product statistics</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<ProductStatisticsDto>> GetProductStatistics()
        {
            var statistics = await _productService.GetProductStatisticsAsync();
            return Ok(statistics);
        }
    }
} 