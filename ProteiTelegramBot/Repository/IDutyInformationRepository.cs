using System.Data;
using Microsoft.EntityFrameworkCore;
using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Repository;

public interface IDutyInformationRepository
{
    Task<bool> TryToCreateNewDutyAsync(Duty duty);
}

class DutyInformationRepository : IDutyInformationRepository
{
    private readonly IEmployeRepository _employeRepository;
    private readonly DataContext _dataContext;
    private readonly ILogger<DutyInformationRepository> _logger;
    private readonly DbSet<DutyInformation> _dutyInformationContext;

    public DutyInformationRepository(IEmployeRepository employeRepository,
        DataContext dataContext,
        ILogger<DutyInformationRepository> logger)
    {
        _employeRepository = employeRepository;
        _dataContext = dataContext;
        _logger = logger;
        _dutyInformationContext = _dataContext.DutyInformation;
    }

    public async Task<bool> TryToCreateNewDutyAsync(Duty duty)
    {
        var today = DateTimeUtcOnlyYearMonthDay();
        var wk = today.DayOfWeek;
        if (wk is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            _logger.LogInformation("Skip not working day");
            return false;
        }

        var employee = await _employeRepository.GeyByYouTrackLoginAsync(duty.ProteiLogin);

        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);

        try
        {
            var dutyInfo = new DutyInformation()
            {
                DutyDate = today,
                DutyEmployeeId = employee.Id
            };

            if (_dutyInformationContext.Any(x => x.DutyDate == dutyInfo.DutyDate))
            {
                _logger.LogError($"Duty at date: {dutyInfo.DutyDate:yy-MM-dd} already exists");
                return false;
            }

            await _dutyInformationContext.AddAsync(dutyInfo);
            await _dataContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException dbuex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(dbuex.InnerException != null
                ? $"API call, Time {DateTime.Now:ss:fffffff}: Exception DbUpdateException caught, Inner Exception Message: {dbuex.InnerException.Message}\n"
                : $"API call, Time {DateTime.Now:ss:fffffff}: Exception DbUpdateException caught, Exception Message: {dbuex.Message}\n");
            return false;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"API call, Time {DateTime.Now:ss:fffffff}: Exception caught: {ex.Message}\n");
            return false;
        }

        return true;
    }

    private DateTime DateTimeUtcOnlyYearMonthDay()
    {
        var utc = DateTime.UtcNow;
        var withoutAdditional = new DateTime(utc.Year, utc.Month, utc.Day);
        withoutAdditional = DateTime.SpecifyKind(withoutAdditional, DateTimeKind.Utc);
        return withoutAdditional;
    }
}