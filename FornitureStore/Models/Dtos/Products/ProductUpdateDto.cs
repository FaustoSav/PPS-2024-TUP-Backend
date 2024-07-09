using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.Products
{
    public class ProductUpdateDto
    {
        
            [Required]
            public int Id { get; set; }

            [MaxLength(50)]
            public string? ImageId { get; set; }

            public bool? Available { get; set; }

            [MaxLength(50)]
            public string? ProductName { get; set; }

            [MaxLength(500)]
            public string? ProductDescription { get; set; }

            public double? Price { get; set; }

            public int? CategoryId { get; set; }
        

    }
}
