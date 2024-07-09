using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.Orders
{
    public class OrderDetailCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
