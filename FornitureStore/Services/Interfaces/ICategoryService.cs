using FornitureStore.Models.Dtos.Categories;
using FornitureStore.Models.Entities;

namespace FornitureStore.Services.Interfaces
{
    public interface ICategoryService
    {

        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int idCategory);
        Task<Category?> GetCategoryByNameAsync(string categoryName);
        Task<bool> UpdateCategoryAsync(CategoryUpdateDto category);
        Task<bool> AddCategoryAsync(CategoryCreateDto category); 
    }
}
