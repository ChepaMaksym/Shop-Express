using Shop_Express.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Shop_Express.Models
{
    public class Job
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool Completed { get; set; } = false;

        [RegularExpression(@"^[a-zA-Z0-9]{3,20}$",
        ErrorMessage = "Name must be 3 to 20 characters long and contain only Latin letters and numbers.")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public User User { get; set; }
        public Job()
        {
        }
        public Job(string name, User user)
        {
            Name = name;
            User = user;
        }
        public Job(JobModel jobModel)
        {
            Completed = jobModel.Job.Completed;
            Name = jobModel.Job.Name;
            User = jobModel.User;
        }
    }
}
