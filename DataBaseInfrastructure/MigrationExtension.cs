using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataBaseInfrastructure;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        var logger = services.GetRequiredService<ILogger<MyDbContext>>();
        var context = services.GetRequiredService<MyDbContext>();

        try
        {
            logger.LogInformation("Применение миграций...");
            
            // ЭТО ВСЁ, ЧТО НУЖНО!
            // MigrateAsync сам создаст БД, если её нет,
            // и применит все ожидающие миграции
            await context.Database.MigrateAsync();
            
            logger.LogInformation("Миграции успешно применены");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при применении миграций");
        }
    }
}