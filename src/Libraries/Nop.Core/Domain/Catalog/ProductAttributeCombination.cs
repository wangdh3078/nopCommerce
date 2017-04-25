namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 产品属性组合
    /// </summary>
    public partial class ProductAttributeCombination : BaseEntity
    {
        /// <summary>
        /// 获取或设置产品标识符
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 获取或设置属性
        /// </summary>
        public string AttributesXml { get; set; }

        /// <summary>
        ///获取或设置库存数量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否在缺货时允许订单
        /// </summary>
        public bool AllowOutOfStockOrders { get; set; }

        /// <summary>
        /// 获取或设置SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        ///获取或设置制造商零件编号
        /// </summary>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        ///获取或设置全球贸易项目编号（GTIN）。 这些标识符包括UPC（北美），EAN（欧洲），JAN（日本）和ISBN（用于书籍）。
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        /// 获取或设置属性组合价格。 这样，当该属性组合添加到购物车时，店主可以覆盖默认产品价格。 例如，您可以以这种方式给予折扣。
        /// </summary>
        public decimal? OverriddenPrice { get; set; }

        /// <summary>
        /// 当通知管理员时获取或设置数量
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// 获取产品
        /// </summary>
        public virtual Product Product { get; set; }

    }
}
