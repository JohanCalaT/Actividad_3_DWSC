using UniversidadAPI.Models;

namespace UniversidadAPI.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> InsertAsync(User user);
}
