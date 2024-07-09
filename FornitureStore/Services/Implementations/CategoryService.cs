using FornitureStore.DBContext;
using FornitureStore.Models.Dtos.Categories;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FornitureStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly FornitureStoreContext _fornitureStoreContext;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(FornitureStoreContext fornitureStoreContext, ILogger<CategoryService> logger)
        {
            _fornitureStoreContext = fornitureStoreContext;
            _logger = logger;
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _fornitureStoreContext.Categories.AsNoTracking().ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al obtener las categorias.");
                return Enumerable.Empty<Category>();
            }
        }
        public async Task<Category?> GetCategoryByIdAsync(int idCategory)
        {
            try
            {
                return await _fornitureStoreContext.Categories.FirstOrDefaultAsync(c => c.Id == idCategory);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Hubo un error al buscar una categoria por ID");
                return null;

            }
        }
        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            try
            {
                return await _fornitureStoreContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Hubo un error al buscar una categoria por Nombre");
                return null;

            }
        }
        public async Task<bool> UpdateCategoryAsync(CategoryUpdateDto category)
        {
            Category? categoryToUpdate = await _fornitureStoreContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (categoryToUpdate == null)
            {
                _logger.LogWarning("La categoria con ID {Id} que desea actualizar no existe en la base de datos", category.Id);
                return false;
            }

            try
            {

                categoryToUpdate.ImageId = category.ImageId ?? categoryToUpdate.ImageId;
                categoryToUpdate.CategoryName = category.CategoryName ?? categoryToUpdate.CategoryName;
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al intentar actualizar la categoria con ID {Id}", category.Id);
                return false;
            }
        }

        public async Task<bool> AddCategoryAsync(CategoryCreateDto category)
        {
            try
            {
                Category newCategory = new Category
                {
                    CategoryName = category.CategoryName,
                    ImageId = category.ImageId,
                };
                await _fornitureStoreContext.Categories.AddAsync(newCategory);
                await _fornitureStoreContext.SaveChangesAsync();

                return true;
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Hubo un error al tratar de crear una nueva categoria con nombre {category.CategoryName}");
                return false;
            }
        }

    }
}
