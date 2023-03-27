using FluentValidation;

namespace ProteiTelegramBot.Config.Validations;

public class TelegramOptionsValidator : AbstractValidator<TelegramOptions>
{
    public TelegramOptionsValidator()
    {
        RuleFor(x => x.BotToken).NotEmpty();
        RuleFor(x => x.ChatId).NotEmpty().NotEqual(0);
    }
}