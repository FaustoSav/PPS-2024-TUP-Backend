using FornitureStore.DBContext;
using FornitureStore.Models.Dtos.Products;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FornitureStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly FornitureStoreContext _fornitureStoreContext;
        private readonly ILogger<ProductService> _logger;
        public ProductService(FornitureStoreContext fornitureStoreContext, ILogger<ProductService> logger)
        {
            _fornitureStoreContext = fornitureStoreContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _fornitureStoreContext.Products.AsNoTracking()
                .OrderBy(p => p.Available)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _fornitureStoreContext.Products
                .Where(p => p.CategoryId == categoryId)
                .AsNoTracking()
                .OrderByDescending(p => p.Available)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int idProduct)
        {
            return await _fornitureStoreContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == idProduct);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string productName)
        {
            
           return  await _fornitureStoreContext.Products
                .AsNoTracking()
                .Where(p => p.ProductName.Contains(productName))
                .ToListAsync();
        }

        public async Task<bool> AddProductAsync(ProductCreateDto product)
        {
            try
            {
                bool productExist = await _fornitureStoreContext.Products.AnyAsync(p => p.ProductName == product.ProductName || p.ImageId == product.ImageId);
                if (productExist)
                {
                    _logger.LogWarning("No se puede crear. El producto con nombre {ProductName} y Id de imagen {ImageId} ya existe en la base de datos", product.ProductName, product.ImageId);
                    return false;
                }

                Product newProduct = new Product
                {
                    ImageId = product.ImageId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Available = product.Available,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                };

                await _fornitureStoreContext.Products.AddAsync(newProduct);
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar el producto {ProductName}", product.ProductName);
                return false;
            }
        }
        public async Task<bool> UpdateProductAsync(ProductUpdateDto product)
        {
            try
            {
                var existingProduct = await _fornitureStoreContext.Products.FindAsync(product.Id);

                if (existingProduct == null)
                {
                    _logger.LogWarning("El producto con ID {ProductId} no fue encontrado", product.Id);
                    return false;
                }

                // Validar que no exista otro producto con el mismo ImageId y ProductName
                if (!string.IsNullOrEmpty(product.ImageId) && !string.IsNullOrEmpty(product.ProductName))
                {
                    var duplicateProduct = await _fornitureStoreContext.Products
                        .AnyAsync(p => p.Id != product.Id &&
                                       p.ImageId == product.ImageId &&
                                       p.ProductName == product.ProductName);

                    if (duplicateProduct)
                    {
                        _logger.LogWarning("Ya existe otro producto con el mismo ImageId {ImageId} y ProductName {ProductName}",
                            product.ImageId, product.ProductName);
                        return false;
                    }
                }

                existingProduct.ImageId = product.ImageId ?? existingProduct.ImageId;
                existingProduct.Available = product.Available ?? existingProduct.Available;
                existingProduct.ProductName = product.ProductName ?? existingProduct.ProductName;
                existingProduct.ProductDescription = product.ProductDescription ?? existingProduct.ProductDescription;
                existingProduct.Price = product.Price ?? existingProduct.Price;
                existingProduct.CategoryId = product.CategoryId ?? existingProduct.CategoryId;

                _fornitureStoreContext.Products.Update(existingProduct);
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al actualizar el producto con ID {ProductId}", product.Id);
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var product = await _fornitureStoreContext.Products.FindAsync(productId);

                if (product == null)
                {
                    _logger.LogWarning("El producto con ID {ProductId} no fue encontrado", productId);
                    return false;
                }

                _fornitureStoreContext.Products.Remove(product);
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al eliminar el producto con ID {ProductId}", productId);
                return false;
            }
        }

        public async Task<bool> ProductExistsAsync(string productName)
        {
            return await _fornitureStoreContext.Products
                .AnyAsync(p => p.ProductName == productName);
        }
    }
}
