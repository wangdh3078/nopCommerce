using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Nop.Core.Html
{
    /// <summary>
    /// ResolveLinks������
    /// </summary>
    public partial class ResolveLinksHelper
    {
        #region �ֶ�
        /// <summary>
        /// ���ڽ������ӵ�������ʽ��
        /// </summary>
        private static readonly Regex regex = new Regex("((http://|https://|www\\.)([A-Z0-9.\\-]{1,})\\.[0-9A-Z?;~&\\(\\)#,=\\-_\\./\\+]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string link = "<a href=\"{0}{1}\" rel=\"nofollow\">{2}</a>";
        private const int MAX_LENGTH = 50;
        #endregion

        #region ����
        /// <summary>
        /// ��ʽ���ı�
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <returns>��ʽ������ı�</returns>
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
        /// ���κξ���URL���̵�ָ������󳤶�
        /// </summary>
        private static string ShortenUrl(string url, int max)
        {
            if (url.Length <= max)
                return url;

            // ɾ��Э��
            int startIndex = url.IndexOf("://");
            if (startIndex > -1)
                url = url.Substring(startIndex + 3);

            if (url.Length <= max)
                return url;

            // ѹ���ļ��нṹ
            int firstIndex = url.IndexOf("/") + 1;
            int lastIndex = url.LastIndexOf("/");
            if (firstIndex < lastIndex)
            {
                url = url.Remove(firstIndex, lastIndex - firstIndex);
                url = url.Insert(firstIndex, "...");
            }

            if (url.Length <= max)
                return url;

            // ɾ����ַ����
            int queryIndex = url.IndexOf("?");
            if (queryIndex > -1)
                url = url.Substring(0, queryIndex);

            if (url.Length <= max)
                return url;

            // ɾ��URLƬ��
            int fragmentIndex = url.IndexOf("#");
            if (fragmentIndex > -1)
                url = url.Substring(0, fragmentIndex);

            if (url.Length <= max)
                return url;

            // ѹ��ҳ��
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
