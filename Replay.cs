namespace Model
{
    public record Replay
    {
        public string FileName { get; set; }
        public int Loop { get; set; }
        public string? Start { get; set; }
        public int State { get; set; }
        public double Volume { get; set; }
    }
}