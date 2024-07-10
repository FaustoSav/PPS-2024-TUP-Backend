using System.ComponentModel.DataAnnotations;

namespace FornitureStore.Models.Dtos.User
{
    public class CredentialsDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "savoyafausto@gmail.com";
        [Required]
        [MaxLength(70)]
        public string Password { get; set; } = "123456";
    }
}
