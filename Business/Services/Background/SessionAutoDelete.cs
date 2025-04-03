using Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services.Background;

public class SessionAutoDelete : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SessionAutoDelete> _logger;

    public SessionAutoDelete(IServiceProvider serviceProvider, ILogger<SessionAutoDelete> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SessionAutoDelete service started...");
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            _logger.LogInformation("30 minutes delay before scanning...");
            await DeleteExpiredSessionsAsync();
        }
    }

    private async Task DeleteExpiredSessionsAsync()
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            ISessionRepository sessionRepo = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
            ICollection<SessionEntity> sessions = await sessionRepo.GetAllSessionsAsync();
        
            foreach (SessionEntity session in sessions)
            {
                if (session.StartDate <= DateTime.UtcNow)
                {
                    await sessionRepo.DeleteSessionAsync(session.Id);
                    _logger.LogInformation($"Deleted expired session {session.Id}");
                }
            }
        }
    }
}