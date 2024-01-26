using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Shop_Express.Models;

namespace Shop_Express.Data
{
    public class JobbingContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }

        public JobbingContext(DbContextOptions<JobbingContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.User)
                .WithMany(u => u.Job)
                .HasForeignKey(j => j.UserId);

            modelBuilder.Entity<UserPassword>()
                .HasOne(up => up.User)
                .WithOne(u => u.UserPassword)
                .HasForeignKey<UserPassword>(up => up.UserId);
        }
    }
}
