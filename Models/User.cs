using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Shop_Express.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        public Role Role { get; set; } = new Role();
        public ICollection<Job>? Job { get; set; }
        public UserPassword UserPassword {get; set;} = new UserPassword();

        public User()
        {

        }
        public User(string email)
        {
            Email = email;
        }
        public User(string email, Role role) : this(email)
        {
            Role = role;
        }

    }
}
