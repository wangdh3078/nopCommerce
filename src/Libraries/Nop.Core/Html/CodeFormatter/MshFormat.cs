
namespace Nop.Core.Html.CodeFormatter
{
    /// <summary>
    /// 从MSH（代码名称Monad）源代码生成颜色编码的HTML 4.01。
    /// </summary>
    public partial class MshFormat : CodeFormat
	{
        /// <summary>
        ///正则表达式字符串，以匹配单行注释（＃）。
        /// </summary>
        protected override string CommentRegex
		{
			get { return @"#.*?(?=\r|\n)"; }
		}

        /// <summary>
        /// 正则表达式字符串，以匹配字符串和字符文字。
        /// </summary>
        protected override string StringRegex
		{
			get { return @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'"; }
		}

        /// <summary>
        /// MSH关键字列表。
        /// </summary>
        protected override string Keywords 
		{
			get 
			{ 
				return "function filter global script local private if else"
					+ " elseif for foreach in while switch continue break"
					+ " return default param begin process end throw trap";
			}
		}

        /// <summary>
        /// 使用预处理器属性来突出显示运算符。
        /// </summary>
        protected override string Preprocessors
		{
			get
			{
				return "-band -bor -match -notmatch -like -notlike -eq -ne"
					+ " -gt -ge -lt -le -is -imatch -inotmatch -ilike"
					+ " -inotlike -ieq -ine -igt -ige -ilt -ile";
			}
		}

	}
}
