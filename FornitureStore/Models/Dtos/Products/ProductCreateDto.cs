using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.Products
{
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string ImageId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ProductDescription { get; set; } = string.Empty;

        [Required]
        public bool Available { get; set; } = true;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public double Price { get; set; }

    }

}
