using FornitureStore.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.User
{
    public class UserCreateDto
    {

        [MaxLength(100)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        public Role Role = Role.Client;
    }
}
