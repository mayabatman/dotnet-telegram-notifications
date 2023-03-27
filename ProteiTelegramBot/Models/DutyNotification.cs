namespace ProteiTelegramBot.Models;

public class DutyNotification : Notification
{
    private readonly Duty _duty;

    public DutyNotification(Duty duty)
    {
        _duty = duty;
    }

    public override string GetMessage()
    {
        var messege =
            "***📟 Смена дежурного:***" +
            "\r\n" +
            $"Сегодня дежурит: @{_duty.TelegramLogin}";
        return messege;
    }
}