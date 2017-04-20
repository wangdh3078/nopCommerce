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
    ///	Ϊ���д����ʽ�������ṩ����ʵ�֡�
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ҫ����վ����ʾ��ʽ���Ĵ��룬��ҳ��������һ����ʽ��
    /// ����ʽ������CSharpFormat���ɵĲ�ͬCSS��ĸ�ʽ��
    /// .csharpcode��pre��.rem��.kwrd��.str��.op��.preproc ��.alt��.lnum��
    /// </para>
    /// <para>
    /// ��ע�⣬�������Դ�������ж���ע�ͣ���/ * ... * /����
    /// ���кš�������б�����ѡ����ɲ��ϸ����HTML 4.01�Ĵ��롣
    /// IE5 +��Mozilla 0.8+�Ĵ�����Ȼ��ܺá�
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
        /// ��ȡ������ѡ���ȡ�
        /// </summary>
        /// <value>�����滻�Ʊ���ַ��Ŀո��ַ����� 
        /// Ĭ��ֵΪ<b> 4 </ b>�����������������ࡣ</value>
        public byte TabSpaces
		{
			get { return _tabSpaces; }
			set { _tabSpaces = value; }
		}

		private bool _lineNumbers;

        /// <summary>
        /// ����������û�����кš�
        /// </summary>
        /// <value>��<b> true </ b>ʱ���������кš�
        /// Ĭ��ֵΪ<b> false </ b>��</value>
        public bool LineNumbers
		{
			get { return _lineNumbers; }
			set { _lineNumbers = value; }
		}

		private bool _alternate;

        /// <summary>
        /// ���û���ý�����б�����
        /// </summary>
        /// <value>��<b> true </ b>ʱ���б����ǽ���ġ�
        /// Ĭ��ֵΪ<b> false </ b>��</value>
        public bool Alternate
		{
			get { return _alternate; }
			set { _alternate = value; }
		}

		private bool _embedStyleSheet;

        /// <summary>
        /// ���û����Ƕ��ʽCSS��ʽ��
        /// </summary>
        /// <value>��<b> true </ b>ʱ��CSS <style>Ԫ�ذ�����HTML����С�
        /// Ĭ��ֵΪ<b> false </ b>��</value>
        public bool EmbedStyleSheet
		{
			get { return _embedStyleSheet; }
			set { _embedStyleSheet = value; }
		}

        /// <overloads>��Դ����ת��ΪHTML 4.01��</overloads>
        /// 
        /// <summary>
        /// ��Դ������ת��ΪHTML 4.01��
        /// </summary>
        /// <param name="source">Դ��������</param>
        /// <returns>����HTML��ʽ������ַ�����</returns>
        public string FormatCode(Stream source)
        {
            using (var reader = new StreamReader(source))
            { 
                string s = reader.ReadToEnd();            
			    return FormatCode(s, _lineNumbers, _alternate, _embedStyleSheet, false);
            }
        }

        /// <summary>
        /// ��Դ�����ַ���ת��ΪHTML 4.01��
        /// </summary>
        /// <returns>����HTML��ʽ������ַ�����</returns>
        public string FormatCode(string source)
		{
			return FormatCode(source, _lineNumbers, _alternate, _embedStyleSheet, false);
		}

        /// <summary>
        /// ����ʹ���������Ը�ʽ�����ִ��룬
        /// ����HTML�ļ��е�JavaScript�顣
        /// </summary>
        public string FormatSubCode(string source)
		{
			return FormatCode(source, false, false, false, true);
		}

        /// <summary>
        /// ��ȡCSS��ʽ����Ϊ����
        /// </summary>
        /// <returns>һ���ı�<see cref =��Stream��/>��CSS���塣</returns>
        public static Stream GetCssStream()
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"Manoli.Utils.CSharpFormat.csharp.css");
		}

        /// <summary>
        /// ��ȡCSS��ʽ����Ϊ�ַ�����
        /// </summary>
        /// <returns>����CSS������ַ�����</returns>
        public static string GetCssString()
		{
            using (var reader = new StreamReader(GetCssStream()))
            {
                return reader.ReadToEnd();
            }
		}

		private Regex codeRegex;

        /// <summary>
        /// ���ڲ����������Ƶ�������ʽ��
        /// </summary>
        protected Regex CodeRegex
		{
			get { return codeRegex; }
			set { codeRegex = value; }
		}

        /// <summary>
        /// ����������������ÿ��ƥ���������Ӧ��HTMLƬ�Ρ�
        /// </summary>
        /// <param name="match">�ӵ���������ʽƥ��õ���<see cref =��Match��/>��</param>
        /// <returns>����HTML����Ƭ�ε��ַ�����</returns>
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
