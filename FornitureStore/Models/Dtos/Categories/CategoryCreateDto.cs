using FornitureStore.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.Categories
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string ImageId { get; set; } 

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; } 
       
    }
}
