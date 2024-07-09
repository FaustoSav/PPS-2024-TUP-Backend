using FornitureStore.DBContext;
using FornitureStore.Models.Dtos.Products;
using FornitureStore.Models.Entities;

namespace FornitureStore.Services.Interfaces
{
    public interface IProductService
    {




        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Product?> GetProductByIdAsync(int idProduct);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string productName);
        Task<bool> AddProductAsync(ProductCreateDto product);
        Task<bool> UpdateProductAsync(ProductUpdateDto product);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> ProductExistsAsync(string productName);
    }
}
