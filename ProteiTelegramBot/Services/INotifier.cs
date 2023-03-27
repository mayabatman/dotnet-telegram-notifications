using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Services;

public interface INotifier
{
    Task Notify(Notification notification);
}