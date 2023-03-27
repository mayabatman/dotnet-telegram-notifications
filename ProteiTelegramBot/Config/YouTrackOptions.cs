namespace ProteiTelegramBot.Config;

public class YouTrackOptions
{
    public const string YouTrack = nameof(YouTrack);

    public string Token { get; set; } = null!;

    public string Task { get; set; } = null!;

    public string Url { get; set; } = null!;
}