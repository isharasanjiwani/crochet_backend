using System;
using CrochetWorldApi.Data;

namespace CrochetWorldApi.BackgroundServices;

public class ClearOldRefreshTokenBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ClearOldRefreshTokenBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Service} ExecuteAsync started.", nameof(ClearOldRefreshTokenBackgroundService));
        
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var deletedTokens = db.RefreshTokens.Where(rt => rt.ExpiryDate < DateTime.UtcNow);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    // Log the exception or handle it as needed
                    logger.LogError(ex, "Iteration failed. Continuing the loop.");
                }

                // Wait for a specified interval before running again (e.g., 24 hours)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        } 
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            
        } 
        catch (Exception ex)
        {
            logger.LogError(ex, "Background service encountered an unexpected error.");
        }
        finally
        {
            logger.LogInformation("{Service} ExecuteAsync existing.", nameof(ClearOldRefreshTokenBackgroundService));
        }
        
    }
}
