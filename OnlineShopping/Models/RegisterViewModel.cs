using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is Invalid")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string ConfirmPassword { get; set; }
    }
}
