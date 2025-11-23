using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models.Domain;
using OnlineShopping.Services.Interfaces;

namespace OnlineShopping.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly OnlineShoppingContext _context;

        public UserService(OnlineShoppingContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.User.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.User.AnyAsync(u => u.UserName == username);
        }

        public async Task CreateAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.User.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.User.AnyAsync(e => e.Id == id);
        }
    }
}
