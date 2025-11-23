using System.Net.Http.Headers;

namespace OnlineShopping.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Product>? Products { get; set; }
    }
}
