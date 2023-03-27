using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Services;

class DutyTelegramNotifier : IDutyNotifier
{
    private readonly ILogger<DutyTelegramNotifier> _logger;
    private readonly INotifier _notifier;

    public DutyTelegramNotifier(ILogger<DutyTelegramNotifier> logger,
        INotifier notifier)
    {
        _logger = logger;
        _notifier = notifier;
    }

    public async Task Notify(Duty duty)
    {
        _logger.LogInformation($"Notification send to Telegram about {duty.ProteiLogin}");
        await _notifier.Notify(new DutyNotification(duty));
    }
}