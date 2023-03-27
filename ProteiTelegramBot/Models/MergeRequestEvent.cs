using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    public class MergeRequestEventRequest
    {
        public MergeRequestProject Project { get; set; }

        public MergeRequestUser User { get; set; }

        [JsonPropertyName("object_attributes")]
        public MergeRequestObjectAttributes ObjectAttributes { get; set; }
    }

    public class MergeRequestUser
    {
        public string Name { get; set; }

        public string Username { get; set; }
    }

    public class MergeRequestProject
    {
        public string Name { get; set; }
    }

    public class MergeRequestObjectAttributes
    {
        public string Description { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string State { get; set; }

        public string Action { get; set; }
    }
}