namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// ���Կؼ�����
    /// </summary>
    public enum AttributeControlType
    {
        /// <summary>
        ///�����б�
        /// </summary>
        DropdownList = 1,
        /// <summary>
        /// ��ѡ��ť
        /// </summary>
        RadioList = 2,
        /// <summary>
        /// ��ѡ��
        /// </summary>
        Checkboxes = 3,
        /// <summary>
        /// �ı���
        /// </summary>
        TextBox = 4,
        /// <summary>
        /// �����ı���
        /// </summary>
        MultilineTextbox = 10,
        /// <summary>
        /// ʱ��ؼ�
        /// </summary>
        Datepicker = 20,
        /// <summary>
        /// �ļ��ϴ��ؼ�
        /// </summary>
        FileUpload = 30,
        /// <summary>
        /// ��ɫѡ��ؼ�
        /// </summary>
        ColorSquares = 40,
        /// <summary>
        /// ͼƬ
        /// </summary>
        ImageSquares = 45,
        /// <summary>
        /// ֻ����ѡ��
        /// </summary>
        ReadonlyCheckboxes = 50,
    }
}