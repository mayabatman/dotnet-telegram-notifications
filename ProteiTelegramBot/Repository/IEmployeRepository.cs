using ProteiTelegramBot.Models;

namespace ProteiTelegramBot.Repository;

public interface IEmployeRepository
{
    Task<Employee> GeyByYouTrackLoginAsync(string youTrackLogin);

    Task<List<Employee>> GetEmployeesAsync();

    Task<Employee> GetEmployeeByIdAsync(int employeeId);

    Task<Employee> CreateEmployeeAsync(Employee employee);

    Task<Employee> UpdateEmployeeAsync(int employeeId, Employee employee);

    Task DeleteEmployeeAsync(int employeeId);
}