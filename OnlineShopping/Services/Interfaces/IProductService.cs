using OnlineShopping.Models.Domain;

namespace OnlineShopping.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> SearchAsync(string title);
        Task<Product?> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
