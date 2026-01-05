using Microsoft.EntityFrameworkCore;
using SmartStoreReservation.Data;
using SmartStoreReservation.Services;
using SmartStoreReservation.Core.Interfaces;
using SmartStoreReservation.Core.Mappings;
using SmartStoreReservation.API.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adaugă servicii în container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "SmartStore Reservation API", 
        Version = "v1",
        Description = "API pentru Sistemul de Rezervare Cabine SmartStore"
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Helper pentru a obține connection string din variabile de mediu (Docker) sau appsettings
var connectionString = Environment.GetEnvironmentVariable("Supabase_ConnectionString") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string-ul bazei de date lipsește! Vă rugăm să setați variabila de mediu Supabase_ConnectionString sau DefaultConnection în appsettings.json");
}

// Loghează connection string-ul (mascheaza parola pentru securitate)
var maskedConnStr = connectionString.Contains("Password=") 
    ? System.Text.RegularExpressions.Regex.Replace(connectionString, @"Password=[^;]*", "Password=***")
    : connectionString;
Console.WriteLine($"Se folosește connection string-ul: {maskedConnStr}");

// Baza de date
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repository Pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicii
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Seed Data Service
builder.Services.AddScoped<SeedDataService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// CORS pentru Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5000", "http://localhost:5269")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Seed Data - Rulează la pornire
using (var scope = app.Services.CreateScope())
{
    try
    {
        var seedService = scope.ServiceProvider.GetRequiredService<SeedDataService>();
        await seedService.SeedAsync();
        Console.WriteLine("✅ Seed data a fost încărcat cu succes!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Eroare la încărcarea seed data: {ex.Message}");
    }
}

// Configurează pipeline-ul de cereri HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartStore API v1");
        c.RoutePrefix = string.Empty; // Swagger la root
    });
}

// Global Exception Handler
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => new { Status = "Sănătos", Timestamp = DateTime.UtcNow });

app.Run();
app.UseCors("AllowAll");
// app.UseHttpsRedirection(); // Disable for simple Docker setup on 8080/http
app.UseAuthorization();
app.MapControllers();

app.Run();
