using FluentValidation;

namespace ProteiTelegramBot.Config.Validations;

public class YouTrackOptionsValidator : AbstractValidator<YouTrackOptions>
{
    public YouTrackOptionsValidator()
    {
        RuleFor(x => x.Task).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.Url).NotEmpty();
    }
}