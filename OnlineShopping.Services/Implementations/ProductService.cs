using Microsoft.EntityFrameworkCore;
using OnlineShopping.Infrastructure.Data;
using OnlineShopping.Core.Entities;
using OnlineShopping.Services.Interfaces;

namespace OnlineShopping.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly OnlineShoppingContext _context;

        public ProductService(OnlineShoppingContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> SearchAsync(string title)
        {
            return await _context.Product.Include(p => p.Category).Where(p => p.Title.Contains(title)).ToListAsync();
        }

        public async Task CreateAsync(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Product.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Product.AnyAsync(e => e.Id == id);
        }
    }
}
