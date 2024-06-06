using System.Collections.Generic;

namespace Model
{
#pragma warning disable 8618
    public record Text
    {
        public string? Name { get; set; }
        public TextContent TextContent { get; set; }
        public List<Audio>? Audios { get; set; }
        public int Id { get; set; }
        public TextData TextData { get; set; }
    }
}