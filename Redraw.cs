namespace Model
{
#pragma warning disable 8618
    public record Redraw
    {
        public int Disp { get; set; }
        public ImageFile ImageFile { get; set; }
        public string? PosName { get; set; }
    }
}