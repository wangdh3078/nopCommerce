using System;
using System.Text.RegularExpressions;
using Nop.Core.Domain.Common;
using Nop.Core.Html.CodeFormatter;
using Nop.Core.Infrastructure;

namespace Nop.Core.Html
{
    /// <summary>
    /// ����һ��BBCode������
    /// </summary>
    public partial class BBCodeHelper
    {
        #region �ֶ�
        private static readonly Regex regexBold = new Regex(@"\[b\](.+?)\[/b\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexItalic = new Regex(@"\[i\](.+?)\[/i\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUnderLine = new Regex(@"\[u\](.+?)\[/u\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl1 = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexUrl2 = new Regex(@"\[url\](.+?)\[/url\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexQuote = new Regex(@"\[quote=(.+?)\](.+?)\[/quote\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex regexImg = new Regex(@"\[img\](.+?)\[/img\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region ����
        /// <summary>
        /// ��ʽ���ı�
        /// </summary>
        /// <param name="text">�ı�</param>
        /// <param name="replaceBold">ָʾ�Ƿ��滻�����ֵ</param>
        /// <param name="replaceItalic">ָʾ�Ƿ��滻б���ֵ</param>
        /// <param name="replaceUnderline">ָʾ�Ƿ��滻�»��ߵ�ֵ</param>
        /// <param name="replaceUrl">ָʾ�Ƿ��滻URL��ֵ</param>
        /// <param name="replaceCode">ָʾ�Ƿ��滻�����ֵ</param>
        /// <param name="replaceQuote">һ��ֵ��ʾ�Ƿ��滻����</param>
        /// <param name="replaceImg">ָʾ�Ƿ��滻Img��ֵ</param>
        /// <returns>��ʽ������ı�</returns>
        public static string FormatText(string text, bool replaceBold, bool replaceItalic,
            bool replaceUnderline, bool replaceUrl, bool replaceCode, bool replaceQuote, bool replaceImg)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (replaceBold)
            {
                //��ʽ�������ǩ: [b][/b]
                // ��Ϊ: <strong></strong>
                text = regexBold.Replace(text, "<strong>$1</strong>");
            }

            if (replaceItalic)
            {
                // ��ʽ��б���ǩ: [i][/i]
                // ��Ϊ: <em></em>
                text = regexItalic.Replace(text, "<em>$1</em>");
            }

            if (replaceUnderline)
            {
                // ��ʽ�»��߱�ǩ: [u][/u]
                // ��Ϊ: <u></u>
                text = regexUnderLine.Replace(text, "<u>$1</u>");
            }

            if (replaceUrl)
            {
                var newWindow = EngineContext.Current.Resolve<CommonSettings>().BbcodeEditorOpenLinksInNewWindow;
                // ��ʽ��URL��ǩ: [url=http://www.nopCommerce.com]my site[/url]
                // ��Ϊ: <a href="http://www.nopCommerce.com">my site</a>
                text = regexUrl1.Replace(text, string.Format("<a href=\"$1\" rel=\"nofollow\"{0}>$2</a>", newWindow ? " target=_blank" : ""));

                // ��ʽ��URL��ǩ: [url]http://www.nopCommerce.com[/url]
                // ��Ϊ: <a href="http://www.nopCommerce.com">http://www.nopCommerce.com</a>
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
                // ��Ϊ: <img src="http://www.nopCommerce.com/Content/Images/Image.jpg">
                text = regexImg.Replace(text, "<img src=\"$1\" class=\"user-posted-image\" alt=\"\">");
            }
            return text;
        }

        /// <summary>
        /// ���ַ�����ɾ����������
        /// </summary>
        /// <param name="str">�ַ���</param>
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
