using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.Orders
{
    public class OrderCreateDto
    {
        [Required]
        public int UserId { get; set; }
       

        [Required]
        public List<OrderDetailCreateDto> OrderDetails { get; set; }
    }
}
