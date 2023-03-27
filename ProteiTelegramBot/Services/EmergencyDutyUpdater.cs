namespace ProteiTelegramBot.Services;

public class EmergencyDutyUpdater
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EmergencyDutyUpdater> _logger;

    public const string JobId = nameof(EmergencyDutyUpdater);

    public EmergencyDutyUpdater(IServiceScopeFactory serviceScopeFactory,
        ILogger<EmergencyDutyUpdater> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task UpdateEmergencyDutyAsync()
    {
        try
        {
            _logger.LogInformation($"Update information about duty at {DateTime.Now}");
            var scope = _serviceScopeFactory.CreateScope();

            var dutyService = scope.ServiceProvider.GetService<IDutyService>() ??
                              throw new InvalidOperationException("IDutyService not registered");
            await dutyService.UpdateDutyAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception due to:{Environment.NewLine}{e}");
            throw;
        }
    }
}