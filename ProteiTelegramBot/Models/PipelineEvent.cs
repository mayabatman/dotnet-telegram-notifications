using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    /*public class PipelineEvent //: Event
    {
        public PipelineProject Project { get; set; }

        public PipelineUser User { get; set; }

        [JsonPropertyName("source_pipeline")]
        public SourcePipeline Source  { get; set; }
    }

    /*public class PipelineUser 
    {
        public string Name { get; set; }

        public string Username { get; set; }
    }

    public class PipelineProject 
    {
        public string Name { get; set; }
    }

    public class SourcePipeline
    {
        [JsonPropertyName("pipeline_id")]
        public int Id { get; set; }
        public SourceProject Project { get; set; }
    }

    public class SourceProject
    {
        [JsonPropertyName("web_url")]
        public string Url { get; set; }
    }*/

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class PipelineEvent
    {
        public string object_kind { get; set; }
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
        public string description { get; set; }
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
