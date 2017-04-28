using Nop.Core.Plugins;

namespace Nop.Web.Framework.Menu
{
    /// <summary>
    /// 在管理区域菜单中有一些项目的插件接口
    /// </summary>
    public interface IAdminMenuPlugin : IPlugin
    {
        /// <summary>
        /// 管理站点地图 您可以使用菜单项的“SystemName”来管理现有的站点地图或添加新的菜单项。
        /// </summary>
        /// <param name="rootNode">站点地图的根节点。</param>
        void ManageSiteMap(SiteMapNode rootNode);
    }
}
