namespace Model
{
#pragma warning disable 8618

    /// <summary>
    /// 缩放
    /// </summary>
    public record Zoom
    {
        public string Axis { get; set; }
        public object Val { get; set; }
    }
}