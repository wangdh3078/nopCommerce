#region Copyright ?2001-2003 Jean-Claude Manoli [jc@manoli.net]
/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author(s) be held liable for any damages arising from
 * the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 *   1. The origin of this software must not be misrepresented; you must not
 *      claim that you wrote the original software. If you use this software
 *      in a product, an acknowledgment in the product documentation would be
 *      appreciated but is not required.
 * 
 *   2. Altered source versions must be plainly marked as such, and must not
 *      be misrepresented as being the original software.
 * 
 *   3. This notice may not be removed or altered from any source distribution.
 */ 
#endregion

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Nop.Core.Html.CodeFormatter
{
    /// <summary>
    /// ��HTML / XML / ASPXԴ����������ɫ�����HTML 4.01��
    /// </summary>
    /// <remarks>
    /// <para>
    ///��ʵ�ּٶ���lt; script��gt;�еĴ��� ����JavaScript����lt;������gt;�ڵĴ��� ����C����</para>
    /// <para>
    /// �����е�Ĭ��ѡ��������Ϊ2���ַ���</para>
    /// </remarks>
    public partial class HtmlFormat : SourceFormat
	{
		private readonly CSharpFormat csf; //��ʽ��Ƕ��ʽC������
        private readonly JavaScriptFormat jsf; //��ʽ���ͻ���JavaScript����
        private readonly Regex attribRegex;

		/// <summary/>
		public HtmlFormat()
		{
			const string regJavaScript = @"(?<=&lt;script(?:\s.*?)?&gt;).+?(?=&lt;/script&gt;)";
			const string regComment = @"&lt;!--.*?--&gt;";
			const string regAspTag = @"&lt;%@.*?%&gt;|&lt;%|%&gt;";
			const string regAspCode = @"(?<=&lt;%).*?(?=%&gt;)";
			const string regTagDelimiter = @"(?:&lt;/?!?\??(?!%)|(?<!%)/?&gt;)+";
			const string regTagName = @"(?<=&lt;/?!?\??(?!%))[\w\.:-]+(?=.*&gt;)";
			const string regAttributes = @"(?<=&lt;(?!%)/?!?\??[\w:-]+).*?(?=(?<!%)/?&gt;)";
			const string regEntity = @"&amp;\w+;";
			const string regAttributeMatch = @"(=?"".*?""|=?'.*?')|([\w:-]+)";

            //������ʽ������һ��ͨ���д��������滻
            string regAll = "(" + regJavaScript + ")|(" + regComment + ")|(" 
				+ regAspTag + ")|(" + regAspCode + ")|(" 
				+ regTagDelimiter + ")|(" + regTagName + ")|("
				+ regAttributes + ")|(" + regEntity + ")";

			CodeRegex = new Regex(regAll, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			attribRegex = new Regex(regAttributeMatch, RegexOptions.Singleline);

			csf = new CSharpFormat();
			jsf = new JavaScriptFormat();
		}

        /// <summary>
        ///���������������ÿ����������/ֵ���Ӧ��HTMLƬ�Ρ�
        /// </summary>
        /// <param name="match">�ӵ���������ʽƥ��õ���<see cref =��Match��/>��</param>
        /// <returns>����HTML����Ƭ�ε��ַ�����</returns>
        private string AttributeMatchEval(Match match)
		{
			if(match.Groups[1].Success) //attribute value
				return "<span class=\"kwrd\">" + match.ToString() + "</span>";

			if(match.Groups[2].Success) //attribute name
				return "<span class=\"attr\">" + match.ToString() + "</span>";

			return match.ToString();
		}

        /// <summary>
        /// ����������������ÿ��ƥ���������Ӧ��HTMLƬ�Ρ�
        /// </summary>
        /// <param name="match">�ӵ���������ʽƥ��õ���<see cref =��Match��/>��</param>
        /// <returns>����HTML����Ƭ�ε��ַ�����</returns>
        protected override string MatchEval(Match match)
		{
			if(match.Groups[1].Success) //JavaScript code
			{
				//string s = match.ToString();
				return jsf.FormatSubCode(match.ToString());
			}
			if(match.Groups[2].Success) //comment
			{
                var reader = new StringReader(match.ToString());
				string line;
                var sb = new StringBuilder();
				while ((line = reader.ReadLine()) != null)
				{
					if(sb.Length > 0)
					{
						sb.Append("\n");
					}
					sb.Append("<span class=\"rem\">");
					sb.Append(line);
					sb.Append("</span>");
				}
				return sb.ToString();
			}
			if(match.Groups[3].Success) //asp tag
			{
				return "<span class=\"asp\">" + match.ToString() + "</span>";
			}
			if(match.Groups[4].Success) //asp C# code
			{
				return csf.FormatSubCode(match.ToString());
			}
			if(match.Groups[5].Success) //tag delimiter
			{
				return "<span class=\"kwrd\">" + match.ToString() + "</span>";
			}
			if(match.Groups[6].Success) //html tagname
			{
				return "<span class=\"html\">" + match.ToString() + "</span>";
			}
			if(match.Groups[7].Success) //attributes
			{
				return attribRegex.Replace(match.ToString(), 
					new MatchEvaluator(this.AttributeMatchEval));
			}
			if(match.Groups[8].Success) //entity
			{
				return "<span class=\"attr\">" + match.ToString() + "</span>";
			}
			return match.ToString();
		}
	}
}

