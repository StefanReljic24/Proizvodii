using Microsoft.EntityFrameworkCore;
using Proizvodii.Entity;

namespace Proizvodii.Data
{
    public class EFDataContext : DbContext
    {
        public EFDataContext(DbContextOptions options)
           : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<Product>();

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(e => e.Category);


            modelBuilder.Entity<UserRole>()
                .HasKey(p => new { p.UserId, p.RoleId });


            modelBuilder.Entity<UserRole>()
               .HasOne(p => p.User)
               .WithMany(p => p.UserRole)
               .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(pt => pt.Role)
                .WithMany(t => t.UserRole)
                .HasForeignKey(pt => pt.RoleId);
        }
    }
}
