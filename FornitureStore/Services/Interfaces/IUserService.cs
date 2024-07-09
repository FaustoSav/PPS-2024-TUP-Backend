using FornitureStore.Models.Dtos.User;
using FornitureStore.Models.Entities;

namespace FornitureStore.Services.Interfaces
{
    public interface IUserService
    {
         Task<BaseResponse> ValidarUsuarioAsync(CredentialsDto credentials);
         Task<User?> GetUserByEmailAsync(string email);
         Task<IEnumerable<User>> GetAllUsersAsync();
         Task<User?> GetUserByIdAsync(int idUser);
        Task<bool> AddUserAsync(UserCreateDto newUser);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(UserUpdateDto user);
    }
}
