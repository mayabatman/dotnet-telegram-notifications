namespace ProteiTelegramBot.Models;

public class EmployeeResponse
{
    public int Id { get; set; }

    public string TelegramLogin { get; set; }

    public string ProteiLogin { get; set; }
}

public class EmployeesResponse
{
    public IEnumerable<EmployeeResponse> Items { get; set; }
}

public class EmployeeRequest
{
    public string TelegramLogin { get; set; }

    public string ProteiLogin { get; set; }
}