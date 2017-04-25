using System.Collections.Generic;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    ///产品属性映射
    /// </summary>
    public partial class ProductAttributeMapping : BaseEntity, ILocalizedEntity
    {
        private ICollection<ProductAttributeValue> _productAttributeValues;

        /// <summary>
        /// 获取或设置产品标识符
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 获取或设置产品属性标识符
        /// </summary>
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// 获取或设置一个文本提示值
        /// </summary>
        public string TextPrompt { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示实体是否是必需的
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        ///获取或设置属性控件类型标识符
        /// </summary>
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// 获取或设置显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        //验证字段

        /// <summary>
        /// 获取或设置最小长度的验证规则（对于文本框和多行文本框）
        /// </summary>
        public int? ValidationMinLength { get; set; }

        /// <summary>
        /// 获取或设置最大长度的验证规则（对于文本框和多行文本框）
        /// </summary>
        public int? ValidationMaxLength { get; set; }

        /// <summary>
        /// 获取或设置文件允许扩展的验证规则（文件上传）
        /// </summary>
        public string ValidationFileAllowedExtensions { get; set; }

        /// <summary>
        ///获取或设置文件最大大小（KB）的验证规则（文件上传）
        /// </summary>
        public int? ValidationFileMaximumSize { get; set; }

        /// <summary>
        ///获取或设置默认值（对于文本框和多行文本框）
        /// </summary>
        public string DefaultValue { get; set; }



        /// <summary>
        ///当启用此属性（可见）时，获取或设置条件（取决于其他属性）。
        ///留空（或null）启用此属性。
        /// Conditional attributes that only appear if a previous attribute is selected, such as having an option 
        /// for personalizing clothing with a name and only providing the text input box if the "Personalize" radio button is checked.
        /// </summary>
        public string ConditionAttributeXml { get; set; }



        /// <summary>
        /// 获取属性控件类型
        /// </summary>
        public AttributeControlType AttributeControlType
        {
            get
            {
                return (AttributeControlType)this.AttributeControlTypeId;
            }
            set
            {
                this.AttributeControlTypeId = (int)value; 
            }
        }

        /// <summary>
        ///获取产品属性
        /// </summary>
        public virtual ProductAttribute ProductAttribute { get; set; }

        /// <summary>
        /// 获取产品
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// 获取产品属性值
        /// </summary>
        public virtual ICollection<ProductAttributeValue> ProductAttributeValues
        {
            get { return _productAttributeValues ?? (_productAttributeValues = new List<ProductAttributeValue>()); }
            protected set { _productAttributeValues = value; }
        }

    }

}
