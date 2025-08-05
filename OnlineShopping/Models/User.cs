using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class User
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is Invalid")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
        public string UserType { get; set; } // Admin or Customer
    }
}
