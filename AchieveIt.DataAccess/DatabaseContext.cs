using AchieveIt.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AchieveIt.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
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

            modelBuilder.Entity<PersonBase>(entity => {
                entity.Property(column => column.GroupId).HasColumnName("Group_id");
            });

            modelBuilder.Entity<Group>(entity => {
                entity.ToTable("Group");
            });
            
            modelBuilder.Entity<Subject>(entity => {
                entity.ToTable("Subject");
            });
            
            modelBuilder.Entity<TeacherGroup>()
                .HasOne<Group>()
                .WithMany(group => group.TeacherGroups);

            modelBuilder.Entity<TeacherGroup>(entity =>
            {
                entity.ToTable("TeacherGroups");
            });
            
            modelBuilder.Entity<TeacherGroup>()
                .HasOne(teacherGroup => teacherGroup.Teacher);

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