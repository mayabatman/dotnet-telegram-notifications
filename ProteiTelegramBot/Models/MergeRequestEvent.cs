using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    /*
    public class MergeRequestEventRequest
    {
        public MergeRequestProject Project { get; set; }
        public MergeRequestUser User { get; set; }
        public MergeRequestObjectAttributes ObjectAttributes { get; set; }
    }
    
    public class MergeRequestUser
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("Username")]
        public string Username { get; set; }
    }

    public class MergeRequestProject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class MergeRequestObjectAttributes
    {
        public string Description { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string State { get; set; }

        public string Action { get; set; }
    }*/
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class MergeRequestEvent
    {
        public string object_kind { get; set; }
        public MergeRequestUser user { get; set; }
        public MergeRequestProject project { get; set; }
        public MergeRequestObjectAttributes object_attributes { get; set; }
    }
    public class MergeRequestObjectAttributes
    {
        public string title { get; set; }
        public string state { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string action { get; set; }
    }


    public class MergeRequestProject
    {
        public string name { get; set; }
    }

    public class MergeRequestUser
    {
        public string name { get; set; }
        public string username { get; set; }
    }


}