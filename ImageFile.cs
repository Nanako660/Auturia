using Newtonsoft.Json;

namespace Model
{
    public record ImageFile
    {
        [JsonProperty(PropertyName = "file")]
        public string? FileName { get; set; }

        [JsonProperty(PropertyName = "options")]
        public ImageOption? ImageOption { get; set; }
    }
}