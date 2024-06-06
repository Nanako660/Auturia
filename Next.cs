namespace Model
{
#pragma warning disable 8618
    public record Next
    {
        public string Storage { get; set; }
        public string Target { get; set; }
        public int Type { get; set; }
    }
}