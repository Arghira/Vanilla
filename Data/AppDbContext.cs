using Microsoft.EntityFrameworkCore;
using Vanilla.Models;

namespace Vanilla.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DB sets
        public DbSet<TableEntity> Tables => Set<TableEntity>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Admin> Admins => Set<Admin>();        // <-- if you add Admins

        // deterministic seed ids (class-level, NOT DateTime.Now/Guid.NewGuid())
        private static readonly Guid SeedAdminId = new("2f0c5f46-2a62-4f2a-bc0f-91b0a9f7e5a1");

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // 8 tables — deterministic values
            mb.Entity<TableEntity>().HasData(
                new TableEntity { Id = 1, Name = "Masa 1" },
                new TableEntity { Id = 2, Name = "Masa 2" },
                new TableEntity { Id = 3, Name = "Masa 3" },
                new TableEntity { Id = 4, Name = "Masa 4" },
                new TableEntity { Id = 5, Name = "Masa 5" },
                new TableEntity { Id = 6, Name = "Masa 6" },
                new TableEntity { Id = 7, Name = "Masa 7" },
                new TableEntity { Id = 8, Name = "Masa 8" }
            );

            // Admin seed — deterministic Guid
            mb.Entity<Admin>().HasData(new Admin
            {
                Id = SeedAdminId,
                Name = "Admin",
                Email = "admin@example.com"
            });

            // DB default for CreatedAt (no DateTime.Now here)
            mb.Entity<Reservation>()
              .Property(r => r.CreatedAt)
              .HasDefaultValueSql("timezone('utc', now())"); // Postgres

            mb.Entity<Reservation>()
              .HasIndex(r => new { r.TableId, r.StartAt })
              .IsUnique();
        }
    }
}