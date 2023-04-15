using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    public class Event
    {
        public RequestProject? Project { get; set; }

        public RequestUser? User { get; set; }

        [JsonPropertyName("object_attributes")]
        public RequestObjectAttributes? ObjectAttributes { get; set; }

        [JsonPropertyName("object_kind")]
        public string? ObjectKind { get; set; }

    }

    public class RequestUser //пользователь, который создал запрос
    {
        public string? Name { get; set; }

        public string? Username { get; set; }
    }

    public class RequestProject // проект к которому зарпосили мердж
    {
        public string? Name { get; set; }
    }

    public class RequestObjectAttributes// подробности о проекте в котором мердж реквест
    {
        public string? Description { get; set; }

        public string? Title { get; set; }

        public string? Url { get; set; }

        public string? State { get; set; }

        public string? Action { get; set; }
    }
}
