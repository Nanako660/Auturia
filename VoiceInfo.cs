namespace Model
{
#pragma warning disable 8618
    public record VoiceInfo
    {
        public string Name { get; set; }
        public Replay Replay { get; set; }
        public Update Update { get; set; }
    }
}