using CollegeApp.Models;

namespace CollegeApp.Services
{
    public interface IUserService
    {
        (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string passwordHash);
        Task<bool> CreateUserAsync(UserDTO model);
        Task<List<UserReadonlyDTO>> GetAllUsersAsync();
        Task<UserReadonlyDTO> GetUserByIdAsync(int id);
        Task<UserReadonlyDTO> GetUserByNameAsync(string username);
    }
}
