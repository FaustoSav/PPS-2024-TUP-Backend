using FornitureStore.Models.Dtos.Products;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FornitureStore.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("search/{name}")]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            try
            {
                var products = await _productService.GetProductsByNameAsync(name);
                if (products == null || !products.Any())
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar productos con el nombre {Name}.", name);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryIdAsync(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos de la categoría con ID {CategoryId}.", categoryId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool productCreated = await _productService.AddProductAsync(productDto);
                if (!productCreated)
                {
                    return BadRequest("No se pudo crear el producto. Verifique si ya existe uno con el mismo nombre o ImageId.");
                }

                return Ok($"Producto {productDto.ProductName} creado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool productUpdated = await _productService.UpdateProductAsync(productDto);
                if (!productUpdated)
                {
                    return BadRequest("No se pudo actualizar el producto. Verifique si ya existe otro con el mismo nombre o ImageId.");
                }

                return Ok($"Producto {productDto.ProductName} actualizado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool productDeleted = await _productService.DeleteProductAsync(id);
                if (!productDeleted)
                {
                    
                    return NotFound();
                }

                return Ok($"Product  ID {id} borrado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
