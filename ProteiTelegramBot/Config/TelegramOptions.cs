namespace ProteiTelegramBot.Config;

public class TelegramOptions
{
    public const string Telegram = nameof(Telegram);

    public string BotToken { get; set; } = null!;

    public long ChatId { get; set; }
}