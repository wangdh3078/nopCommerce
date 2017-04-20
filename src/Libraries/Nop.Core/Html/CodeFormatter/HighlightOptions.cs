namespace Nop.Core.Html.CodeFormatter
{
    /// <summary>
    /// 处理用于更改渲染代码的所有选项。
    /// </summary>
    public partial class HighlightOptions
    {
        public string Code { get; set; }
        public bool DisplayLineNumbers { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public bool AlternateLineNumbers { get; set; }
    }
}

