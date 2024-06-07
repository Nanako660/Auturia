namespace Model
{
    public class Select
    {
        public record Select
        {
            public string Exp { get; set; }
            [JsonProperty("language")]
            public List<LanguageText> LanguageTexts { get; set; }
            public int Render { get; set; }
            public int Selidx { get; set; }
            public string Storage { get; set; }
            public string Tag { get; set; }
            public string Target { get; set; }
            public string Text { get; set; }
        }
    }
}