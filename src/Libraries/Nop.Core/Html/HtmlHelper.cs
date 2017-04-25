using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Nop.Core.Html
{
    /// <summary>
    /// HTML帮助类
    /// </summary>
    public partial class HtmlHelper
    {
        #region 字段
        private readonly static Regex paragraphStartRegex = new Regex("<p>", RegexOptions.IgnoreCase);
        private readonly static Regex paragraphEndRegex = new Regex("</p>", RegexOptions.IgnoreCase);
        //private static Regex ampRegex = new Regex("&(?!(?:#[0-9]{2,4};|[a-z0-9]+;))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Methods

        /// <summary>
        /// 格式化文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="stripTags">指示是否剥离标签的值</param>
        /// <param name="convertPlainTextToHtml">A value indicating whether HTML is allowed</param>
        /// <param name="allowHtml">指示是否允许HTML的值</param>
        /// <param name="allowBBCode">指示是否允许BBCode的值</param>
        /// <param name="resolveLinks">指示是否解析链接的值</param>
        /// <param name="addNoFollowTag">指示是否添加“noFollow”标签的值</param>
        /// <returns>格式化后的文本</returns>
        public static string FormatText(string text, bool stripTags,
            bool convertPlainTextToHtml, bool allowHtml,
            bool allowBBCode, bool resolveLinks, bool addNoFollowTag)
        {

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            try
            {
                if (stripTags)
                {
                    text = StripTags(text);
                }

                text = allowHtml ? EnsureOnlyAllowedHtml(text) : HttpUtility.HtmlEncode(text);

                if (convertPlainTextToHtml)
                {
                    text = ConvertPlainTextToHtml(text);
                }

                if (allowBBCode)
                {
                    text = BBCodeHelper.FormatText(text, true, true, true, true, true, true, true);
                }

                if (resolveLinks)
                {
                    text = ResolveLinksHelper.FormatText(text);
                }

                if (addNoFollowTag)
                {
                    //add noFollow tag. not implemented
                }
            }
            catch (Exception exc)
            {
                text = string.Format("Text cannot be formatted. Error: {0}", exc.Message);
            }
            return text;
        }

        /// <summary>
        /// Strips标签
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>格式化后的文本</returns>
        public static string StripTags(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, @"(>)(\r|\n)*(<)", "><");
            text = Regex.Replace(text, "(<[^>]*>)([^<]*)", "$2");
            text = Regex.Replace(text, "(&#x?[0-9]{2,4};|&quot;|&amp;|&nbsp;|&lt;|&gt;|&euro;|&copy;|&reg;|&permil;|&Dagger;|&dagger;|&lsaquo;|&rsaquo;|&bdquo;|&rdquo;|&ldquo;|&sbquo;|&rsquo;|&lsquo;|&mdash;|&ndash;|&rlm;|&lrm;|&zwj;|&zwnj;|&thinsp;|&emsp;|&ensp;|&tilde;|&circ;|&Yuml;|&scaron;|&Scaron;)", "@");

            return text;
        }

        /// <summary>
        ///替换锚文本（从以下URL <a href="http://example.com">名称</a>中删除标记，并仅输出字符串“名称”）
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static string ReplaceAnchorTags(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Regex.Replace(text, @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1", RegexOptions.IgnoreCase);
            return text;
        }

        /// <summary>
        ///将纯文本转换为HTML
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>格式化后的文本</returns>
        public static string ConvertPlainTextToHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\r", "<br />");
            text = text.Replace("\n", "<br />");
            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");

            return text;
        }

        /// <summary>
        ///将HTML转换为纯文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="decode">指示是否解码文本的值</param>
        /// <param name="replaceAnchorTags">指示是否替换锚文本的值（从以下URL <a href="http://example.com">名称</a>中删除标记，并仅输出字符串“名称”）</param>
        /// <returns>格式化后的文本</returns>
        public static string ConvertHtmlToPlainText(string text,
            bool decode = false, bool replaceAnchorTags = false)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (decode)
                text = HttpUtility.HtmlDecode(text);

            text = text.Replace("<br>", "\n");
            text = text.Replace("<br >", "\n");
            text = text.Replace("<br />", "\n");
            text = text.Replace("&nbsp;&nbsp;", "\t");
            text = text.Replace("&nbsp;&nbsp;", "  ");

            if (replaceAnchorTags)
                text = ReplaceAnchorTags(text);

            return text;
        }

        /// <summary>
        /// 将文字转换为段落
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>格式化后的文本</returns>
        public static string ConvertPlainTextToParagraph(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = paragraphStartRegex.Replace(text, string.Empty);
            text = paragraphEndRegex.Replace(text, "\n");
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text = text + "\n\n";
            text = text.Replace("\n\n", "\n");
            var strArray = text.Split(new[] { '\n' });
            var builder = new StringBuilder();
            foreach (string str in strArray)
            {
                if ((str != null) && (str.Trim().Length > 0))
                {
                    builder.AppendFormat("<p>{0}</p>\n", str);
                }
            }
            return builder.ToString();
        }
        #endregion

        #region Utilities

        private static string EnsureOnlyAllowedHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            const string allowedTags = "br,hr,b,i,u,a,div,ol,ul,li,blockquote,img,span,p,em,strong,font,pre,h1,h2,h3,h4,h5,h6,address,cite";

            var m = Regex.Matches(text, "<.*?>", RegexOptions.IgnoreCase);
            for (int i = m.Count - 1; i >= 0; i--)
            {
                string tag = text.Substring(m[i].Index + 1, m[i].Length - 1).Trim().ToLower();

                if (!IsValidTag(tag, allowedTags))
                {
                    text = text.Remove(m[i].Index, m[i].Length);
                }
            }

            return text;
        }

        private static bool IsValidTag(string tag, string tags)
        {
            string[] allowedTags = tags.Split(',');
            if (tag.IndexOf("javascript") >= 0) return false;
            if (tag.IndexOf("vbscript") >= 0) return false;
            if (tag.IndexOf("onclick") >= 0) return false;

            var endchars = new [] { ' ', '>', '/', '\t' };

            int pos = tag.IndexOfAny(endchars, 1);
            if (pos > 0) tag = tag.Substring(0, pos);
            if (tag[0] == '/') tag = tag.Substring(1);

            foreach (string aTag in allowedTags)
            {
                if (tag == aTag) return true;
            }

            return false;
        }
        #endregion
        
    }
}
