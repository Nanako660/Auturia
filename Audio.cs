namespace Model
{
#pragma warning disable 8618
    public record Audio
    {
        public string Name { get; set; }
        public int Pan { get; set; }
        public int Type { get; set; }
        public string Voice { get; set; }
    }
}