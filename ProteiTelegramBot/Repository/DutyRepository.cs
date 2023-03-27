using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Repository;

class DutyRepository : IDutyRepository
{
    private readonly IEmployeRepository _employeRepository;

    public DutyRepository(IEmployeRepository employeRepository)
    {
        _employeRepository = employeRepository;
    }

    public async Task<Duty> GeyByYouTrackLoginAsync(string youTrackLogin)
    {
        var employee = await _employeRepository.GeyByYouTrackLoginAsync(youTrackLogin);
        return new Duty()
        {
            TelegramLogin = employee.TelegramLogin,
            ProteiLogin = employee.ProteiLogin,
        };
    }
}