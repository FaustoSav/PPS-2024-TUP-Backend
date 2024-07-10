using FornitureStore.Models.Dtos.User;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FornitureStore.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthenticateController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _config = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] CredentialsDto credentialsDto)
        {
            BaseResponse validarUsuarioResult = await _userService.ValidarUsuarioAsync(credentialsDto);

            if (validarUsuarioResult.Message == "Email incorrecto")
            {
                return BadRequest(validarUsuarioResult.Message);
            }
            else if (validarUsuarioResult.Message == "Contraseña incorrecta")
            {
                return Unauthorized();
            }

            if (validarUsuarioResult.Result)
            {
                User? user = await _userService.GetUserByEmailAsync(credentialsDto.Email);

                var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));
                var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

                var claimsForToken = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("firstName", user.FirstName),
                new Claim(ClaimTypes.Role, user.Role)
            };

                var jwtSecurityToken = new JwtSecurityToken(
                  _config["Authentication:Issuer"],
                  _config["Authentication:Audience"],
                  claimsForToken,
                  DateTime.UtcNow,
                  DateTime.UtcNow.AddHours(1),
                  credentials);

                string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return Ok(tokenToReturn);
            }

            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            var response = await _userService.CreateUserAsync(userCreateDto);
            if (response.Result)
            {
                return Ok(response.Message);
            }
            return BadRequest(response.Message);
        }
    }
}
