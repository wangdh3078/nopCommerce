using System.Collections.Generic;

namespace Nop.Web.Framework.Themes
{
    /// <summary>
    /// 主题驱动
    /// </summary>
    public partial interface IThemeProvider
    {
        /// <summary>
        /// 通过主题名称获取主题配置
        /// </summary>
        /// <param name="themeName">主题名</param>
        /// <returns></returns>
        ThemeConfiguration GetThemeConfiguration(string themeName);
        /// <summary>
        /// 获取全部主题配置
        /// </summary>
        /// <returns></returns>

        IList<ThemeConfiguration> GetThemeConfigurations();

        /// <summary>
        /// 主题配置是否存在
        /// </summary>
        /// <param name="themeName">主题名</param>
        /// <returns></returns>
        bool ThemeConfigurationExists(string themeName);
    }
}
