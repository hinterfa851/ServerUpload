using Microsoft.EntityFrameworkCore;
using ServerUpload.DAL.Entities;
using Version = ServerUpload.DAL.Entities.Version;

namespace ServerUpload.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Version> Versions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>()
              .HasMany<Version>(g => g.Versions)
              .WithOne(s => s.Material)
              .HasForeignKey(s => s.MaterialId)
              .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Version>()
                .Property(p => p.StrHash)
                .IsRequired();
            modelBuilder.Entity<Version>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Material>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Material>()
                .Property(p => p.Category)
                .IsRequired();
        }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

    }
}
