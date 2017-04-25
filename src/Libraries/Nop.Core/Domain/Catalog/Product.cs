using System;
using System.Collections.Generic;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 产品
    /// </summary>
    public partial class Product : BaseEntity, ILocalizedEntity, ISlugSupported, IAclSupported, IStoreMappingSupported
    {
        private ICollection<ProductCategory> _productCategories;
        private ICollection<ProductManufacturer> _productManufacturers;
        private ICollection<ProductPicture> _productPictures;
        private ICollection<ProductReview> _productReviews;
        private ICollection<ProductSpecificationAttribute> _productSpecificationAttributes;
        private ICollection<ProductTag> _productTags;
        private ICollection<ProductAttributeMapping> _productAttributeMappings;
        private ICollection<ProductAttributeCombination> _productAttributeCombinations;
        private ICollection<TierPrice> _tierPrices;
        private ICollection<Discount> _appliedDiscounts;
        private ICollection<ProductWarehouseInventory> _productWarehouseInventory;


        /// <summary>
        /// 获取或设置产品类型标识符
        /// </summary>
        public int ProductTypeId { get; set; }
        /// <summary>
        ///获取或设置父产品标识符。 用于识别相关产品（仅限“分组”产品）
        /// </summary>
        public int ParentGroupedProductId { get; set; }
        /// <summary>
        /// 获取或设置指示此产品是否在目录或搜索结果中可见的值。
        /// It's used when this product is associated to some "grouped" one
        /// This way associated products could be accessed/added/etc only from a grouped product details page
        /// </summary>
        public bool VisibleIndividually { get; set; }

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///获取或设置简短描述
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        ///获取或设置完整的描述
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// 获取或设置管理员评论
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// 获取或设置已使用的产品模板标识符的值
        /// </summary>
        public int ProductTemplateId { get; set; }

        /// <summary>
        /// 获取或设置供应商标识符
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否在主页上显示产品
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        ///获取或设置元关键字
        /// </summary>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// 获取或设置元描述
        /// </summary>
        public string MetaDescription { get; set; }
        /// <summary>
        /// 获取或设置元标题
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示产品是否允许客户评价
        /// </summary>
        public bool AllowCustomerReviews { get; set; }
        /// <summary>
        ///获得或设置评级总和（批准的评论）
        /// </summary>
        public int ApprovedRatingSum { get; set; }
        /// <summary>
        /// 获得或设置评级总和（未经批准的评论）
        /// </summary>
        public int NotApprovedRatingSum { get; set; }
        /// <summary>
        ///获得或设置总评分投票（批准的评论）
        /// </summary>
        public int ApprovedTotalReviews { get; set; }
        /// <summary>
        /// 获得或设置总评分投票（未经批准的评论）
        /// </summary>
        public int NotApprovedTotalReviews { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示实体是否受ACL
        /// </summary>
        public bool SubjectToAcl { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示实体是限制/限制于某些商店
        /// </summary>
        public bool LimitedToStores { get; set; }

        /// <summary>
        /// 获取或设置SKU
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// 获取或设置制造商零件编号
        /// </summary>
        public string ManufacturerPartNumber { get; set; }
        /// <summary>
        /// 获取或设置全球贸易项目编号（GTIN）。 这些标识符包括UPC（北美），EAN（欧洲），JAN（日本）和ISBN（用于书籍）。
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        ///获取或设置一个值，指示产品是否是礼品卡
        /// </summary>
        public bool IsGiftCard { get; set; }
        /// <summary>
        /// 获取或设置礼品卡类型标识符
        /// </summary>
        public int GiftCardTypeId { get; set; }
        /// <summary>
        ///获得或设置购买后可以使用的礼品卡金额。 如果未指定，则将使用产品价格。
        /// </summary>
        public decimal? OverriddenGiftCardAmount { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示产品是否要求将其他产品添加到购物车中（产品X需要产品Y）
        /// </summary>
        public bool RequireOtherProducts { get; set; }
        /// <summary>
        /// 获取或设置所需的产品标识符（逗号分隔）
        /// </summary>
        public string RequiredProductIds { get; set; }
        /// <summary>
        ///获取或设置一个值，指示所需产品是否自动添加到购物车中
        /// </summary>
        public bool AutomaticallyAddRequiredProducts { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示产品是否下载
        /// </summary>
        public bool IsDownload { get; set; }
        /// <summary>
        /// 获取或设置下载标识符
        /// </summary>
        public int DownloadId { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示此可下载的产品是否可以无限次数下载
        /// </summary>
        public bool UnlimitedDownloads { get; set; }
        /// <summary>
        /// 获取或设置最大下载数量
        /// </summary>
        public int MaxNumberOfDownloads { get; set; }
        /// <summary>
        /// 获取或设置客户访问该文件的天数。
        /// </summary>
        public int? DownloadExpirationDays { get; set; }
        /// <summary>
        /// 获取或设置下载激活类型
        /// </summary>
        public int DownloadActivationTypeId { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示产品是否具有样本下载文件
        /// </summary>
        public bool HasSampleDownload { get; set; }
        /// <summary>
        /// 获取或设置样本下载标识符
        /// </summary>
        public int SampleDownloadId { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示产品是否具有用户协议
        /// </summary>
        public bool HasUserAgreement { get; set; }
        /// <summary>
        /// 获取或设置许可协议的文本
        /// </summary>
        public string UserAgreementText { get; set; }

        /// <summary>
        ///获取或设置一个值，指示产品是否重复
        /// </summary>
        public bool IsRecurring { get; set; }
        /// <summary>
        ///获取或设置循环长度
        /// </summary>
        public int RecurringCycleLength { get; set; }
        /// <summary>
        ///获取或设置循环周期
        /// </summary>
        public int RecurringCyclePeriodId { get; set; }
        /// <summary>
        ///获取或设置总周期
        /// </summary>
        public int RecurringTotalCycles { get; set; }

        /// <summary>
        ///获取或设置一个值，指示产品是否为租赁
        /// </summary>
        public bool IsRental { get; set; }
        /// <summary>
        /// 获取或设定一段时间的租金长度（此期间的价格）
        /// </summary>
        public int RentalPriceLength { get; set; }
        /// <summary>
        /// 获得或设置租赁期（此期间的价格）
        /// </summary>
        public int RentalPricePeriodId { get; set; }

        /// <summary>
        ///获取或设置一个值，指示实体是否已启用
        /// </summary>
        public bool IsShipEnabled { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示实体是否免费运送
        /// </summary>
        public bool IsFreeShipping { get; set; }
        /// <summary>
        /// 获取或设置一个值，该产品应单独发货（每个项目）
        /// </summary>
        public bool ShipSeparately { get; set; }
        /// <summary>
        ///获取或设置额外的运费
        /// </summary>
        public decimal AdditionalShippingCharge { get; set; }
        /// <summary>
        /// 获取或设置交货日期标识符
        /// </summary>
        public int DeliveryDateId { get; set; }

        /// <summary>
        ///获取或设置一个值，指示产品是否被标记为免税
        /// </summary>
        public bool IsTaxExempt { get; set; }
        /// <summary>
        ///获取或设置税种类别标识符
        /// </summary>
        public int TaxCategoryId { get; set; }
        /// <summary>
        ///获取或设置一个值，指示产品是电信还是广播或电子服务
        /// </summary>
        public bool IsTelecommunicationsOrBroadcastingOrElectronicServices { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示如何管理广告资源
        /// </summary>
        public int ManageInventoryMethodId { get; set; }
        /// <summary>
        /// 获取或设置产品可用性范围标识符
        /// </summary>
        public int ProductAvailabilityRangeId { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否为此产品使用多个仓库
        /// </summary>
        public bool UseMultipleWarehouses { get; set; }
        /// <summary>
        /// 获取或设置仓库标识符
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        ///获取或设置库存数量
        /// </summary>
        public int StockQuantity { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否显示库存可用性
        /// </summary>
        public bool DisplayStockAvailability { get; set; }
        /// <summary>
        ///获取或设置一个指示是否显示库存数量的值
        /// </summary>
        public bool DisplayStockQuantity { get; set; }
        /// <summary>
        /// 获取或设置最小库存数量
        /// </summary>
        public int MinStockQuantity { get; set; }
        /// <summary>
        /// 获取或设置低库存活动标识符
        /// </summary>
        public int LowStockActivityId { get; set; }
        /// <summary>
        ///当通知管理员时获取或设置数量
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }
        /// <summary>
        ///获取或设置值后台模式标识符
        /// </summary>
        public int BackorderModeId { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示是否允许回购库存订阅
        /// </summary>
        public bool AllowBackInStockSubscriptions { get; set; }
        /// <summary>
        ///获取或设置订单最小数量
        /// </summary>
        public int OrderMinimumQuantity { get; set; }
        /// <summary>
        /// 获取或设置订单最大数量
        /// </summary>
        public int OrderMaximumQuantity { get; set; }
        /// <summary>
        /// 获取或设置逗号分隔的允许数量列表。 如果允许任何数量，则为空或为空
        /// </summary>
        public string AllowedQuantities { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether we allow adding to the cart/wishlist only attribute combinations that exist and have stock greater than zero.
        /// This option is used only when we have "manage inventory" set to "track inventory by product attributes"
        /// </summary>
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示该产品是否可退回（客户可以向本产品提交退货请求）
        /// </summary>
        public bool NotReturnable { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否禁用购买（添加到购物车）按钮
        /// </summary>
        public bool DisableBuyButton { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否禁用“添加到wishlist”按钮
        /// </summary>
        public bool DisableWishlistButton { get; set; }
        /// <summary>
        ///获取或设置一个值，指示此项是否可用于预购
        /// </summary>
        public bool AvailableForPreOrder { get; set; }
        /// <summary>
        /// 获取或设置产品可用性的开始日期和时间（对于预购产品）
        /// </summary>
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }
        /// <summary>
        ///获取或设置一个值，表示是否显示“Call for Pricing”或“Call for quote”，而不是价格
        /// </summary>
        public bool CallForPrice { get; set; }
        /// <summary>
        /// 获取或设定价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        ///获取或设定旧价格
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// 获取或设置产品成本
        /// </summary>
        public decimal ProductCost { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示客户是否输入价格
        /// </summary>
        public bool CustomerEntersPrice { get; set; }
        /// <summary>
        /// 获取或设置客户输入的最低价格
        /// </summary>
        public decimal MinimumCustomerEnteredPrice { get; set; }
        /// <summary>
        /// 获取或设置客户输入的最大价格
        /// </summary>
        public decimal MaximumCustomerEnteredPrice { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否启用基本价格（PAngV）。 由德国用户使用。
        /// </summary>
        public bool BasepriceEnabled { get; set; }
        /// <summary>
        /// 获取或设置PAngV产品中的数量
        /// </summary>
        public decimal BasepriceAmount { get; set; }
        /// <summary>
        ///获取或设置PAngV（MeasureWeight实体）的产品单位
        /// </summary>
        public int BasepriceUnitId { get; set; }
        /// <summary>
        /// 获取或设置PAngV的参考量
        /// </summary>
        public decimal BasepriceBaseAmount { get; set; }
        /// <summary>
        /// 获取或设置PAngV（MeasureWeight实体）的参考单位
        /// </summary>
        public int BasepriceBaseUnitId { get; set; }


        /// <summary>
        /// 获取或设置一个值，指示本产品是否被标记为新的
        /// </summary>
        public bool MarkAsNew { get; set; }
        /// <summary>
        ///获取或设置新产品的开始日期和时间（从日期将产品设置为“新”）。 留空以忽略此属性
        /// </summary>
        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }
        /// <summary>
        /// 获取或设置新产品的结束日期和时间（将产品设置为“新”）。 留空以忽略此属性
        /// </summary>
        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示此产品是否配置了层次价格
        /// <remarks>The same as if we run this.TierPrices.Count > 0
        ///我们使用此属性进行性能优化：
        /// 如果此属性设置为false，那么我们不需要加载层次价格的导航属性
        /// </remarks>
        /// </summary>
        public bool HasTierPrices { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this product has discounts applied
        /// <remarks>The same as if we run this.AppliedDiscounts.Count > 0
        /// We use this property for performance optimization:
        /// if this property is set to false, then we do not need to load Applied Discounts navigation property
        /// </remarks>
        /// </summary>
        public bool HasDiscountsApplied { get; set; }

        /// <summary>
        /// Gets or sets the weight
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 获取或设置长度
        /// </summary>
        public decimal Length { get; set; }
        /// <summary>
        ///获取或设置宽度
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// 获取或设置高度
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// 获取或设置可用的开始日期和时间
        /// </summary>
        public DateTime? AvailableStartDateTimeUtc { get; set; }
        /// <summary>
        /// 获取或设置可用的结束日期和时间
        /// </summary>
        public DateTime? AvailableEndDateTimeUtc { get; set; }

        /// <summary>
        /// 获取或设置显示顺序。
        /// 排序相关联的产品时使用该值（与“分组”产品中使用）
        ///此值用于排序主页产品时
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        ///获取或设置一个值，指示实体是否已发布
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        ///获取或设置一个值，指示实体是否已被删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        ///获取或设置产品创建的日期和时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        ///获取或设置产品更新的日期和时间
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }






        /// <summary>
        /// 取或设置产品类型
        /// </summary>
        public ProductType ProductType
        {
            get
            {
                return (ProductType)this.ProductTypeId;
            }
            set
            {
                this.ProductTypeId = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置后退模式
        /// </summary>
        public BackorderMode BackorderMode
        {
            get
            {
                return (BackorderMode)this.BackorderModeId;
            }
            set
            {
                this.BackorderModeId = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置下载激活类型
        /// </summary>
        public DownloadActivationType DownloadActivationType
        {
            get
            {
                return (DownloadActivationType)this.DownloadActivationTypeId;
            }
            set
            {
                this.DownloadActivationTypeId = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置礼品卡类型
        /// </summary>
        public GiftCardType GiftCardType
        {
            get
            {
                return (GiftCardType)this.GiftCardTypeId;
            }
            set
            {
                this.GiftCardTypeId = (int)value;
            }
        }

        /// <summary>
        /// 获得或设置低库存活动
        /// </summary>
        public LowStockActivity LowStockActivity
        {
            get
            {
                return (LowStockActivity)this.LowStockActivityId;
            }
            set
            {
                this.LowStockActivityId = (int)value;
            }
        }

        /// <summary>
        ///获取或设置指示如何管理广告资源的值
        /// </summary>
        public ManageInventoryMethod ManageInventoryMethod
        {
            get
            {
                return (ManageInventoryMethod)this.ManageInventoryMethodId;
            }
            set
            {
                this.ManageInventoryMethodId = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置周期性产品的周期
        /// </summary>
        public RecurringProductCyclePeriod RecurringCyclePeriod
        {
            get
            {
                return (RecurringProductCyclePeriod)this.RecurringCyclePeriodId;
            }
            set
            {
                this.RecurringCyclePeriodId = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置租赁产品的期限
        /// </summary>
        public RentalPricePeriod RentalPricePeriod
        {
            get
            {
                return (RentalPricePeriod)this.RentalPricePeriodId;
            }
            set
            {
                this.RentalPricePeriodId = (int)value;
            }
        }






        /// <summary>
        /// 获取或设置产品类别的集合
        /// </summary>
        public virtual ICollection<ProductCategory> ProductCategories
        {
            get { return _productCategories ?? (_productCategories = new List<ProductCategory>()); }
            protected set { _productCategories = value; }
        }

        /// <summary>
        ///获取或设置产品制造商的集合
        /// </summary>
        public virtual ICollection<ProductManufacturer> ProductManufacturers
        {
            get { return _productManufacturers ?? (_productManufacturers = new List<ProductManufacturer>()); }
            protected set { _productManufacturers = value; }
        }

        /// <summary>
        /// 获取或设置产品图片的集合
        /// </summary>
        public virtual ICollection<ProductPicture> ProductPictures
        {
            get { return _productPictures ?? (_productPictures = new List<ProductPicture>()); }
            protected set { _productPictures = value; }
        }

        /// <summary>
        /// 获取或设置产品评论的集合
        /// </summary>
        public virtual ICollection<ProductReview> ProductReviews
        {
            get { return _productReviews ?? (_productReviews = new List<ProductReview>()); }
            protected set { _productReviews = value; }
        }

        /// <summary>
        /// 获取或设置产品规格属性
        /// </summary>
        public virtual ICollection<ProductSpecificationAttribute> ProductSpecificationAttributes
        {
            get { return _productSpecificationAttributes ?? (_productSpecificationAttributes = new List<ProductSpecificationAttribute>()); }
            protected set { _productSpecificationAttributes = value; }
        }

        /// <summary>
        ///获取或设置产品标签
        /// </summary>
        public virtual ICollection<ProductTag> ProductTags
        {
            get { return _productTags ?? (_productTags = new List<ProductTag>()); }
            protected set { _productTags = value; }
        }

        /// <summary>
        ///获取或设置产品属性映射
        /// </summary>
        public virtual ICollection<ProductAttributeMapping> ProductAttributeMappings
        {
            get { return _productAttributeMappings ?? (_productAttributeMappings = new List<ProductAttributeMapping>()); }
            protected set { _productAttributeMappings = value; }
        }

        /// <summary>
        ///获取或设置产品属性组合
        /// </summary>
        public virtual ICollection<ProductAttributeCombination> ProductAttributeCombinations
        {
            get { return _productAttributeCombinations ?? (_productAttributeCombinations = new List<ProductAttributeCombination>()); }
            protected set { _productAttributeCombinations = value; }
        }

        /// <summary>
        /// 获取或设置层次价格
        /// </summary>
        public virtual ICollection<TierPrice> TierPrices
        {
            get { return _tierPrices ?? (_tierPrices = new List<TierPrice>()); }
            protected set { _tierPrices = value; }
        }

        /// <summary>
        /// Gets or sets the collection of applied discounts
        /// </summary>
        public virtual ICollection<Discount> AppliedDiscounts
        {
            get { return _appliedDiscounts ?? (_appliedDiscounts = new List<Discount>()); }
            protected set { _appliedDiscounts = value; }
        }
        
        /// <summary>
        /// Gets or sets the collection of "ProductWarehouseInventory" records. We use it only when "UseMultipleWarehouses" is set to "true" and ManageInventoryMethod" to "ManageStock"
        /// </summary>
        public virtual ICollection<ProductWarehouseInventory> ProductWarehouseInventory
        {
            get { return _productWarehouseInventory ?? (_productWarehouseInventory = new List<ProductWarehouseInventory>()); }
            protected set { _productWarehouseInventory = value; }
        }
    }
}