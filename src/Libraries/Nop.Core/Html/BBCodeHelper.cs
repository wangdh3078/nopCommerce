using System;
using System.Text.RegularExpressions;
using Nop.Core.Domain.Common;
using Nop.Core.Html.CodeFormatter;
using Nop.Core.Infrastructure;

namespace Nop.Core.Html
{
    /// <summary>
    /// 代表一个BBCode帮助类
    /// </summary>
    public partial class BBCodeHelper
    {
        #region 字段
        private static readonly Regex regexBold = new Regex(@"\[b\](.+?)\[/b\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexItalic = new Regex(@"\[i\](.+?)\[/i\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUnderLine = new Regex(@"\[u\](.+?)\[/u\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl1 = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl2 = new Regex(@"\[url\](.+?)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexQuote = new Regex(@"\[quote=(.+?)\](.+?)\[/quote\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexImg = new Regex(@"\[img\](.+?)\[/img\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region 方法
        /// <summary>
        /// 格式化文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="replaceBold">指示是否替换粗体的值</param>
        /// <param name="replaceItalic">指示是否替换斜体的值</param>
        /// <param name="replaceUnderline">指示是否替换下划线的值</param>
        /// <param name="replaceUrl">指示是否替换URL的值</param>
        /// <param name="replaceCode">指示是否替换代码的值</param>
        /// <param name="replaceQuote">一个值表示是否替换引号</param>
        /// <param name="replaceImg">指示是否替换Img的值</param>
        /// <returns>格式化后的文本</returns>
        public static string FormatText(string text, bool replaceBold, bool replaceItalic,
            bool replaceUnderline, bool replaceUrl, bool replaceCode, bool replaceQuote, bool replaceImg)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (replaceBold)
            {
                //格式化粗体标签: [b][/b]
                // 成为: <strong></strong>
                text = regexBold.Replace(text, "<strong>$1</strong>");
            }

            if (replaceItalic)
            {
                // 格式化斜体标签: [i][/i]
                // 成为: <em></em>
                text = regexItalic.Replace(text, "<em>$1</em>");
            }

            if (replaceUnderline)
            {
                // 格式下划线标签: [u][/u]
                // 成为: <u></u>
                text = regexUnderLine.Replace(text, "<u>$1</u>");
            }

            if (replaceUrl)
            {
                var newWindow = EngineContext.Current.Resolve<CommonSettings>().BbcodeEditorOpenLinksInNewWindow;
                // 格式化URL标签: [url=http://www.nopCommerce.com]my site[/url]
                // 成为: <a href="http://www.nopCommerce.com">my site</a>
                text = regexUrl1.Replace(text, string.Format("<a href=\"$1\" rel=\"nofollow\"{0}>$2</a>", newWindow ? " target=_blank" : ""));

                // 格式化URL标签: [url]http://www.nopCommerce.com[/url]
                // 成为: <a href="http://www.nopCommerce.com">http://www.nopCommerce.com</a>
                text = regexUrl2.Replace(text, string.Format("<a href=\"$1\" rel=\"nofollow\"{0}>$1</a>", newWindow ? " target=_blank" : ""));
            }

            if (replaceQuote)
            {
                while (regexQuote.IsMatch(text))
                    text = regexQuote.Replace(text, "<b>$1 wrote:</b><div class=\"quote\">$2</div>");
            }

            if (replaceCode)
            {
                text = CodeFormatHelper.FormatTextSimple(text);
            }

            if (replaceImg)
            {
                // format the img tags: [img]http://www.nopCommerce.com/Content/Images/Image.jpg[/img]
                // 成为: <img src="http://www.nopCommerce.com/Content/Images/Image.jpg">
                text = regexImg.Replace(text, "<img src=\"$1\" class=\"user-posted-image\" alt=\"\">");
            }
            return text;
        }

        /// <summary>
        /// 从字符串中删除所有引号
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string RemoveQuotes(string str)
        {
            str = Regex.Replace(str, @"\[quote=(.+?)\]", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\[/quote\]", string.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return str;
        }

        #endregion
    }
}
