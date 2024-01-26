using System.ComponentModel.DataAnnotations;

namespace Shop_Express.Models
{
    public class Role
    {
        public Guid  RoleId { get; set; }
        [RegularExpression(@"^[a-zA-Z]{3,20}$",
        ErrorMessage = "Name must be 3 to 20 characters long and contain only Latin letters.")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public Role()
        {
            Name = "Reader";
        }
        public Role(string name)
        {
            Name = name;
        }
    }
}
