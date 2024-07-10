using FornitureStore.Models.Dtos.User;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FornitureStore.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var allUsers = await _userService.GetAllUsersAsync();
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios.");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        [HttpGet]
        [Route("{email}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound($"No se pudo encontrar ningún usuario con el email {email}");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con email {email}.");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        [HttpGet]
        [Route("{userId:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"No se pudo encontrar ningún usuario con el id {userId}");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el usuario con id {userId}.");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

       

        [HttpDelete]
        [Route("{userId:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            try
            {
                bool userIsDeleted = await _userService.DeleteUserAsync(userId);
                if (!userIsDeleted)
                {
                    return NotFound($"No se pudo eliminar el usuario con Id {userId}");
                }

                return Ok("Usuario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Hubo un error al eliminar el usuario con id {userId}.");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        [HttpPut]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool userIsUpdated = await _userService.UpdateUserAsync(user);
                if (!userIsUpdated)
                {
                    return NotFound("No se pudo actualizar el usuario.");
                }

                return Ok("Usuario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un problema al actualizar el usuario.");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        [HttpGet]
        [Route("profile")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            UserGetDto userProfile = new UserGetDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "Not Asigned",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(userProfile);
        }

    }
}

