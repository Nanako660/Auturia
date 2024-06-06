using System.Collections.Generic;
using UnityEngine.UI;

namespace Model
{
#pragma warning disable 8618
    public record Scene
    {
        public int FirstLine { get; set; }
        public string Label { get; set; }
        public List<Next> Nexts { get; set; }
        public List<Select> Selects { get; set; }
        public int SpCount { get; set; }
        public List<Text>? Texts { get; set; }
        public List<string> Title { get; set; }
        public double Version { get; set; }
    }
}