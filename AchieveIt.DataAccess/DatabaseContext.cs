using AchieveIt.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity => {
                entity.ToTable("User");
            });
            
            modelBuilder.Entity<RefreshToken>(entity => {
                entity.ToTable("RefreshToken");
            });
            
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Student>(nameof(Student))
                .HasValue<Teacher>(nameof(Teacher))
                .HasValue<Admin>(nameof(Admin));
        }
    }
}