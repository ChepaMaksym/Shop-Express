using System.ComponentModel.DataAnnotations;

namespace Shop_Express.ViewModels
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$",
        ErrorMessage = "Password must be 8 to 20 characters long and contain only Latin letters and numbers.")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public LoginModel()
        {
        }
        public LoginModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
