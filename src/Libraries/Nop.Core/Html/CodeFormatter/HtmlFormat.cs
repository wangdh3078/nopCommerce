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
    /// 从HTML / XML / ASPX源代码生成颜色编码的HTML 4.01。
    /// </summary>
    /// <remarks>
    /// <para>
    ///此实现假定＆lt; script＆gt;中的代码 块是JavaScript，＆lt;％％＆gt;内的代码 块是C＃。</para>
    /// <para>
    /// 此类中的默认选项卡宽度设置为2个字符。</para>
    /// </remarks>
    public partial class HtmlFormat : SourceFormat
	{
		private readonly CSharpFormat csf; //格式化嵌入式C＃代码
        private readonly JavaScriptFormat jsf; //格式化客户端JavaScript代码
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

            //正则表达式对象将在一次通过中处理所有替换
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
        ///用于评估与代码中每个属性名称/值相对应的HTML片段。
        /// </summary>
        /// <param name="match">从单个正则表达式匹配得到的<see cref =“Match”/>。</param>
        /// <returns>包含HTML代码片段的字符串。</returns>
        private string AttributeMatchEval(Match match)
		{
			if(match.Groups[1].Success) //attribute value
				return "<span class=\"kwrd\">" + match.ToString() + "</span>";

			if(match.Groups[2].Success) //attribute name
				return "<span class=\"attr\">" + match.ToString() + "</span>";

			return match.ToString();
		}

        /// <summary>
        /// 用于评估代码中与每个匹配令牌相对应的HTML片段。
        /// </summary>
        /// <param name="match">从单个正则表达式匹配得到的<see cref =“Match”/>。</param>
        /// <returns>包含HTML代码片段的字符串。</returns>
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

