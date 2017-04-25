namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// ��Ʒ�������
    /// </summary>
    public partial class ProductAttributeCombination : BaseEntity
    {
        /// <summary>
        /// ��ȡ�����ò�Ʒ��ʶ��
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public string AttributesXml { get; set; }

        /// <summary>
        ///��ȡ�����ÿ������
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ���ȱ��ʱ������
        /// </summary>
        public bool AllowOutOfStockOrders { get; set; }

        /// <summary>
        /// ��ȡ������SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        ///��ȡ������������������
        /// </summary>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        ///��ȡ������ȫ��ó����Ŀ��ţ�GTIN���� ��Щ��ʶ������UPC����������EAN��ŷ�ޣ���JAN���ձ�����ISBN�������鼮����
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        /// ��ȡ������������ϼ۸� �������������������ӵ����ﳵʱ���������Ը���Ĭ�ϲ�Ʒ�۸� ���磬�����������ַ�ʽ�����ۿۡ�
        /// </summary>
        public decimal? OverriddenPrice { get; set; }

        /// <summary>
        /// ��֪ͨ����Աʱ��ȡ����������
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// ��ȡ��Ʒ
        /// </summary>
        public virtual Product Product { get; set; }

    }
}
