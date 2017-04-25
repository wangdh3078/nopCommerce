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
    /// ��Ʒ
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
        /// ��ȡ�����ò�Ʒ���ͱ�ʶ��
        /// </summary>
        public int ProductTypeId { get; set; }
        /// <summary>
        ///��ȡ�����ø���Ʒ��ʶ���� ����ʶ����ز�Ʒ�����ޡ����顱��Ʒ��
        /// </summary>
        public int ParentGroupedProductId { get; set; }
        /// <summary>
        /// ��ȡ������ָʾ�˲�Ʒ�Ƿ���Ŀ¼����������пɼ���ֵ��
        /// It's used when this product is associated to some "grouped" one
        /// This way associated products could be accessed/added/etc only from a grouped product details page
        /// </summary>
        public bool VisibleIndividually { get; set; }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///��ȡ�����ü������
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        ///��ȡ����������������
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// ��ȡ�����ù���Ա����
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// ��ȡ��������ʹ�õĲ�Ʒģ���ʶ����ֵ
        /// </summary>
        public int ProductTemplateId { get; set; }

        /// <summary>
        /// ��ȡ�����ù�Ӧ�̱�ʶ��
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ�����ҳ����ʾ��Ʒ
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        ///��ȡ������Ԫ�ؼ���
        /// </summary>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// ��ȡ������Ԫ����
        /// </summary>
        public string MetaDescription { get; set; }
        /// <summary>
        /// ��ȡ������Ԫ����
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�����ͻ�����
        /// </summary>
        public bool AllowCustomerReviews { get; set; }
        /// <summary>
        ///��û����������ܺͣ���׼�����ۣ�
        /// </summary>
        public int ApprovedRatingSum { get; set; }
        /// <summary>
        /// ��û����������ܺͣ�δ����׼�����ۣ�
        /// </summary>
        public int NotApprovedRatingSum { get; set; }
        /// <summary>
        ///��û�����������ͶƱ����׼�����ۣ�
        /// </summary>
        public int ApprovedTotalReviews { get; set; }
        /// <summary>
        /// ��û�����������ͶƱ��δ����׼�����ۣ�
        /// </summary>
        public int NotApprovedTotalReviews { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾʵ���Ƿ���ACL
        /// </summary>
        public bool SubjectToAcl { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾʵ��������/������ĳЩ�̵�
        /// </summary>
        public bool LimitedToStores { get; set; }

        /// <summary>
        /// ��ȡ������SKU
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// ��ȡ������������������
        /// </summary>
        public string ManufacturerPartNumber { get; set; }
        /// <summary>
        /// ��ȡ������ȫ��ó����Ŀ��ţ�GTIN���� ��Щ��ʶ������UPC����������EAN��ŷ�ޣ���JAN���ձ�����ISBN�������鼮����
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�����Ʒ��
        /// </summary>
        public bool IsGiftCard { get; set; }
        /// <summary>
        /// ��ȡ��������Ʒ�����ͱ�ʶ��
        /// </summary>
        public int GiftCardTypeId { get; set; }
        /// <summary>
        ///��û����ù�������ʹ�õ���Ʒ���� ���δָ������ʹ�ò�Ʒ�۸�
        /// </summary>
        public decimal? OverriddenGiftCardAmount { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�Ҫ��������Ʒ��ӵ����ﳵ�У���ƷX��Ҫ��ƷY��
        /// </summary>
        public bool RequireOtherProducts { get; set; }
        /// <summary>
        /// ��ȡ����������Ĳ�Ʒ��ʶ�������ŷָ���
        /// </summary>
        public string RequiredProductIds { get; set; }
        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ�����Ʒ�Ƿ��Զ���ӵ����ﳵ��
        /// </summary>
        public bool AutomaticallyAddRequiredProducts { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�����
        /// </summary>
        public bool IsDownload { get; set; }
        /// <summary>
        /// ��ȡ���������ر�ʶ��
        /// </summary>
        public int DownloadId { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�˿����صĲ�Ʒ�Ƿ�������޴�������
        /// </summary>
        public bool UnlimitedDownloads { get; set; }
        /// <summary>
        /// ��ȡ�����������������
        /// </summary>
        public int MaxNumberOfDownloads { get; set; }
        /// <summary>
        /// ��ȡ�����ÿͻ����ʸ��ļ���������
        /// </summary>
        public int? DownloadExpirationDays { get; set; }
        /// <summary>
        /// ��ȡ���������ؼ�������
        /// </summary>
        public int DownloadActivationTypeId { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�������������ļ�
        /// </summary>
        public bool HasSampleDownload { get; set; }
        /// <summary>
        /// ��ȡ�������������ر�ʶ��
        /// </summary>
        public int SampleDownloadId { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�����û�Э��
        /// </summary>
        public bool HasUserAgreement { get; set; }
        /// <summary>
        /// ��ȡ���������Э����ı�
        /// </summary>
        public string UserAgreementText { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ��ظ�
        /// </summary>
        public bool IsRecurring { get; set; }
        /// <summary>
        ///��ȡ������ѭ������
        /// </summary>
        public int RecurringCycleLength { get; set; }
        /// <summary>
        ///��ȡ������ѭ������
        /// </summary>
        public int RecurringCyclePeriodId { get; set; }
        /// <summary>
        ///��ȡ������������
        /// </summary>
        public int RecurringTotalCycles { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ�Ϊ����
        /// </summary>
        public bool IsRental { get; set; }
        /// <summary>
        /// ��ȡ���趨һ��ʱ�����𳤶ȣ����ڼ�ļ۸�
        /// </summary>
        public int RentalPriceLength { get; set; }
        /// <summary>
        /// ��û����������ڣ����ڼ�ļ۸�
        /// </summary>
        public int RentalPricePeriodId { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾʵ���Ƿ�������
        /// </summary>
        public bool IsShipEnabled { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾʵ���Ƿ��������
        /// </summary>
        public bool IsFreeShipping { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ���ò�ƷӦ����������ÿ����Ŀ��
        /// </summary>
        public bool ShipSeparately { get; set; }
        /// <summary>
        ///��ȡ�����ö�����˷�
        /// </summary>
        public decimal AdditionalShippingCharge { get; set; }
        /// <summary>
        /// ��ȡ�����ý������ڱ�ʶ��
        /// </summary>
        public int DeliveryDateId { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ��Ʒ�Ƿ񱻱��Ϊ��˰
        /// </summary>
        public bool IsTaxExempt { get; set; }
        /// <summary>
        ///��ȡ������˰������ʶ��
        /// </summary>
        public int TaxCategoryId { get; set; }
        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ��Ʒ�ǵ��Ż��ǹ㲥����ӷ���
        /// </summary>
        public bool IsTelecommunicationsOrBroadcastingOrElectronicServices { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ��ι�������Դ
        /// </summary>
        public int ManageInventoryMethodId { get; set; }
        /// <summary>
        /// ��ȡ�����ò�Ʒ�����Է�Χ��ʶ��
        /// </summary>
        public int ProductAvailabilityRangeId { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ�Ϊ�˲�Ʒʹ�ö���ֿ�
        /// </summary>
        public bool UseMultipleWarehouses { get; set; }
        /// <summary>
        /// ��ȡ�����òֿ��ʶ��
        /// </summary>
        public int WarehouseId { get; set; }
        /// <summary>
        ///��ȡ�����ÿ������
        /// </summary>
        public int StockQuantity { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ���ʾ��������
        /// </summary>
        public bool DisplayStockAvailability { get; set; }
        /// <summary>
        ///��ȡ������һ��ָʾ�Ƿ���ʾ���������ֵ
        /// </summary>
        public bool DisplayStockQuantity { get; set; }
        /// <summary>
        /// ��ȡ��������С�������
        /// </summary>
        public int MinStockQuantity { get; set; }
        /// <summary>
        /// ��ȡ�����õͿ����ʶ��
        /// </summary>
        public int LowStockActivityId { get; set; }
        /// <summary>
        ///��֪ͨ����Աʱ��ȡ����������
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }
        /// <summary>
        ///��ȡ������ֵ��̨ģʽ��ʶ��
        /// </summary>
        public int BackorderModeId { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ����ʾ�Ƿ�����ع���涩��
        /// </summary>
        public bool AllowBackInStockSubscriptions { get; set; }
        /// <summary>
        ///��ȡ�����ö�����С����
        /// </summary>
        public int OrderMinimumQuantity { get; set; }
        /// <summary>
        /// ��ȡ�����ö����������
        /// </summary>
        public int OrderMaximumQuantity { get; set; }
        /// <summary>
        /// ��ȡ�����ö��ŷָ������������б� ��������κ���������Ϊ�ջ�Ϊ��
        /// </summary>
        public string AllowedQuantities { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether we allow adding to the cart/wishlist only attribute combinations that exist and have stock greater than zero.
        /// This option is used only when we have "manage inventory" set to "track inventory by product attributes"
        /// </summary>
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ����ʾ�ò�Ʒ�Ƿ���˻أ��ͻ������򱾲�Ʒ�ύ�˻�����
        /// </summary>
        public bool NotReturnable { get; set; }

        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ�Ƿ���ù�����ӵ����ﳵ����ť
        /// </summary>
        public bool DisableBuyButton { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ���á���ӵ�wishlist����ť
        /// </summary>
        public bool DisableWishlistButton { get; set; }
        /// <summary>
        ///��ȡ������һ��ֵ��ָʾ�����Ƿ������Ԥ��
        /// </summary>
        public bool AvailableForPreOrder { get; set; }
        /// <summary>
        /// ��ȡ�����ò�Ʒ�����ԵĿ�ʼ���ں�ʱ�䣨����Ԥ����Ʒ��
        /// </summary>
        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }
        /// <summary>
        ///��ȡ������һ��ֵ����ʾ�Ƿ���ʾ��Call for Pricing����Call for quote���������Ǽ۸�
        /// </summary>
        public bool CallForPrice { get; set; }
        /// <summary>
        /// ��ȡ���趨�۸�
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        ///��ȡ���趨�ɼ۸�
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// ��ȡ�����ò�Ʒ�ɱ�
        /// </summary>
        public decimal ProductCost { get; set; }
        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�ͻ��Ƿ�����۸�
        /// </summary>
        public bool CustomerEntersPrice { get; set; }
        /// <summary>
        /// ��ȡ�����ÿͻ��������ͼ۸�
        /// </summary>
        public decimal MinimumCustomerEnteredPrice { get; set; }
        /// <summary>
        /// ��ȡ�����ÿͻ���������۸�
        /// </summary>
        public decimal MaximumCustomerEnteredPrice { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ����û����۸�PAngV���� �ɵ¹��û�ʹ�á�
        /// </summary>
        public bool BasepriceEnabled { get; set; }
        /// <summary>
        /// ��ȡ������PAngV��Ʒ�е�����
        /// </summary>
        public decimal BasepriceAmount { get; set; }
        /// <summary>
        ///��ȡ������PAngV��MeasureWeightʵ�壩�Ĳ�Ʒ��λ
        /// </summary>
        public int BasepriceUnitId { get; set; }
        /// <summary>
        /// ��ȡ������PAngV�Ĳο���
        /// </summary>
        public decimal BasepriceBaseAmount { get; set; }
        /// <summary>
        /// ��ȡ������PAngV��MeasureWeightʵ�壩�Ĳο���λ
        /// </summary>
        public int BasepriceBaseUnitId { get; set; }


        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ����Ʒ�Ƿ񱻱��Ϊ�µ�
        /// </summary>
        public bool MarkAsNew { get; set; }
        /// <summary>
        ///��ȡ�������²�Ʒ�Ŀ�ʼ���ں�ʱ�䣨�����ڽ���Ʒ����Ϊ���¡����� �����Ժ��Դ�����
        /// </summary>
        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }
        /// <summary>
        /// ��ȡ�������²�Ʒ�Ľ������ں�ʱ�䣨����Ʒ����Ϊ���¡����� �����Ժ��Դ�����
        /// </summary>
        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�˲�Ʒ�Ƿ������˲�μ۸�
        /// <remarks>The same as if we run this.TierPrices.Count > 0
        ///����ʹ�ô����Խ��������Ż���
        /// �������������Ϊfalse����ô���ǲ���Ҫ���ز�μ۸�ĵ�������
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
        /// ��ȡ�����ó���
        /// </summary>
        public decimal Length { get; set; }
        /// <summary>
        ///��ȡ�����ÿ��
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// ��ȡ�����ø߶�
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// ��ȡ�����ÿ��õĿ�ʼ���ں�ʱ��
        /// </summary>
        public DateTime? AvailableStartDateTimeUtc { get; set; }
        /// <summary>
        /// ��ȡ�����ÿ��õĽ������ں�ʱ��
        /// </summary>
        public DateTime? AvailableEndDateTimeUtc { get; set; }

        /// <summary>
        /// ��ȡ��������ʾ˳��
        /// ����������Ĳ�Ʒʱʹ�ø�ֵ���롰���顱��Ʒ��ʹ�ã�
        ///��ֵ����������ҳ��Ʒʱ
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        ///��ȡ������һ��ֵ��ָʾʵ���Ƿ��ѷ���
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        ///��ȡ������һ��ֵ��ָʾʵ���Ƿ��ѱ�ɾ��
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        ///��ȡ�����ò�Ʒ���������ں�ʱ��
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
        /// <summary>
        ///��ȡ�����ò�Ʒ���µ����ں�ʱ��
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }






        /// <summary>
        /// ȡ�����ò�Ʒ����
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
        /// ��ȡ�����ú���ģʽ
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
        /// ��ȡ���������ؼ�������
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
        /// ��ȡ��������Ʒ������
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
        /// ��û����õͿ��
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
        ///��ȡ������ָʾ��ι�������Դ��ֵ
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
        /// ��ȡ�����������Բ�Ʒ������
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
        /// ��ȡ���������޲�Ʒ������
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
        /// ��ȡ�����ò�Ʒ���ļ���
        /// </summary>
        public virtual ICollection<ProductCategory> ProductCategories
        {
            get { return _productCategories ?? (_productCategories = new List<ProductCategory>()); }
            protected set { _productCategories = value; }
        }

        /// <summary>
        ///��ȡ�����ò�Ʒ�����̵ļ���
        /// </summary>
        public virtual ICollection<ProductManufacturer> ProductManufacturers
        {
            get { return _productManufacturers ?? (_productManufacturers = new List<ProductManufacturer>()); }
            protected set { _productManufacturers = value; }
        }

        /// <summary>
        /// ��ȡ�����ò�ƷͼƬ�ļ���
        /// </summary>
        public virtual ICollection<ProductPicture> ProductPictures
        {
            get { return _productPictures ?? (_productPictures = new List<ProductPicture>()); }
            protected set { _productPictures = value; }
        }

        /// <summary>
        /// ��ȡ�����ò�Ʒ���۵ļ���
        /// </summary>
        public virtual ICollection<ProductReview> ProductReviews
        {
            get { return _productReviews ?? (_productReviews = new List<ProductReview>()); }
            protected set { _productReviews = value; }
        }

        /// <summary>
        /// ��ȡ�����ò�Ʒ�������
        /// </summary>
        public virtual ICollection<ProductSpecificationAttribute> ProductSpecificationAttributes
        {
            get { return _productSpecificationAttributes ?? (_productSpecificationAttributes = new List<ProductSpecificationAttribute>()); }
            protected set { _productSpecificationAttributes = value; }
        }

        /// <summary>
        ///��ȡ�����ò�Ʒ��ǩ
        /// </summary>
        public virtual ICollection<ProductTag> ProductTags
        {
            get { return _productTags ?? (_productTags = new List<ProductTag>()); }
            protected set { _productTags = value; }
        }

        /// <summary>
        ///��ȡ�����ò�Ʒ����ӳ��
        /// </summary>
        public virtual ICollection<ProductAttributeMapping> ProductAttributeMappings
        {
            get { return _productAttributeMappings ?? (_productAttributeMappings = new List<ProductAttributeMapping>()); }
            protected set { _productAttributeMappings = value; }
        }

        /// <summary>
        ///��ȡ�����ò�Ʒ�������
        /// </summary>
        public virtual ICollection<ProductAttributeCombination> ProductAttributeCombinations
        {
            get { return _productAttributeCombinations ?? (_productAttributeCombinations = new List<ProductAttributeCombination>()); }
            protected set { _productAttributeCombinations = value; }
        }

        /// <summary>
        /// ��ȡ�����ò�μ۸�
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