using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Services;

public interface IDutyExtractor
{
    Task<Duty> GetCurrentDutyAsync();
}