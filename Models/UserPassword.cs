namespace Shop_Express.Models
{
    public class UserPassword
    {
        public Guid Id { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public UserPassword()
        {

        }
        public UserPassword(string hash, string salt, User user)
        {
            Hash = hash;
            Salt = salt;
            User = user;
        }

    }
}
