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

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Nop.Core.Html.CodeFormatter
{
    /// <summary>
    ///	为所有代码格式化程序提供基础实现。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 要在网站上显示格式化的代码，网页必须引用一个样式表，
    /// 该样式表定义了CSharpFormat生成的不同CSS类的格式：
    /// .csharpcode，pre，.rem，.kwrd，.str，.op，.preproc ，.alt，.lnum。
    /// </para>
    /// <para>
    /// 请注意，如果您的源代码中有多行注释（如/ * ... * /），
    /// “行号”或“替代行背景”选项将生成不严格符合HTML 4.01的代码。
    /// IE5 +或Mozilla 0.8+的代码仍然会很好。
    /// </para>
    /// </remarks>
    public abstract partial class SourceFormat
	{
		/// <summary/>
		protected SourceFormat()
		{
			_tabSpaces = 4;
			_lineNumbers = false;
			_alternate = false;
			_embedStyleSheet = false;
		}

		private byte _tabSpaces;

        /// <summary>
        /// 获取或设置选项卡宽度。
        /// </summary>
        /// <value>用于替换制表符字符的空格字符数。 
        /// 默认值为<b> 4 </ b>，除非重载是派生类。</value>
        public byte TabSpaces
		{
			get { return _tabSpaces; }
			set { _tabSpaces = value; }
		}

		private bool _lineNumbers;

        /// <summary>
        /// 在输出中启用或禁用行号。
        /// </summary>
        /// <value>当<b> true </ b>时，会生成行号。
        /// 默认值为<b> false </ b>。</value>
        public bool LineNumbers
		{
			get { return _lineNumbers; }
			set { _lineNumbers = value; }
		}

		private bool _alternate;

        /// <summary>
        /// 启用或禁用交替的行背景。
        /// </summary>
        /// <value>当<b> true </ b>时，行背景是交替的。
        /// 默认值为<b> false </ b>。</value>
        public bool Alternate
		{
			get { return _alternate; }
			set { _alternate = value; }
		}

		private bool _embedStyleSheet;

        /// <summary>
        /// 启用或禁用嵌入式CSS样式表。
        /// </summary>
        /// <value>当<b> true </ b>时，CSS <style>元素包含在HTML输出中。
        /// 默认值为<b> false </ b>。</value>
        public bool EmbedStyleSheet
		{
			get { return _embedStyleSheet; }
			set { _embedStyleSheet = value; }
		}

        /// <overloads>将源代码转换为HTML 4.01。</overloads>
        /// 
        /// <summary>
        /// 将源代码流转换为HTML 4.01。
        /// </summary>
        /// <param name="source">源代码流。</param>
        /// <returns>包含HTML格式代码的字符串。</returns>
        public string FormatCode(Stream source)
        {
            using (var reader = new StreamReader(source))
            { 
                string s = reader.ReadToEnd();            
			    return FormatCode(s, _lineNumbers, _alternate, _embedStyleSheet, false);
            }
        }

        /// <summary>
        /// 将源代码字符串转换为HTML 4.01。
        /// </summary>
        /// <returns>包含HTML格式代码的字符串。</returns>
        public string FormatCode(string source)
		{
			return FormatCode(source, _lineNumbers, _alternate, _embedStyleSheet, false);
		}

        /// <summary>
        /// 允许使用其他语言格式化部分代码，
        /// 例如HTML文件中的JavaScript块。
        /// </summary>
        public string FormatSubCode(string source)
		{
			return FormatCode(source, false, false, false, true);
		}

        /// <summary>
        /// 获取CSS样式表作为流。
        /// </summary>
        /// <returns>一个文本<see cref =“Stream”/>的CSS定义。</returns>
        public static Stream GetCssStream()
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"Manoli.Utils.CSharpFormat.csharp.css");
		}

        /// <summary>
        /// 获取CSS样式表作为字符串。
        /// </summary>
        /// <returns>包含CSS定义的字符串。</returns>
        public static string GetCssString()
		{
            using (var reader = new StreamReader(GetCssStream()))
            {
                return reader.ReadToEnd();
            }
		}

		private Regex codeRegex;

        /// <summary>
        /// 用于捕获语言令牌的正则表达式。
        /// </summary>
        protected Regex CodeRegex
		{
			get { return codeRegex; }
			set { codeRegex = value; }
		}

        /// <summary>
        /// 用于评估代码中与每个匹配令牌相对应的HTML片段。
        /// </summary>
        /// <param name="match">从单个正则表达式匹配得到的<see cref =“Match”/>。</param>
        /// <returns>包含HTML代码片段的字符串。</returns>
        protected abstract string MatchEval(Match match);

		//does the formatting job
		private string FormatCode(string source, bool lineNumbers, 
			bool alternate, bool embedStyleSheet, bool subCode)
		{
			//replace special characters
            var sb = new StringBuilder(source);

			if(!subCode)
			{
				sb.Replace("&", "&amp;");
				sb.Replace("<", "&lt;");
				sb.Replace(">", "&gt;");
				sb.Replace("\t", string.Empty.PadRight(_tabSpaces));
			}
			
			//color the code
			source = codeRegex.Replace(sb.ToString(), new MatchEvaluator(this.MatchEval));

			sb = new StringBuilder();
			
			if (embedStyleSheet)
			{
				sb.AppendFormat("<style type=\"{0}\">\n",MimeTypes.TextCss);
				sb.Append(GetCssString());
				sb.Append("</style>\n");
			}

			if (lineNumbers || alternate) //we have to process the code line by line
			{
				if(!subCode)
					sb.Append("<div class=\"csharpcode\">\n");
                var reader = new StringReader(source);
				int i = 0;
				const string spaces = "    ";
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					i++;
					if (alternate && ((i % 2) == 1))
					{
						sb.Append("<pre class=\"alt\">");
					}
					else
					{
						sb.Append("<pre>");
					}

					if(lineNumbers)
					{
						var order = (int)Math.Log10(i);
						sb.Append("<span class=\"lnum\">" 
							+ spaces.Substring(0, 3 - order) + i.ToString() 
							+ ":  </span>");
					}
					
					if(line.Length == 0)
						sb.Append("&nbsp;");
					else
						sb.Append(line);
					sb.Append("</pre>\n");
				}
				reader.Close();
				if(!subCode)
					sb.Append("</div>");
			}
			else
			{
				//have to use a <pre> because IE below ver 6 does not understand 
				//the "white-space: pre" CSS value
				if(!subCode)
					sb.Append("<pre class=\"csharpcode\">\n");
				sb.Append(source);
				if(!subCode)
					sb.Append("</pre>");
			}
			
			return sb.ToString();
		}

	}
}
