using Microsoft.EntityFrameworkCore;
using Vanilla.Models;

namespace Vanilla.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TableEntity> Tables => Set<TableEntity>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // ✅ deterministic seed for 8 mese
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

            // ✅ let DB set CreatedAt (constant SQL, not DateTime.Now)
#if POSTGRES
    mb.Entity<Reservation>()
      .Property(r => r.CreatedAt)
      .HasDefaultValueSql("timezone('utc', now())"); // Postgres
#else
            mb.Entity<Reservation>()
              .Property(r => r.CreatedAt)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");      // SQLite
#endif
        }
    }
}
