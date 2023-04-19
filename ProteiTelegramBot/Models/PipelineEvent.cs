using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    public class PipelineEvent : Event
    {
        public PipelineObjectAttributes object_attributes { get; set; }
        public PipelineUser user { get; set; }
        public PipelineProject project { get; set; }
    }

    public class PipelineObjectAttributes
    {
        public string status { get; set; }
        public List<string> stages { get; set; }
        public List<Variable> variables { get; set; }
    }

    public class PipelineProject
    {
        public string name { get; set; }
        public string web_url { get; set; }
    }

    public class PipelineUser
    {
        public string name { get; set; }
        public string username { get; set; }
    }

    public class Variable
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
