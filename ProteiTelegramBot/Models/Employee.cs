namespace ProteiTelegramBot.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string TelegramLogin { get; set; } = null!;

        public string ProteiLogin { get; set; } = null!;

        public List<DutyInformation>? DutyRoster { get; set; }
    }
}