using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    public class MergeRequestEvent : Event
    {
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