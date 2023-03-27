using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Services;

public interface IDutyNotifier
{
    Task Notify(Duty duty);
}