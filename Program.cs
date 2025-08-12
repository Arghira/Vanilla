using Microsoft.EntityFrameworkCore;
using Vanilla.Data;
using Vanilla.Reservari.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add this line to configure your DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger + Controllers + CORS
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DB + Services (în extensii)
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();
app.UseDefaultFiles();  // caută index.html în wwwroot
app.UseStaticFiles();   // servește fișierele din wwwroot

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Migrații/seed automat la pornire (prod/dev)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(services);
}

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
