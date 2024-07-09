using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FornitureStore.Models.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ImageId { get; set; } = string.Empty;

        [Required]
        public bool Available { get; set; } = true;

        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string ProductDescription { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }
        [JsonIgnore]
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
