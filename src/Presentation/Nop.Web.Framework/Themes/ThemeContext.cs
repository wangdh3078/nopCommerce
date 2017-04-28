using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Customers;
using Nop.Services.Common;

namespace Nop.Web.Framework.Themes
{
    /// <summary>
    /// T主题上下文
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly IThemeProvider _themeProvider;

        private bool _themeIsCached;
        private string _cachedThemeName;

        public ThemeContext(IWorkContext workContext,
            IStoreContext storeContext,
            IGenericAttributeService genericAttributeService, 
            StoreInformationSettings storeInformationSettings, 
            IThemeProvider themeProvider)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._genericAttributeService = genericAttributeService;
            this._storeInformationSettings = storeInformationSettings;
            this._themeProvider = themeProvider;
        }

        /// <summary>
        /// 获取或设置当前主题系统名称
        /// </summary>
        public string WorkingThemeName
        {
            get
            {
                if (_themeIsCached)
                    return _cachedThemeName;

                string theme = "";
                if (_storeInformationSettings.AllowCustomerToSelectTheme)
                {
                    if (_workContext.CurrentCustomer != null)
                        theme = _workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.WorkingThemeName, _genericAttributeService, _storeContext.CurrentStore.Id);
                }

                //默认商店主题
                if (string.IsNullOrEmpty(theme))
                    theme = _storeInformationSettings.DefaultStoreTheme;

                //确保主题存在
                if (!_themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = _themeProvider.GetThemeConfigurations()
                        .FirstOrDefault();
                    if (themeInstance == null)
                        throw new Exception("No theme could be loaded");
                    theme = themeInstance.ThemeName;
                }
                
                //缓存主题
                this._cachedThemeName = theme;
                this._themeIsCached = true;
                return theme;
            }
            set
            {
                if (!_storeInformationSettings.AllowCustomerToSelectTheme)
                    return;

                if (_workContext.CurrentCustomer == null)
                    return;

                _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, SystemCustomerAttributeNames.WorkingThemeName, value, _storeContext.CurrentStore.Id);

                //清除缓存
                this._themeIsCached = false;
            }
        }
    }
}
