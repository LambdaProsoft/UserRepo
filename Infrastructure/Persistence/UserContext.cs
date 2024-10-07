using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace UserInfrastructure.Persistence
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.DNI).IsRequired();
                entity.Property(u => u.Country).IsRequired();
                entity.Property(u => u.City).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.LastLogin).IsRequired();
                entity.Property(u => u.Address).IsRequired();
                entity.Property(u => u.BirthDate).IsRequired();
                entity.Property(u => u.Phone).IsRequired();

            });
        }
    }
}
