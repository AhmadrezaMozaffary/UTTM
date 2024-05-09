using Microsoft.EntityFrameworkCore;
using UTTM.Models;

namespace UTTM.Context
{
    public class UttmDbContext : DbContext
    {
        public UttmDbContext(DbContextOptions<UttmDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasOne(e => e.Society).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasOne(e => e.Society).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
        }

        public DbSet<User> User { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<University> University { get; set; }
        public DbSet<Major> Major { get; set; }
        public DbSet<Society> Society { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Professor> Professor { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
