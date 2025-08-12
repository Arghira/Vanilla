using Microsoft.EntityFrameworkCore;
using Vanilla.Data;
using Vanilla.Services;

namespace Vanilla.Reservari.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        var provider = cfg["DatabaseProvider"] ?? "sqlite";
        var cs = cfg.GetConnectionString("Default") ?? "Data Source=app.db";

        services.AddDbContext<AppDbContext>(opt =>
        {
            if (provider.Equals("postgres", StringComparison.OrdinalIgnoreCase))
                opt.UseNpgsql(cs);
            else
                opt.UseSqlite(cs);
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IReservationService, ReservationService>();
        return services;
    }
}
