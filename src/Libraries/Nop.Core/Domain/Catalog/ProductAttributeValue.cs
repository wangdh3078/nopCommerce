using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// ��Ʒ����ֵ
    /// </summary>
    public partial class ProductAttributeValue : BaseEntity, ILocalizedEntity
    {
        /// <summary>
        /// ��ȡ�����ò�Ʒ����ӳ���ʶ��
        /// </summary>
        public int ProductAttributeMappingId { get; set; }

        /// <summary>
        ///��ȡ����������ֵ���ͱ�ʶ��
        /// </summary>
        public int AttributeValueTypeId { get; set; }

        /// <summary>
        /// ��ȡ�����ù����Ĳ�Ʒ��ʶ��������AttributeValueType.AssociatedToProductһ��ʹ�ã�
        /// </summary>
        public int AssociatedProductId { get; set; }

        /// <summary>
        /// ��ȡ�����ò�Ʒ��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��ȡ��������ɫRGBֵ����"Color squares" ��������һ��ʹ�ã�
        /// </summary>
        public string ColorSquaresRgb { get; set; }

        /// <summary>
        /// Gets or sets the picture ID for image square (used with "Image squares" attribute type)
        /// </summary>
        public int ImageSquaresPictureId { get; set; }

        /// <summary>
        /// ��ȡ�����ü۸����������AttributeValueType.Simpleһ��ʹ�ã�
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// ��ȡ������Ȩ�ص���������AttributeValueType.Simpleһ��ʹ�ã�
        /// </summary>
        public decimal WeightAdjustment { get; set; }

        /// <summary>
        /// ��ȡ����������ֵcost������Attribute Value Type.Simpleһ��ʹ�ã�
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ�ͻ��Ƿ�������������Ʒ������������AttributeValueType.AssociatedToProductһ��ʹ�ã�
        /// </summary>
        public bool CustomerEntersQty { get; set; }

        /// <summary>
        /// ��ȡ�����ù�����Ʒ����������������ֵ����������Ĳ�Ʒʹ�ã�
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��ֵ�Ƿ���Ԥѡ
        /// </summary>
        public bool IsPreSelected { get; set; }

        /// <summary>
        /// ��ȡ��������ʾ˳��
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// ��ȡ���������ֵ�������ͼƬ����ʶ������ �����ѡ�񣩺󣬴�ͼƬ���滻��Ʒ��ͼƬ��
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        ///��ȡ��Ʒ����ӳ��
        /// </summary>
        public virtual ProductAttributeMapping ProductAttributeMapping { get; set; }

        /// <summary>
        /// ��ȡ����������ֵ����
        /// </summary>
        public AttributeValueType AttributeValueType
        {
            get
            {
                return (AttributeValueType)this.AttributeValueTypeId;
            }
            set
            {
                this.AttributeValueTypeId = (int)value;
            }
        }
    }
}
