using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FornitureStore.Models.Enums;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Xml.Linq;

namespace FornitureStore.Models.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(70)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        private Role role = Enums.Role.Client;

        public string Role {
            get => role.ToString(); 
            set
            {
                if (Enum.TryParse(value, true, out Role parsedGender))
                {
                    role = parsedGender;
                }
                else
                {
                    throw new ArgumentException($"Rol incorrecto: {value}, verificar que sea Admin, Client o Guest");
                }
            }
        }

        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}
