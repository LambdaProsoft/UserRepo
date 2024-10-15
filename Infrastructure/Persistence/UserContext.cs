using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace UserInfrastructure.Persistence
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
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
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id); // Clave primaria
                entity.Property(e => e.Token).IsRequired(); // Token es requerido
                entity.Property(e => e.ExpirationDate).IsRequired(); // ExpirationDate es requerido

                // Relación muchos-a-uno con la tabla Users
                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)  // Un usuario puede tener múltiples refresh tokens
                      .HasForeignKey(rt => rt.UserId)  // Clave foránea en RefreshToken que apunta a User
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra el usuario, también se eliminan los tokens


            });
        }
    }
}
