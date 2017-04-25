using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Nop.Core.Html.CodeFormatter
{
    /// <summary>
    ///代码格式帮助类
    /// </summary>
    public partial class CodeFormatHelper
    {
        #region 字段
        //private static Regex regexCode1 = new Regex(@"(?<begin>\[code:(?<lang>.*?)(?:;ln=(?<linenumbers>(?:on|off)))?(?:;alt=(?<altlinenumbers>(?:on|off)))?(?:;(?<title>.*?))?\])(?<code>.*?)(?<end>\[/code\])", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private readonly static Regex regexHtml = new Regex("<[^>]*>", RegexOptions.Compiled);
        private readonly static Regex regexCode2 = new Regex(@"\[code\](?<inner>(.*?))\[/code\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region 方法

        /// <summary>
        /// 格式化文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>格式化后的文本</returns>
        public static string FormatTextSimple(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.Contains("[/code]"))
            {
                text = regexCode2.Replace(text, new MatchEvaluator(CodeEvaluatorSimple));
                text = regexCode2.Replace(text, "$1");
            }
            return text;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 代码评估器方法
        /// </summary>
        /// <param name="match">匹配</param>
        /// <returns>格式化后的文本</returns>
        private static string CodeEvaluator(Match match)
        {
            if (!match.Success)
                return match.Value;

            var options = new HighlightOptions()
            {
                Language = match.Groups["lang"].Value,
                Code = match.Groups["code"].Value,
                DisplayLineNumbers = match.Groups["linenumbers"].Value == "on",
                Title = match.Groups["title"].Value,
                AlternateLineNumbers = match.Groups["altlinenumbers"].Value == "on"
            };
            string result = match.Value.Replace(match.Groups["begin"].Value, "");
            result = result.Replace(match.Groups["end"].Value, "");
            result = Highlight(options, result);
            return result;

        }

        /// <summary>
        /// 代码评估器方法
        /// </summary>
        /// <param name="match">匹配</param>
        /// <returns>格式化后的文本</returns>
        private static string CodeEvaluatorSimple(Match match)
        {
            if (!match.Success)
                return match.Value;

            var options = new HighlightOptions();

            options.Language = "c#";
            options.Code = match.Groups["inner"].Value;
            options.DisplayLineNumbers = false;
            options.Title =string.Empty;
            options.AlternateLineNumbers =false;

            string result = match.Value;
            result = Highlight(options, result);
            return result;

        }

        /// <summary>
        /// Strips HTML
        /// </summary>
        /// <param name="html">HTML</param>
        /// <returns>Formatted text</returns>
        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            return regexHtml.Replace(html, string.Empty);
        }

        /// <summary>
        /// 返回格式化的文本。
        /// </summary>
        /// <param name="options">Whatever options were set in the regex groups.</param>
        /// <param name="text">Send the e.body so it can get formatted.</param>
        /// <returns>The formatted string of the match.</returns>
        private static string Highlight(HighlightOptions options, string text)
        {
            switch (options.Language)
            {
                case "c#":
                    var csf = new CSharpFormat();
                    csf.LineNumbers = options.DisplayLineNumbers;
                    csf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(csf.FormatCode(text));

                case "vb":
                    var vbf = new VisualBasicFormat();
                    vbf.LineNumbers = options.DisplayLineNumbers;
                    vbf.Alternate = options.AlternateLineNumbers;
                    return vbf.FormatCode(text);

                case "js":
                    var jsf = new JavaScriptFormat();
                    jsf.LineNumbers = options.DisplayLineNumbers;
                    jsf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(jsf.FormatCode(text));

                case "html":
                    var htmlf = new HtmlFormat();
                    htmlf.LineNumbers = options.DisplayLineNumbers;
                    htmlf.Alternate = options.AlternateLineNumbers;
                    text = StripHtml(text).Trim();
                    string code = htmlf.FormatCode(HttpUtility.HtmlDecode(text)).Trim();
                    return code.Replace("\r\n", "<br />").Replace("\n", "<br />");

                case "xml":
                    var xmlf = new HtmlFormat();
                    xmlf.LineNumbers = options.DisplayLineNumbers;
                    xmlf.Alternate = options.AlternateLineNumbers;
                    text = text.Replace("<br />", "\r\n");
                    text = StripHtml(text).Trim();
                    string xml = xmlf.FormatCode(HttpUtility.HtmlDecode(text)).Trim();
                    return xml.Replace("\r\n", "<br />").Replace("\n", "<br />");

                case "tsql":
                    var tsqlf = new TsqlFormat();
                    tsqlf.LineNumbers = options.DisplayLineNumbers;
                    tsqlf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(tsqlf.FormatCode(text));

                case "msh":
                    var mshf = new MshFormat();
                    mshf.LineNumbers = options.DisplayLineNumbers;
                    mshf.Alternate = options.AlternateLineNumbers;
                    return HttpUtility.HtmlDecode(mshf.FormatCode(text));

            }

            return string.Empty;
        }

        #endregion

        
    }
}

