
namespace Nop.Core.Html.CodeFormatter
{
    /// <summary>
    /// ��MSH����������Monad��Դ����������ɫ�����HTML 4.01��
    /// </summary>
    public partial class MshFormat : CodeFormat
	{
        /// <summary>
        ///������ʽ�ַ�������ƥ�䵥��ע�ͣ�������
        /// </summary>
        protected override string CommentRegex
		{
			get { return @"#.*?(?=\r|\n)"; }
		}

        /// <summary>
        /// ������ʽ�ַ�������ƥ���ַ������ַ����֡�
        /// </summary>
        protected override string StringRegex
		{
			get { return @"@?""""|@?"".*?(?!\\).""|''|'.*?(?!\\).'"; }
		}

        /// <summary>
        /// MSH�ؼ����б�
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
        /// ʹ��Ԥ������������ͻ����ʾ�������
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
