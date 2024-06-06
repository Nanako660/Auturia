namespace Model
{
    /// <summary>
    /// 立绘选项
    /// 例如衣服，表情，动作等
    /// </summary>
    /// <param name="Deress"></param>
    /// <param name="Face"></param>
    /// <param name="Pose"></param>
    public record ImageOption
    {
        public string Dress { get; set; }
        public string Face { get; set; }
        public string Pose { get; set; }
    }
}