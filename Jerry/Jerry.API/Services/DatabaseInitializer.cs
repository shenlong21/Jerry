using Jerry.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Jerry.API.Services
{
    public interface IDatabaseInitializer
    {
        Task InitializeDatabaseAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly JerryContext _dbContext;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(JerryContext dbContext, ILogger<DatabaseInitializer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Starting database migration...");
                
                // Apply migrations
                await _dbContext.Database.MigrateAsync();
                
                _logger.LogInformation("Database initialized and migrations applied successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing database");
                throw;
            }
        }
    }
}
