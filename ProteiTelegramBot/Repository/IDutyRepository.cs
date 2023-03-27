using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Repository;

public interface IDutyRepository
{
    Task<Duty> GeyByYouTrackLoginAsync(string youTrackLogin);
}