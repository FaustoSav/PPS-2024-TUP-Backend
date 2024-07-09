using FornitureStore.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.Categories
{
    public class CategoryUpdateDto
    {

        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? ImageId { get; set; }

        [MaxLength(50)]
        public string? CategoryName { get; set; }

    }
}
