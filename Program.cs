using Microsoft.EntityFrameworkCore;
using Vanilla.Data;
using Vanilla.Reservari.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ===================  DB: UN SINGUR AppDbContext  ===================
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(cs));

// ===================  RESTUL SERVICIILOR  ===================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS pentru dev (ajustează origin-urile după nevoie)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowDev", p => p
        .WithOrigins(
            "http://localhost:5173", "https://localhost:5173",
            "http://localhost:3000", "https://localhost:3000",
            "http://localhost:5041", "https://localhost:7138"
        )
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// IMPORTANT: NU mai înregistra DbContext și prin extensie.
// Dacă AddPersistence() adaugă și DbContext, scoate-l sau modifică-l să NU o mai facă.
//builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// ===================  STATIC UI (wwwroot)  ===================
app.UseDefaultFiles();   // trebuie înainte de UseStaticFiles
app.UseStaticFiles();

app.UseCors("AllowDev");

// Swagger: Dev SAU dacă setezi env var Swagger__Enabled=true
var swaggerEnabled = app.Environment.IsDevelopment()
                   || app.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(); // la /swagger
}

// Migrații/seed la pornire
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.InitializeAsync(scope.ServiceProvider);
}

app.MapControllers();

// SPA fallback (rute necunoscute -> index.html din wwwroot)
app.MapFallbackToFile("/index.html");

app.Run();
