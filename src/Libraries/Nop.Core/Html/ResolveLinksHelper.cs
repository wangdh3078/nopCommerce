using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nop.Core.Html
{
    /// <summary>
    /// ResolveLinks帮助类
    /// </summary>
    public partial class ResolveLinksHelper
    {
        #region 字段
        /// <summary>
        /// 用于解析链接的正则表达式。
        /// </summary>
        private static readonly Regex regex = new Regex("((http://|https://|www\\.)([A-Z0-9.\\-]{1,})\\.[0-9A-Z?;~&\\(\\)#,=\\-_\\./\\+]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string link = "<a href=\"{0}{1}\" rel=\"nofollow\">{2}</a>";
        private const int MAX_LENGTH = 50;
        #endregion

        #region 方法
        /// <summary>
        /// 格式化文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>格式化后的文本</returns>
        public static string FormatText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var info = CultureInfo.InvariantCulture;
            foreach (Match match in regex.Matches(text))
            {
                if (!match.Value.Contains("://"))
                {
                    text = text.Replace(match.Value, string.Format(info, link, "http://", match.Value, ShortenUrl(match.Value, MAX_LENGTH)));
                }
                else
                {
                    text = text.Replace(match.Value, string.Format(info, link, string.Empty, match.Value, ShortenUrl(match.Value, MAX_LENGTH)));
                }
            }

            return text;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// 将任何绝对URL缩短到指定的最大长度
        /// </summary>
        private static string ShortenUrl(string url, int max)
        {
            if (url.Length <= max)
                return url;

            // 删除协议
            int startIndex = url.IndexOf("://");
            if (startIndex > -1)
                url = url.Substring(startIndex + 3);

            if (url.Length <= max)
                return url;

            // 压缩文件夹结构
            int firstIndex = url.IndexOf("/") + 1;
            int lastIndex = url.LastIndexOf("/");
            if (firstIndex < lastIndex)
            {
                url = url.Remove(firstIndex, lastIndex - firstIndex);
                url = url.Insert(firstIndex, "...");
            }

            if (url.Length <= max)
                return url;

            // 删除网址参数
            int queryIndex = url.IndexOf("?");
            if (queryIndex > -1)
                url = url.Substring(0, queryIndex);

            if (url.Length <= max)
                return url;

            // 删除URL片段
            int fragmentIndex = url.IndexOf("#");
            if (fragmentIndex > -1)
                url = url.Substring(0, fragmentIndex);

            if (url.Length <= max)
                return url;

            // 压缩页面
            firstIndex = url.LastIndexOf("/") + 1;
            lastIndex = url.LastIndexOf(".");
            if (lastIndex - firstIndex > 10)
            {
                string page = url.Substring(firstIndex, lastIndex - firstIndex);
                int length = url.Length - max + 3;
                if (page.Length > length)
                    url = url.Replace(page, "..." + page.Substring(length));
            }

            return url;
        }
        #endregion

        
    }
}
