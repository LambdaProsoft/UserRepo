using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace UserInfrastructure.Persistence
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

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

                entity.HasMany(u => u.RefreshTokens)
                      .WithOne(rt => rt.User)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.VerificationCodes)
                      .WithOne(vc => vc.User)
                      .HasForeignKey(vc => vc.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.ExpirationDate).IsRequired();

                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.ToTable("VerificationCode");
                entity.HasKey(vc => vc.Id);
                entity.Property(vc => vc.Code).IsRequired();
                entity.Property(vc => vc.ExpirationDate).IsRequired();
                entity.Property(vc => vc.IsUsed).IsRequired().HasDefaultValue(false);

                entity.HasOne(vc => vc.User)
                      .WithMany(u => u.VerificationCodes)
                      .HasForeignKey(vc => vc.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}