using ProteiTelegramBot.Repository;

namespace ProteiTelegramBot.Services;

public class DutyService : IDutyService
{
    private readonly ILogger<DutyService> _logger;
    private readonly IDutyExtractor _dutyExtractor;
    private readonly IDutyNotifier _dutyNotifier;
    private readonly IDutyInformationRepository _dutyInformationRepository;

    public DutyService(ILogger<DutyService> logger,
        IDutyExtractor dutyExtractor,
        IDutyNotifier dutyNotifier,
        IDutyInformationRepository dutyInformationRepository)
    {
        _logger = logger;
        _dutyExtractor = dutyExtractor;
        _dutyNotifier = dutyNotifier;
        _dutyInformationRepository = dutyInformationRepository;
    }

    public async Task UpdateDutyAsync()
    {
        _logger.LogInformation($"Invoke update duty");
        var duty = await _dutyExtractor.GetCurrentDutyAsync();
        if (await _dutyInformationRepository.TryToCreateNewDutyAsync(duty))
        {
            _logger.LogInformation($"Notified about new duty");
            await _dutyNotifier.Notify(duty);
        }
    }
}