using Nop.Core.Configuration;

namespace Nop.Core.Domain
{
    public class StoreInformationSettings : ISettings
    {
        /// <summary>
        /// 获取或设置一个值，指示是否显示"powered by nopCommerce" 文本。
        /// </summary>
        public bool HidePoweredByNopCommerce { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示商店是否关闭
        /// </summary>
        public bool StoreClosed { get; set; }

        /// <summary>
        /// 获取或设置徽标的图片标识符。 如果为0，则将使用默认值
        /// </summary>
        public int LogoPictureId { get; set; }

        /// <summary>
        /// 获取或设置默认商店主题
        /// </summary>
        public string DefaultStoreTheme { get; set; }

        /// <summary>
        ///获取或设置一个值，指示是否允许客户选择主题
        /// </summary>
        public bool AllowCustomerToSelectTheme { get; set; }

        /// <summary>
        ///获取或设置一个值，该值指示mini分析器是否应显示在公共存储中（用于调试）
        /// </summary>
        public bool DisplayMiniProfilerInPublicStore { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应仅在访问管理区域的用户显示mini分析器
        /// </summary>
        public bool DisplayMiniProfilerForAdminOnly { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否应该显示有关新的欧盟Cookie法律的警告
        /// </summary>
        public bool DisplayEuCookieLawWarning { get; set; }

        /// <summary>
        /// 获取或设置网站的Facebook页面URL的值
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// 获取或设置网站的Twitter页面URL的值
        /// </summary>
        public string TwitterLink { get; set; }

        /// <summary>
        ///获取或设置网站的YouTube频道网址值
        /// </summary>
        public string YoutubeLink { get; set; }

        /// <summary>
        /// 获取或设置网站的Google+信息页网址值
        /// </summary>
        public string GooglePlusLink { get; set; }
    }
}
