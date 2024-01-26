using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Shop_Express.Service
{
    public class UserPasswordService
    {
        public async Task<(string Hash, string Salt)> HashPasswordAsync(string password)
        {
            byte[] saltBytes = GenerateSalt();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string hash = Convert.ToBase64String(hashBytes);
                string salt = Convert.ToBase64String(saltBytes);
                return (hash, salt);
            }
        }

        public async Task<IActionResult> VerifyPasswordAsync(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(enteredPassword);
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            byte[] combinedBytes = new byte[passwordBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, passwordBytes.Length, saltBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string enteredHash = Convert.ToBase64String(hashBytes);
                if (enteredHash == storedHash)
                    return new OkResult();
                else
                    return new BadRequestResult();
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
