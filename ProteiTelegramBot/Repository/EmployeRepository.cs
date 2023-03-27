using Microsoft.EntityFrameworkCore;
using ProteiTelegramBot.Exceptions;
using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Repository;

public class EmployeRepository : IEmployeRepository
{
    private readonly DbSet<Employee> _employees;
    private readonly DataContext _dataContext;
    private readonly ILogger<EmployeRepository> _logger;


    public EmployeRepository(DataContext dataContext,
        ILogger<EmployeRepository> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
        _employees = dataContext.Employees;
    }

    public async Task<Employee> GeyByYouTrackLoginAsync(string youTrackLogin)
    {
        var employee = await _employees.Where(x => x.ProteiLogin == youTrackLogin)
            .FirstOrDefaultAsync();

        if (employee == null)
        {
            throw new EntityNotFoundException($"User with YouTrack login '{youTrackLogin}' not found");
        }

        return employee;
    }

    public Task<List<Employee>> GetEmployeesAsync()
    {
        return _employees.ToListAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
    {
        var employee = await _employees.FindAsync(employeeId);

        if (employee == null)
        {
            throw new EntityNotFoundException(nameof(Employee));
        }

        return employee;
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        try
        {
            await _employees.AddAsync(employee);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Can't create employee. Employee with same name data already exists");
            throw new EntityAlreadyExistsException();
        }

        await _dataContext.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateEmployeeAsync(int employeeId, Employee employee)
    {
        var originalEmployee = await _employees.FindAsync(employeeId);

        if (originalEmployee == null)
        {
            throw new EntityNotFoundException(nameof(Employee));
        }

        originalEmployee.TelegramLogin = employee.TelegramLogin;
        originalEmployee.ProteiLogin = employee.ProteiLogin;
        await _dataContext.SaveChangesAsync();

        return originalEmployee;
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        var employee = await _employees.FindAsync(employeeId);

        if (employee == null)
        {
            throw new EntityNotFoundException(nameof(Employee));
        }

        _employees.Remove(employee);
        await _dataContext.SaveChangesAsync();
    }
}