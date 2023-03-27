namespace ProteiTelegramBot.Models
{
    public class DutyInformation
    {
        public int Id { get; set; }

        public int DutyEmployeeId { get; set; }

        public Employee DutyEmployee { get; set; }

        public DateTime DutyDate { get; set; }
    }
}