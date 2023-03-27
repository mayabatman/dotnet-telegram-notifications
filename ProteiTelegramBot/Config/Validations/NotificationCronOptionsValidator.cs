using FluentValidation;

namespace ProteiTelegramBot.Config.Validations;

public class NotificationCronOptionsValidator : AbstractValidator<NotificationCronOptions>
{
    public NotificationCronOptionsValidator()
    {
        // The hour in which the schedule will be activated(0-23).
        RuleFor(x => x.NotifyDutyAtHourUtc).Must(x => x is >= 0 and <= 23);
    }
}