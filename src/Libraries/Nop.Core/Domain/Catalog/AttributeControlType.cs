namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 属性控件类型
    /// </summary>
    public enum AttributeControlType
    {
        /// <summary>
        ///下拉列表
        /// </summary>
        DropdownList = 1,
        /// <summary>
        /// 单选按钮
        /// </summary>
        RadioList = 2,
        /// <summary>
        /// 复选框
        /// </summary>
        Checkboxes = 3,
        /// <summary>
        /// 文本框
        /// </summary>
        TextBox = 4,
        /// <summary>
        /// 多行文本框
        /// </summary>
        MultilineTextbox = 10,
        /// <summary>
        /// 时间控件
        /// </summary>
        Datepicker = 20,
        /// <summary>
        /// 文件上传控件
        /// </summary>
        FileUpload = 30,
        /// <summary>
        /// 颜色选择控件
        /// </summary>
        ColorSquares = 40,
        /// <summary>
        /// 图片
        /// </summary>
        ImageSquares = 45,
        /// <summary>
        /// 只读复选框
        /// </summary>
        ReadonlyCheckboxes = 50,
    }
}