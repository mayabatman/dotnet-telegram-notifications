using Microsoft.Extensions.Options;
using ProteiTelegramBot.Config;
using ProteiTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ProteiTelegramBot.Services;

class Notifier : INotifier
{
    private readonly ITelegramBotClient _botClient;
    private readonly TelegramOptions _telegramSettings;

    public Notifier(ITelegramBotClient botClient,
        ILogger<Notifier> logger,
        IOptions<TelegramOptions> telegramSettings)
    {
        _botClient = botClient;
        _telegramSettings = telegramSettings.Value;
    }

    public async Task Notify(Notification notification)
    {
        await _botClient.SendTextMessageAsync(_telegramSettings.ChatId, notification.GetMessage(),
            ParseMode.Markdown);
    }
}