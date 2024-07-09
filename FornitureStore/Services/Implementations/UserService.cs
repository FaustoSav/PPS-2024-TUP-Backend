using FornitureStore.DBContext;
using FornitureStore.Models.Dtos.User;
using FornitureStore.Models.Entities;
using FornitureStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                if (userForLogin.Password == credentials.Password)
                {
                    response.Result = true;
                    response.Message = "Inicio de sesion exitoso";
                }
                else
                {
                    response.Result = false;
                    response.Message = "Contraseña Incorrecta";
                }
            }
            else
            {
                response.Result = false;
                response.Message = "Correo Incorrecto";
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
        public async Task<bool> AddUserAsync(UserCreateDto newUser)
        {
            bool userExist = await _fornitureStoreContext.Users
                .AnyAsync(u => u.Email == newUser.Email);

            if (userExist)
            {
                _logger.LogWarning("El usuario con email {Email} ya existe en la base de datos", newUser.Email);
                return false;
            }

            try
            {
                User userToAdd = new User
                {
                    UserName = newUser.UserName,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Password = newUser.Password,
                    Email = newUser.Email,
                    Role = newUser.Role.ToString()
                };

                await _fornitureStoreContext.Users.AddAsync(userToAdd);
                await _fornitureStoreContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un problema al agregar el usuario con email {Email} a la base de datos", newUser.Email);
                return false;
            }
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
    }
}
