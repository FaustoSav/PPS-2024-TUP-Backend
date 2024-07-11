using FornitureStore.DBContext;
using FornitureStore.Models.Dtos.User;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.ExceptionServices;

namespace FornitureStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly FornitureStoreContext _fornitureStoreContext;


        public UserService(FornitureStoreContext fornitureStoreContext, ILogger<UserService> logger)
        {
            _fornitureStoreContext = fornitureStoreContext;
            _logger = logger;
        }

        public async Task<BaseResponse> ValidarUsuarioAsync(CredentialsDto credentials)
        {
            BaseResponse response = new BaseResponse();
            User? userForLogin = await _fornitureStoreContext.Users.SingleOrDefaultAsync(u => u.Email == credentials.Email);
            if (userForLogin != null)
            {
                // Verificar la contraseña hasheada
                if (BCrypt.Net.BCrypt.Verify(credentials.Password, userForLogin.Password))
                {
                    response.Result = true;
                    response.Message = "Inicio de sesión exitoso";
                }
                else
                {
                    response.Result = false;
                    response.Message = "Contraseña incorrecta";
                }
            }
            else
            {
                response.Result = false;
                response.Message = "Correo incorrecto";
            }
            return response;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = await _fornitureStoreContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _fornitureStoreContext.Users
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int idUser)
        {
            return await _fornitureStoreContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == idUser);
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            User? usertoDelete = await _fornitureStoreContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (usertoDelete == null)
            {
                _logger.LogWarning("Parece que el usuario que se desea eliminar no existe en la base de datos. Verifique los datos");
                return false;
            }

            try
            {
                _fornitureStoreContext.Users.Remove(usertoDelete);
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al tratar de eliminar el usuario con Id {Id}", userId);
                return false;
            }
        }
        public async Task<bool> UpdateUserAsync(UserUpdateDto user)
        {
            User? userToUpdate = await _fornitureStoreContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (userToUpdate == null)
            {
                _logger.LogWarning("El usuario con ID {Id} que desea actualizar no existe en la base de datos", user.Id);
                return false;
            }

            try
            {
                // Actualizar todas las propiedades
                userToUpdate.UserName = user.Username;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
              

                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al intentar actualizar el usuario con ID {Id}", user.Id);
                return false;
            }
        }


        public async Task<BaseResponse> CreateUserAsync(UserCreateDto userCreateDto)
        {
            BaseResponse response = new BaseResponse();

            // Hashear la contraseña
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password);

            User newUser = new User
            {
                UserName = userCreateDto.UserName,
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                Password = hashedPassword, // Usar la contraseña hasheada
                Email = userCreateDto.Email,
                Role = userCreateDto.Role.ToString()
            };

            response.Result = false;
            response.Message = "El correo ingresado ya existe";
            User? userForRegister = await _fornitureStoreContext.Users.SingleOrDefaultAsync(u => u.Email == userCreateDto.Email);

            if(userForRegister == null)
            {
                _fornitureStoreContext.Users.Add(newUser);
                await _fornitureStoreContext.SaveChangesAsync();
                response.Result = true;
                response.Message = "Usuario registrado exitosamente";
            }
            

            return response;
        }
    }
}
