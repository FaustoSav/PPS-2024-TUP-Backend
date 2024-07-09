using FornitureStore.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.User
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

       
    }
}
