using Microsoft.EntityFrameworkCore;
using StudentRegistration.Application.Services;
using StudentRegistration.Domain.Repositories;
using StudentRegistration.Infrastructure.Data;
using StudentRegistration.Infrastructure.Repositories;

// STEP 1: Load environment variables from .env file FIRST (before CreateBuilder)
var possiblePaths = new[]
{
    Path.Combine(Directory.GetCurrentDirectory(), ".env"),
    Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env"),
    Path.Combine(AppContext.BaseDirectory, "..", "..", ".env"),
    Path.Combine(AppContext.BaseDirectory, ".env")
};

string? envPath = null;
foreach (var path in possiblePaths)
{
    var fullPath = Path.GetFullPath(path);
    if (File.Exists(fullPath))
    {
        envPath = fullPath;
        break;
    }
}

if (!string.IsNullOrEmpty(envPath) && File.Exists(envPath))
{
    Console.WriteLine($"Loading .env from: {envPath}\n");
    foreach (var line in File.ReadAllLines(envPath))
    {
        // Skip empty lines and comments
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            continue;

        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            var key = parts[0].Trim();
            var value = parts[1].Trim();
            Environment.SetEnvironmentVariable(key, value);
            Console.WriteLine($"  ✓ {key}");
        }
    }
    Console.WriteLine("\n✓ Environment variables loaded from .env file\n");
}
else
{
    Console.WriteLine("⚠ No .env file found. Using default connection string.\n");
}
var builder = WebApplication.CreateBuilder(args);

// Display current environment
var currentEnvironment = builder.Environment.EnvironmentName;
Console.WriteLine($"\n🌍 ASPNETCORE_ENVIRONMENT: {currentEnvironment}\n");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Student Registration API",
        Version = "v1",
        Description = "API for student registration system with credit management"
    });
});

// STEP 2: Database Configuration - Build connection string from environment variables
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "StudentRegistrationDB";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root";

var connectionString = $"Server={dbServer};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};";



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Usar versión específica de MySQL (8.0) en lugar de AutoDetect
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
});

// Register Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IInscriptionRepository, InscriptionRepository>();

// Register Application Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IInscriptionService, InscriptionService>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4201")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Apply migrations and seed data on startup (Development and Staging)
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        Console.WriteLine("Testing database connection...");
        var canConnect = await dbContext.Database.CanConnectAsync();

        if (canConnect)
        {
            Console.WriteLine("✓ Database connection successful!");

            // Apply pending migrations
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("✓ Database migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("✗ Cannot connect to database. Check connection string in appsettings.json");
            Console.WriteLine($"Connection string: {connectionString}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ Error with database: {ex.Message}");
        Console.WriteLine("The API will start but database operations will fail.");
        Console.WriteLine($"Connection string: {connectionString}");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Registration API v1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
