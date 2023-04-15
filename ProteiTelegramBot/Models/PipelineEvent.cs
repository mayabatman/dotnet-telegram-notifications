using System.Text.Json.Serialization;

namespace ProteiTelegramBot.Models
{
    public class PipelineEvent //: Event
    {
        public PipelineProject Project { get; set; }

        public PipelineUser User { get; set; }

        [JsonPropertyName("source_pipeline")]
        public SourcePipeline Source  { get; set; }
    }

    public class PipelineUser 
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
    }
}
