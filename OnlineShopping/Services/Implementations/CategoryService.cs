using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models.Domain;
using OnlineShopping.Services.Interfaces;

namespace OnlineShopping.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly OnlineShoppingContext _context;

        public CategoryService(OnlineShoppingContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Category.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Category.AnyAsync(e => e.Id == id);
        }
    }
}
