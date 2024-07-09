using FornitureStore.Models.Dtos.Categories;
using FornitureStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FornitureStore.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

       
        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Categoria con ID {id} no encontrada.");
            }
            return Ok(category);
        }

     
        [HttpGet]
        [Route("categoryName")]
        public async Task<IActionResult> GetCategoryByName(string categoryName)
        {
            var category = await _categoryService.GetCategoryByNameAsync(categoryName);
            if (category == null)
            {
                return NotFound($"Categoria con nombre {categoryName} no encontrada.");
            }
            return Ok(category);
        }

     
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool categoryCreated = await _categoryService.AddCategoryAsync(categoryDto);
            if (!categoryCreated)
            {
                return StatusCode(500, "Hubo un error al crear la categoria. Intente nuevamente.");
            }

            return Ok($"Categoria {categoryDto.CategoryName} creada correctamente.");
        }

        
        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool categoryUpdated = await _categoryService.UpdateCategoryAsync(categoryDto);
            if (!categoryUpdated)
            {
                return StatusCode(500, "Hubo un error al actualizar la categoria. Intente nuevamente.");
            }

            return Ok($"Categoria con ID {categoryDto.Id} actualizada correctamente.");
        }
    }
}
