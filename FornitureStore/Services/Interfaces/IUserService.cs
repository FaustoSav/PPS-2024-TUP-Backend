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
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(UserUpdateDto user);
        Task<BaseResponse> CreateUserAsync(UserCreateDto userCreateDto);
    }
}
