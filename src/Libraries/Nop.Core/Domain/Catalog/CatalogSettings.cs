using System.Collections.Generic;
using Nop.Core.Configuration;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 目录设置
    /// </summary>
    public class CatalogSettings : ISettings
    {
        public CatalogSettings()
        {
            ProductSortingEnumDisabled = new List<int>();
            ProductSortingEnumDisplayOrder= new Dictionary<int, int>();
        }

        /// <summary>
        ///获取或设置一个值，指示未发布的产品详细信息页面的详细信息页面可以打开（对于SEO优化）
        /// </summary>
        public bool AllowViewUnpublishedProductPage { get; set; }
        /// <summary>
        ///获取或设置一个值，表示客户在访问未发布产品的页面时看到“停止”消息（如果“AllowViewUnpublishedProductPage”为true）
        /// </summary>
        public bool DisplayDiscontinuedMessageForUnpublishedProducts { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether "Published" or "Disable buy/wishlist buttons" flags should be updated after order cancellation (deletion).
        /// Of course, when qty > configured minimum stock level
        /// </summary>
        public bool PublishBackProductWhenCancellingOrders { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否在产品详细信息页面上显示产品SKU
        /// </summary>
        public bool ShowSkuOnProductDetailsPage { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否在目录页面上显示产品SKU
        /// </summary>
        public bool ShowSkuOnCatalogPages { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示产品的制造商部件号
        /// </summary>
        public bool ShowManufacturerPartNumber { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否显示产品的GTIN
        /// </summary>
        public bool ShowGtin { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否应为产品显示“免费送货”图标
        /// </summary>
        public bool ShowFreeShippingNotification { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否启用了产品排序
        /// </summary>
        public bool AllowProductSorting { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否允许客户更改产品视图模式
        /// </summary>
        public bool AllowProductViewModeChanging { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否允许客户更改产品视图模式
        /// </summary>
        public string DefaultViewMode { get; set; }

        /// <summary>
        ///获取或设置一个值，指示类别详细信息页面是否应包含子类别的产品
        /// </summary>
        public bool ShowProductsFromSubcategories { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否应在每个类别旁边显示产品数量
        /// </summary>
        public bool ShowCategoryProductNumber { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否包含子类别（'ShowCategoryProductNumber'为'true'时）
        /// </summary>
        public bool ShowCategoryProductNumberIncludingSubcategories { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否启用了类别面包屑
        /// </summary>
        public bool CategoryBreadcrumbEnabled { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否启用“共享按钮”
        /// </summary>
        public bool ShowShareButton { get; set; }

        /// <summary>
        /// 获取或设置共享代码（例如AddThis按钮代码）
        /// </summary>
        public string PageShareCode { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示产品评论必须获得批准
        /// </summary>
        public bool ProductReviewsMustBeApproved { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示产品评论的默认评级值
        /// </summary>
        public int DefaultProductRatingValue { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否允许匿名用户编写产品评论。
        /// </summary>
        public bool AllowAnonymousUsersToReviewProduct { get; set; }

        /// <summary>
        ///获取或设置一个值，指示产品是否只能由已经订购的客户进行审核
        /// </summary>
        public bool ProductReviewPossibleOnlyAfterPurchasing { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否启用了关于新商品评论的商店主通知
        /// </summary>
        public bool NotifyStoreOwnerAboutNewProductReviews { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示商店评论是否会在每个商店过滤
        /// </summary>
        public bool ShowProductReviewsPerStore { get; set; }

        /// <summary>
        /// 在帐户页面上获取或设置展示商品评论标签
        /// </summary>
        public bool ShowProductReviewsTabOnAccountPage { get; set; }

        /// <summary>
        /// 在帐户页面中获取或设置商品评论的页面大小
        /// </summary>
        public int ProductReviewsPageSizeOnAccountPage { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否启用了产品“电子邮件朋友”功能
        /// </summary>
        public bool EmailAFriendEnabled { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否允许匿名用户向朋友发送电子邮件。
        /// </summary>
        public bool AllowAnonymousUsersToEmailAFriend { get; set; }

        /// <summary>
        ///获取或设置一些“最近浏览的产品”
        /// </summary>
        public int RecentlyViewedProductsNumber { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否启用“最近浏览的产品”功能
        /// </summary>
        public bool RecentlyViewedProductsEnabled { get; set; }
        /// <summary>
        /// 在“新产品”页面上获取或设置多个产品
        /// </summary>
        public int NewProductsNumber { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否启用“新产品”页面
        /// </summary>
        public bool NewProductsEnabled { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否启用“比较产品”功能
        /// </summary>
        public bool CompareProductsEnabled { get; set; }
        /// <summary>
        ///获取或设置允许比较的产品数量
        /// </summary>
        public int CompareProductsNumber { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否启用自动完成
        /// </summary>
        public bool ProductSearchAutoCompleteEnabled { get; set; }
        /// <summary>
        /// 获取或设置使用“自动填充”功能返回的多个产品
        /// </summary>
        public int ProductSearchAutoCompleteNumberOfProducts { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否在自动完成搜索中显示产品图像
        /// </summary>
        public bool ShowProductImagesInSearchAutoComplete { get; set; }
        /// <summary>
        /// 获取或设置最小搜索字词长度
        /// </summary>
        public int ProductSearchTermMinimumLength { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否在主页上显示畅销书
        /// </summary>
        public bool ShowBestsellersOnHomepage { get; set; }
        /// <summary>
        /// 在主页上获取或设置一些畅销书
        /// </summary>
        public int NumberOfBestsellersOnHomepage { get; set; }

        /// <summary>
        /// 在搜索产品页面上获取或设置每页产品数量
        /// </summary>
        public int SearchPageProductsPerPage { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否允许客户在搜索产品页面上选择页面大小
        /// </summary>
        public bool SearchPageAllowCustomersToSelectPageSize { get; set; }
        /// <summary>
        /// 在搜索产品页面上获取或设置可用的客户可选页面大小选项
        /// </summary>
        public string SearchPagePageSizeOptions { get; set; }

        /// <summary>
        /// 获取或设置“购买上述其他客户购买的产品的列表”选项是启用的
        /// </summary>
        public bool ProductsAlsoPurchasedEnabled { get; set; }

        /// <summary>
        /// 获得或设置其他客户也购买的一些产品来显示
        /// </summary>
        public int ProductsAlsoPurchasedNumber { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否应使用AJAX处理属性更改。 它用于动态属性更改，组合的SKU / GTIN更新，条件属性
        /// </summary>
        public bool AjaxProcessAttributeChange { get; set; }

        /// <summary>
        /// 获取或设置多项产品标签出现在标签云
        /// </summary>
        public int NumberOfProductTags { get; set; }

        /// <summary>
        ///在“产品按标签”页面上获取或设置每页产品数量
        /// </summary>
        public int ProductsByTagPageSize { get; set; }

        /// <summary>
        ///获取或设置一个值，指示客户是否可以通过标签选择“产品”的页面大小
        /// </summary>
        public bool ProductsByTagAllowCustomersToSelectPageSize { get; set; }

        /// <summary>
        /// 获取或设置可用的客户可选择的页面大小选项“产品按标签”
        /// </summary>
        public string ProductsByTagPageSizeOptions { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否在比较产品中包含“简短描述”
        /// </summary>
        public bool IncludeShortDescriptionInCompareProducts { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否在比较产品中包含“完整描述”
        /// </summary>
        public bool IncludeFullDescriptionInCompareProducts { get; set; }
        /// <summary>
        /// 一个选项，指示类别和制造商页面上的产品是否也应包括特色产品
        /// </summary>
        public bool IncludeFeaturedProductsInNormalLists { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否应使用应用折扣显示层次价格（如果可用）
        /// </summary>
        public bool DisplayTierPricesWithDiscounts { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否忽略折扣（侧面）。 启用时可以显着提高性能
        /// </summary>
        public bool IgnoreDiscounts { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否忽略特色产品（侧面）。 启用时可以显着提高性能。
        /// </summary>
        public bool IgnoreFeaturedProducts { get; set; }
        /// <summary>
        ///获取或设置一个值，指示是否忽略ACL规则（侧面）。 启用时可以显着提高性能。
        /// </summary>
        public bool IgnoreAcl { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否忽略“每个商店的限制”规则（侧面）。 启用时可以显着提高性能。
        /// </summary>
        public bool IgnoreStoreLimitations { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否缓存产品价格。 启用时可以显着提高性能。
        /// </summary>
        public bool CacheProductPrices { get; set; }

        /// <summary>
        /// Gets or sets a value indicating maximum number of 'back in stock' subscription
        /// </summary>
        public int MaximumBackInStockSubscriptions { get; set; }

        /// <summary>
        /// 获取或设置表示在制造商块中显示多少制造商的值
        /// </summary>
        public int ManufacturersBlockItemsToDisplay { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否在页脚中显示有关运输和税收的信息（在德国使用）
        /// </summary>
        public bool DisplayTaxShippingInfoFooter { get; set; }
        /// <summary>
        ///获取或设置一个值，指示是否在产品详细信息页面（德国使用）中显示关于运输和税收的信息
        /// </summary>
        public bool DisplayTaxShippingInfoProductDetailsPage { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否在产品盒中显示有关运输和税收的信息（在德国使用）
        /// </summary>
        public bool DisplayTaxShippingInfoProductBoxes { get; set; }
        /// <summary>
        /// 获取或设置一个值，指示是否在购物车页面（德国使用）中显示关于运输和税收的信息
        /// </summary>
        public bool DisplayTaxShippingInfoShoppingCart { get; set; }
        /// <summary>
        ///获取或设置一个值，表示是否在愿望清单页面（德国使用）中显示关于运输和税收的信息
        /// </summary>
        public bool DisplayTaxShippingInfoWishlist { get; set; }
        /// <summary>
        ///获取或设置一个值，指示是否在订单详细信息页面（德国使用）中显示有关运输和税收的信息
        /// </summary>
        public bool DisplayTaxShippingInfoOrderDetailsPage { get; set; }

        /// <summary>
        /// 获取或设置默认值以用于类别页面大小选项（适用于新类别）
        /// </summary>
        public string DefaultCategoryPageSizeOptions { get; set; }
        /// <summary>
        ///获取或设置默认值以用于类别页面大小（适用于新类别）
        /// </summary>
        public int DefaultCategoryPageSize { get; set; }
        /// <summary>
        ///获取或设置默认值用于制造商页面大小选项（对于新的制造商）
        /// </summary>
        public string DefaultManufacturerPageSizeOptions { get; set; }
        /// <summary>
        /// 获取或设置用于制造商页面大小的默认值（对于新厂商）
        /// </summary>
        public int DefaultManufacturerPageSize { get; set; }

        /// <summary>
        ///获取或设置ProductSortingEnum的禁用值列表
        /// </summary>
        public List<int> ProductSortingEnumDisabled { get; set; }

        /// <summary>
        /// 获取或设置ProductSortingEnum值的显示顺序
        /// </summary>
        public Dictionary<int, int> ProductSortingEnumDisplayOrder { get; set; }

        /// <summary>
        ///获取或设置一个值，指示产品是否需要导出/导入其属性
        /// </summary>
        public bool ExportImportProductAttributes { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否需要创建用于导出的下拉列表
        /// </summary>
        public bool ExportImportUseDropdownlistsForAssociatedEntities { get; set; }
    }
}