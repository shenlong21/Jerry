using Jerry.API.Data;
using Jerry.API.Repositories.Interfaces;
using Jerry.API.Repositories.Implementations;
using Jerry.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SQLite3 Database
var connectionString = "Data Source=jerry.sqlite3";
builder.Services.AddDbContext<JerryContext>(options =>
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        sqliteOptions.MigrationsAssembly(typeof(JerryContext).Assembly.FullName);
    }));

// Add Services
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISaltTaskRepository, SaltTaskRepository>();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize SQLite3 database with migrations
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<JerryContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogInformation("Starting database initialization...");

        // Ensure database is created
        await dbContext.Database.EnsureCreatedAsync();
        logger.LogInformation("Database file created/verified");

        // Get pending migrations
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Found {pendingMigrations.Count()} pending migrations");
            foreach (var migration in pendingMigrations)
            {
                logger.LogInformation($"  - {migration}");
            }

            // Apply all pending migrations
            await dbContext.Database.MigrateAsync();
            logger.LogInformation("All migrations applied successfully");
        }
        else
        {
            logger.LogInformation("No pending migrations");
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while initializing the database");
    throw;
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
