using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Models.Domain;

namespace OnlineShopping.Data
{
    public class OnlineShoppingContext : DbContext
    {
        public OnlineShoppingContext (DbContextOptions<OnlineShoppingContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
    }
}
