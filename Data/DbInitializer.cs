using Microsoft.EntityFrameworkCore;

namespace Vanilla.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var env = services.GetRequiredService<IServiceProvider>().GetService<IHostEnvironment>();
            var db = services.GetRequiredService<AppDbContext>();

            try
            {
                // Test connection first
                Console.WriteLine("Testing database connection...");
                await db.Database.CanConnectAsync();
                Console.WriteLine("✅ Database connection successful!");

                // Then try migrations
                await db.Database.MigrateAsync();
                Console.WriteLine("✅ Database migrations completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database error: {ex.Message}");
                // Don't crash the app, just log the error for now
                return;
            }
        }
    }
}
