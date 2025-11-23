using OnlineShopping.Models.Domain;

namespace OnlineShopping.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> LoginAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);

        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
