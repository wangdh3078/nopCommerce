
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Catalog
{
    public class ProductEditorSettings : ISettings
    {
        /// <summary>
        ///获取或设置一个值，指示是否显示“ID”字段
        /// </summary>
        public bool Id { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否显示“产品类型”字段
        /// </summary>
        public bool ProductType { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示 'Visible individually'字段
        /// </summary>
        public bool VisibleIndividually { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“产品模板”字段
        /// </summary>
        public bool ProductTemplate { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“管理员注释”字段
        /// </summary>
        public bool AdminComment { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“供应商”字段
        /// </summary>
        public bool Vendor { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“Stores”字段
        /// </summary>
        public bool Stores { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“ACL”字段
        /// </summary>
        public bool ACL { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否显示“显示在主页”字段
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“显示顺序”字段
        /// </summary>
        public bool DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“允许客户评论”字段
        /// </summary>
        public bool AllowCustomerReviews { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“产品标签”字段
        /// </summary>
        public bool ProductTags { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“制造商零件编号”字段
        /// </summary>
        public bool ManufacturerPartNumber { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“GTIN”字段
        /// </summary>
        public bool GTIN { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“产品成本”字段
        /// </summary>
        public bool ProductCost { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否显示“层次价格”字段
        /// </summary>
        public bool TierPrices { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“折扣”字段
        /// </summary>
        public bool Discounts { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“禁用购买按钮”字段
        /// </summary>
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“禁用愿望清单按钮”字段
        /// </summary>
        public bool DisableWishlistButton { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否显示“可用于预订”字段
        /// </summary>
        public bool AvailableForPreOrder { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“Call for price”字段
        /// </summary>
        public bool CallForPrice { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否显示“旧价格”字段
        /// </summary>
        public bool OldPrice { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否显示“客户输入价格”字段
        /// </summary>
        public bool CustomerEntersPrice { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示“PAngV”字段
        /// </summary>
        public bool PAngV { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Require other products added to the cart' field is shown
        /// </summary>
        public bool RequireOtherProductsAddedToTheCart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Is gift card' field is shown
        /// </summary>
        public bool IsGiftCard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Downloadable product' field is shown
        /// </summary>
        public bool DownloadableProduct { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Recurring product' field is shown
        /// </summary>
        public bool RecurringProduct { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Is rental' field is shown
        /// </summary>
        public bool IsRental { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Free shipping' field is shown
        /// </summary>
        public bool FreeShipping { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Ship separately' field is shown
        /// </summary>
        public bool ShipSeparately { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Additional shipping charge' field is shown
        /// </summary>
        public bool AdditionalShippingCharge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Delivery date' field is shown
        /// </summary>
        public bool DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Telecommunications, broadcasting and electronic services' field is shown
        /// </summary>
        public bool TelecommunicationsBroadcastingElectronicServices { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Product availability range' field is shown
        /// </summary>
        public bool ProductAvailabilityRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Use multiple warehouses' field is shown
        /// </summary>
        public bool UseMultipleWarehouses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Warehouse' field is shown
        /// </summary>
        public bool Warehouse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Display stock availability' field is shown
        /// </summary>
        public bool DisplayStockAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Display stock quantity' field is shown
        /// </summary>
        public bool DisplayStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Minimum stock quantity' field is shown
        /// </summary>
        public bool MinimumStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Low stock activity' field is shown
        /// </summary>
        public bool LowStockActivity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Notify admin for quantity below' field is shown
        /// </summary>
        public bool NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Backorders' field is shown
        /// </summary>
        public bool Backorders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allow back in stock subscriptions' field is shown
        /// </summary>
        public bool AllowBackInStockSubscriptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Minimum cart quantity' field is shown
        /// </summary>
        public bool MinimumCartQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Maximum cart quantity' field is shown
        /// </summary>
        public bool MaximumCartQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allowed quantities' field is shown
        /// </summary>
        public bool AllowedQuantities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Allow only existing attribute combinations' field is shown
        /// </summary>
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Not returnable' field is shown
        /// </summary>
        public bool NotReturnable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Weight' field is shown
        /// </summary>
        public bool Weight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Dimension' fields (height, length, width) are shown
        /// </summary>
        public bool Dimensions { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether 'Available start date' field is shown
        /// </summary>
        public bool AvailableStartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Available end date' field is shown
        /// </summary>
        public bool AvailableEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Mark as new' field is shown
        /// </summary>
        public bool MarkAsNew { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Mark as new. Start date' field is shown
        /// </summary>
        public bool MarkAsNewStartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Mark as new. End date' field is shown
        /// </summary>
        public bool MarkAsNewEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Published' field is shown
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Created on' field is shown
        /// </summary>
        public bool CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Updated on' field is shown
        /// </summary>
        public bool UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Related products' block is shown
        /// </summary>
        public bool RelatedProducts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Cross-sells products' block is shown
        /// </summary>
        public bool CrossSellsProducts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'SEO' tab is shown
        /// </summary>
        public bool Seo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Purchased with orders' tab is shown
        /// </summary>
        public bool PurchasedWithOrders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether one column is used on the product details page
        /// </summary>
        public bool OneColumnProductPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Product attributes' tab is shown
        /// </summary>
        public bool ProductAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Specification attributes' tab is shown
        /// </summary>
        public bool SpecificationAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Manufacturers' field is shown
        /// </summary>
        public bool Manufacturers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Stock quantity history' tab is shown
        /// </summary>
        public bool StockQuantityHistory { get; set; }
    }
}