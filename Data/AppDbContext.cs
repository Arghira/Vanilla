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
            // 8 mese fixe
            mb.Entity<TableEntity>().HasData(
                Enumerable.Range(1, 8)
                    .Select(i => new TableEntity { Id = (short)i, Name = $"Masa {i}" })
            );

            // index unic: o rezervare per masă pe același start_at
            mb.Entity<Reservation>()
              .HasIndex(r => new { r.TableId, r.StartAt })
              .IsUnique();

            // default pentru CreatedAt (merge pe SQLite & Postgres)
            mb.Entity<Reservation>()
              .Property(r => r.CreatedAt)
              .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
