using System.Collections.Generic;

namespace Model
{
#pragma warning disable 8618
    public record ImageInfo
    {
        public List<Zoom> Zooms { get; set; }
        public string Class { get; set; }
        public string Name { get; set; }
        public Redraw? Redraw { get; set; }
        public int ShowMode { get; set; }
        public object? Type { get; set; }

    }
}