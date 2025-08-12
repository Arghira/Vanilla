using Microsoft.EntityFrameworkCore;

namespace Vanilla.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var env = services.GetRequiredService<IHostEnvironment>();
            var db = services.GetRequiredService<AppDbContext>();

            // Aplică migrările
            await db.Database.MigrateAsync();

            // Seed-ul pentru mese e deja în HasData; nimic suplimentar aici.
            // Poți adăuga alte seed-uri dacă vrei.
        }
    }
}
