using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Express.Data;
using Shop_Express.Models;
using Shop_Express.Service;

namespace Shop_Express
{
    public class SampleData
    {
        public static async Task InitializeAsync(JobbingContext context)
        {
            Role adminRole = new Role("Admin");
            Role readerRole = new Role("Reader");
            User reader1 = new User("r1@gmail.com", readerRole);
            User reader2 = new User("r2@gmail.com", readerRole);
            User admin = new User("a1@gmail.com", adminRole);

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                        adminRole,
                        readerRole
                    );
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    reader1,
                    reader2,
                    admin
                    );
            }
            if (!context.Jobs.Any())
            {
                context.Jobs.AddRange(
                    new Job("Task1", reader1),
                    new Job("Task2", reader1),
                    new Job("Task3", reader1),
                    new Job("Task4", reader2)
                    );
            }
            if(!context.UserPasswords.Any())
            {
                var password = 11_111_111.ToString();
                var userPassword1 = await CreateUserPasswordAsync(reader1, password);
                var userPassword2 = await CreateUserPasswordAsync(reader2, password);
                var userPassword3 = await CreateUserPasswordAsync(admin, password);

                context.UserPasswords.AddRange(userPassword1, userPassword2, userPassword3);
            }

            context.SaveChanges();
        }
        public static async Task<UserPassword> CreateUserPasswordAsync(User user, string password)
        {
            var userPassword = new UserPassword();
            var userPasswordService = new UserPasswordService();
            var (hash, salt) = await userPasswordService.HashPasswordAsync(password);

            userPassword.Hash = hash;
            userPassword.Salt = salt;
            userPassword.User = user;

            return userPassword;
        }
    }
}
