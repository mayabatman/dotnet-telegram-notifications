namespace ProteiTelegramBot.Config;

public class NotificationCronOptions
{
    public const string NotificationCron = nameof(NotificationCron);

    public int NotifyDutyAtHourUtc { get; set; } = 5;
}