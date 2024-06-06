namespace Model
{
#pragma warning disable 8618
    public record TextContent
    {
        public string? State { get; set; }
        public string Content { get; set; }
        public int Id { get; set; }
    }
}