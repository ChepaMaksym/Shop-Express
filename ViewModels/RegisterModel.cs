using System.ComponentModel.DataAnnotations;

namespace Shop_Express.ViewModels
{
    public class RegisterModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$",
        ErrorMessage = "Password must be 8 to 20 characters long and contain only Latin letters and numbers.")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$",
        ErrorMessage = "Password must be 8 to 20 characters long and contain only Latin letters and numbers.")]
        [Compare("Password", ErrorMessage = "Password entered incorrectly.")]
        [Required(ErrorMessage = "Password is required.")]
        public string ConfirmPassword { get; set; }
        public RegisterModel()
        {
                
        }
        public RegisterModel(string email, string password, string confirmPassword)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
