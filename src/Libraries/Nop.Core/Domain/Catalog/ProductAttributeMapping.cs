using System.Collections.Generic;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    ///��Ʒ����ӳ��
    /// </summary>
    public partial class ProductAttributeMapping : BaseEntity, ILocalizedEntity
    {
        private ICollection<ProductAttributeValue> _productAttributeValues;

        /// <summary>
        /// ��ȡ�����ò�Ʒ��ʶ��
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ��ȡ�����ò�Ʒ���Ա�ʶ��
        /// </summary>
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// ��ȡ������һ���ı���ʾֵ
        /// </summary>
        public string TextPrompt { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾʵ���Ƿ��Ǳ����
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        ///��ȡ���������Կؼ����ͱ�ʶ��
        /// </summary>
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// ��ȡ��������ʾ˳��
        /// </summary>
        public int DisplayOrder { get; set; }

        //��֤�ֶ�

        /// <summary>
        /// ��ȡ��������С���ȵ���֤���򣨶����ı���Ͷ����ı���
        /// </summary>
        public int? ValidationMinLength { get; set; }

        /// <summary>
        /// ��ȡ��������󳤶ȵ���֤���򣨶����ı���Ͷ����ı���
        /// </summary>
        public int? ValidationMaxLength { get; set; }

        /// <summary>
        /// ��ȡ�������ļ�������չ����֤�����ļ��ϴ���
        /// </summary>
        public string ValidationFileAllowedExtensions { get; set; }

        /// <summary>
        ///��ȡ�������ļ�����С��KB������֤�����ļ��ϴ���
        /// </summary>
        public int? ValidationFileMaximumSize { get; set; }

        /// <summary>
        ///��ȡ������Ĭ��ֵ�������ı���Ͷ����ı���
        /// </summary>
        public string DefaultValue { get; set; }



        /// <summary>
        ///�����ô����ԣ��ɼ���ʱ����ȡ������������ȡ�����������ԣ���
        ///���գ���null�����ô����ԡ�
        /// Conditional attributes that only appear if a previous attribute is selected, such as having an option 
        /// for personalizing clothing with a name and only providing the text input box if the "Personalize" radio button is checked.
        /// </summary>
        public string ConditionAttributeXml { get; set; }



        /// <summary>
        /// ��ȡ���Կؼ�����
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
        ///��ȡ��Ʒ����
        /// </summary>
        public virtual ProductAttribute ProductAttribute { get; set; }

        /// <summary>
        /// ��ȡ��Ʒ
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// ��ȡ��Ʒ����ֵ
        /// </summary>
        public virtual ICollection<ProductAttributeValue> ProductAttributeValues
        {
            get { return _productAttributeValues ?? (_productAttributeValues = new List<ProductAttributeValue>()); }
            protected set { _productAttributeValues = value; }
        }

    }

}
