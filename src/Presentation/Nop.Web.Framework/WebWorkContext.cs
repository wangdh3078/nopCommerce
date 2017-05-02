using System;
using System.Linq;
using System.Web;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Core.Fakes;
using Nop.Services.Authentication;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Framework.Localization;

namespace Nop.Web.Framework
{
    /// <summary>
    /// Web应用程序的工作环境
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region 静态变量

        private const string CustomerCookieName = "Nop.customer";

        #endregion

        #region 字段

        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IVendorService _vendorService;
        private readonly IStoreContext _storeContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly TaxSettings _taxSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly IStoreMappingService _storeMappingService;

        private Customer _cachedCustomer;
        private Customer _originalCustomerIfImpersonated;
        private Vendor _cachedVendor;
        private Language _cachedLanguage;
        private Currency _cachedCurrency;
        private TaxDisplayType? _cachedTaxDisplayType;

        #endregion

        #region 构造函数

        public WebWorkContext(HttpContextBase httpContext,
            ICustomerService customerService,
            IVendorService vendorService,
            IStoreContext storeContext,
            IAuthenticationService authenticationService,
            ILanguageService languageService,
            ICurrencyService currencyService,
            IGenericAttributeService genericAttributeService,
            TaxSettings taxSettings, 
            CurrencySettings currencySettings,
            LocalizationSettings localizationSettings,
            IUserAgentHelper userAgentHelper,
            IStoreMappingService storeMappingService)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._vendorService = vendorService;
            this._storeContext = storeContext;
            this._authenticationService = authenticationService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._genericAttributeService = genericAttributeService;
            this._taxSettings = taxSettings;
            this._currencySettings = currencySettings;
            this._localizationSettings = localizationSettings;
            this._userAgentHelper = userAgentHelper;
            this._storeMappingService = storeMappingService;
        }

        #endregion

        #region Utilities

        protected virtual HttpCookie GetCustomerCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[CustomerCookieName];
        }

        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(CustomerCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24*365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        protected virtual Language GetLanguageFromUrl()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            string virtualPath = _httpContext.Request.AppRelativeCurrentExecutionFilePath;
            string applicationPath = _httpContext.Request.ApplicationPath;
            if (!virtualPath.IsLocalizedUrl(applicationPath, false))
                return null;

            var seoCode = virtualPath.GetLanguageSeoCodeFromUrl(applicationPath, false);
            if (string.IsNullOrEmpty(seoCode))
                return null;

            var language = _languageService
                .GetAllLanguages()
                .FirstOrDefault(l => seoCode.Equals(l.UniqueSeoCode, StringComparison.InvariantCultureIgnoreCase));
            if (language != null && language.Published && _storeMappingService.Authorize(language))
            {
                return language;
            }

            return null;
        }

        protected virtual Language GetLanguageFromBrowserSettings()
        {
            if (_httpContext == null ||
                _httpContext.Request == null ||
                _httpContext.Request.UserLanguages == null)
                return null;

            var userLanguage = _httpContext.Request.UserLanguages.FirstOrDefault();
            if (string.IsNullOrEmpty(userLanguage))
                return null;

            var language = _languageService
                .GetAllLanguages()
                .FirstOrDefault(l => userLanguage.Equals(l.LanguageCulture, StringComparison.InvariantCultureIgnoreCase));
            if (language != null && language.Published && _storeMappingService.Authorize(language))
            {
                return language;
            }

            return null;
        }

        #endregion

        #region 属性

        /// <summary>
        ///获取或设置当前客户
        /// </summary>
        public virtual Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;

                Customer customer = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //检查请求是否由后台任务进行
                    //在这种情况下，返回内置的客户记录用于后台任务
                    customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.BackgroundTask);
                }

                //检查请求是否由搜索引擎进行
                //在这种情况下，返回搜索引擎的内置客户记录
                //或注释以下两行代码以禁用此功能
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    if (_userAgentHelper.IsSearchEngine())
                    {
                        customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.SearchEngine);
                    }
                }

                //注册用户
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    customer = _authenticationService.GetAuthenticatedCustomer();
                }

                //如果需要模拟用户（目前用于“电话订单”支持）
                if (customer != null && !customer.Deleted && customer.Active && !customer.RequireReLogin)
                {
                    var impersonatedCustomerId = customer.GetAttribute<int?>(SystemCustomerAttributeNames.ImpersonatedCustomerId);
                    if (impersonatedCustomerId.HasValue && impersonatedCustomerId.Value > 0)
                    {
                        var impersonatedCustomer = _customerService.GetCustomerById(impersonatedCustomerId.Value);
                        if (impersonatedCustomer != null && !impersonatedCustomer.Deleted && impersonatedCustomer.Active && !impersonatedCustomer.RequireReLogin)
                        {
                            //设置模拟客户
                            _originalCustomerIfImpersonated = customer;
                            customer = impersonatedCustomer;
                        }
                    }
                }

                //加载客户
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    var customerCookie = GetCustomerCookie();
                    if (customerCookie != null && !string.IsNullOrEmpty(customerCookie.Value))
                    {
                        Guid customerGuid;
                        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                        {
                            var customerByCookie = _customerService.GetCustomerByGuid(customerGuid);
                            if (customerByCookie != null &&
                                //this customer (from cookie) should not be registered
                                !customerByCookie.IsRegistered())
                                customer = customerByCookie;
                        }
                    }
                }

                //创建客人如果不存在
                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    customer = _customerService.InsertGuestCustomer();
                }


                //验证
                if (!customer.Deleted && customer.Active && !customer.RequireReLogin)
                {
                    SetCustomerCookie(customer.CustomerGuid);
                    _cachedCustomer = customer;
                }

                return _cachedCustomer;
            }
            set
            {
                SetCustomerCookie(value.CustomerGuid);
                _cachedCustomer = value;
            }
        }

        /// <summary>
        /// 获取或设置原始客户（如果当前的客户被模拟）
        /// </summary>
        public virtual Customer OriginalCustomerIfImpersonated
        {
            get
            {
                return _originalCustomerIfImpersonated;
            }
        }

        /// <summary>
        /// 获取或设置当前供应商（登录管理器）
        /// </summary>
        public virtual Vendor CurrentVendor
        {
            get
            {
                if (_cachedVendor != null)
                    return _cachedVendor;

                var currentCustomer = this.CurrentCustomer;
                if (currentCustomer == null)
                    return null;

                var vendor = _vendorService.GetVendorById(currentCustomer.VendorId);

                //validation
                if (vendor != null && !vendor.Deleted && vendor.Active)
                    _cachedVendor = vendor;

                return _cachedVendor;
            }
        }

        /// <summary>
        /// 获取或设置当前用户工作语言
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;
                
                Language detectedLanguage = null;
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //从URL获取语言
                    detectedLanguage = GetLanguageFromUrl();
                }
                if (detectedLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
                {
                    //从浏览器设置获取语言
                    //但我们只做一次
                    if (!this.CurrentCustomer.GetAttribute<bool>(SystemCustomerAttributeNames.LanguageAutomaticallyDetected, 
                        _genericAttributeService, _storeContext.CurrentStore.Id))
                    {
                        detectedLanguage = GetLanguageFromBrowserSettings();
                        if (detectedLanguage != null)
                        {
                            _genericAttributeService.SaveAttribute(this.CurrentCustomer, SystemCustomerAttributeNames.LanguageAutomaticallyDetected,
                                 true, _storeContext.CurrentStore.Id);
                        }
                    }
                }
                if (detectedLanguage != null)
                {
                    //检测到语言。 现在我们需要保存它
                    if (this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.LanguageId,
                        _genericAttributeService, _storeContext.CurrentStore.Id) != detectedLanguage.Id)
                    {
                        _genericAttributeService.SaveAttribute(this.CurrentCustomer, SystemCustomerAttributeNames.LanguageId,
                            detectedLanguage.Id, _storeContext.CurrentStore.Id);
                    }
                }

                var allLanguages = _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id);
                //查找当前的客户语言
                var languageId = this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.LanguageId,
                    _genericAttributeService, _storeContext.CurrentStore.Id);
                var language = allLanguages.FirstOrDefault(x => x.Id == languageId);
                if (language == null)
                {
                    //没有找到，那么我们加载当前语言的默认货币（如果指定）
                    languageId = _storeContext.CurrentStore.DefaultLanguageId;
                    language = allLanguages.FirstOrDefault(x => x.Id == languageId);
                }
                if (language == null)
                {
                    //它没有指定，然后返回第一个（由当前存储过滤）找到一个
                    language = allLanguages.FirstOrDefault();
                }
                if (language == null)
                {
                    //它没有指定，然后返回第一个找到一个
                    language = _languageService.GetAllLanguages().FirstOrDefault();
                }

                //缓存
                _cachedLanguage = language;
                return _cachedLanguage;
            }
            set
            {
                var languageId = value != null ? value.Id : 0;
                _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                    SystemCustomerAttributeNames.LanguageId,
                    languageId, _storeContext.CurrentStore.Id);

                //reset cache
                _cachedLanguage = null;
            }
        }

        /// <summary>
        /// 获取或设置当前用户工作货币
        /// </summary>
        public virtual Currency WorkingCurrency
        {
            get
            {
                if (_cachedCurrency != null)
                    return _cachedCurrency;
                
                //return primary store currency when we're in admin area/mode
                if (this.IsAdmin)
                {
                    var primaryStoreCurrency =  _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
                    if (primaryStoreCurrency != null)
                    {
                        //cache
                        _cachedCurrency = primaryStoreCurrency;
                        return primaryStoreCurrency;
                    }
                }

                var allCurrencies = _currencyService.GetAllCurrencies(storeId: _storeContext.CurrentStore.Id);
                //find a currency previously selected by a customer
                var currencyId = this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.CurrencyId,
                    _genericAttributeService, _storeContext.CurrentStore.Id);
                var currency = allCurrencies.FirstOrDefault(x => x.Id == currencyId);
                if (currency == null)
                {
                    //it not found, then let's load the default currency for the current language (if specified)
                    currencyId = this.WorkingLanguage.DefaultCurrencyId;
                    currency = allCurrencies.FirstOrDefault(x => x.Id == currencyId);
                }
                if (currency == null)
                {
                    //it not found, then return the first (filtered by current store) found one
                    currency = allCurrencies.FirstOrDefault();
                }
                if (currency == null)
                {
                    //it not specified, then return the first found one
                    currency = _currencyService.GetAllCurrencies().FirstOrDefault();
                }

                //cache
                _cachedCurrency = currency;
                return _cachedCurrency;
            }
            set
            {
                var currencyId = value != null ? value.Id : 0;
                _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                    SystemCustomerAttributeNames.CurrencyId,
                    currencyId, _storeContext.CurrentStore.Id);

                //reset cache
                _cachedCurrency = null;
            }
        }

        /// <summary>
        /// 获取或设置当前的税显示类型
        /// </summary>
        public virtual TaxDisplayType TaxDisplayType
        {
            get
            {
                //cache
                if (_cachedTaxDisplayType != null)
                    return _cachedTaxDisplayType.Value;

                TaxDisplayType taxDisplayType;
                if (_taxSettings.AllowCustomersToSelectTaxDisplayType && this.CurrentCustomer != null)
                {
                    taxDisplayType = (TaxDisplayType) this.CurrentCustomer.GetAttribute<int>(
                        SystemCustomerAttributeNames.TaxDisplayTypeId,
                        _genericAttributeService,
                        _storeContext.CurrentStore.Id);
                }
                else
                {
                    taxDisplayType = _taxSettings.TaxDisplayType;
                }

                //cache
                _cachedTaxDisplayType = taxDisplayType;
                return _cachedTaxDisplayType.Value;

            }
            set
            {
                if (!_taxSettings.AllowCustomersToSelectTaxDisplayType)
                    return;

                _genericAttributeService.SaveAttribute(this.CurrentCustomer, 
                    SystemCustomerAttributeNames.TaxDisplayTypeId,
                    (int)value, _storeContext.CurrentStore.Id);

                //reset cache
                _cachedTaxDisplayType = null;

            }
        }

        /// <summary>
        /// 获取或设置值，指示我们是否在管理区域
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
