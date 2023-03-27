using Microsoft.EntityFrameworkCore;

namespace ProteiTelegramBot.Extensions
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var db = services.GetRequiredService<T>();
                await db.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<T>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            return host;
        }
    }
}