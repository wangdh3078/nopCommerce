using System.Web;

namespace Nop.Core
{
    /// <summary>
    /// Web帮助类
    /// </summary>
    public partial interface IWebHelper
    {
        /// <summary>
        /// 获取URL连接
        /// </summary>
        /// <returns>URL连接</returns>
        string GetUrlReferrer();

        /// <summary>
        /// 获取上下文IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        string GetCurrentIpAddress();

        /// <summary>
        ///获取页面名称
        /// </summary>
        /// <param name="includeQueryString">指示是否包含查询字符串的值</param>
        /// <returns>页面名称</returns>
        string GetThisPageUrl(bool includeQueryString);

        /// <summary>
        /// 获取页面名称
        /// </summary>
        /// <param name="includeQueryString">指示是否包含查询字符串的值</param>
        /// <param name="useSsl">指示是否获得SSL保护页的值</param>
        /// <returns>页面名称</returns>
        string GetThisPageUrl(bool includeQueryString, bool useSsl);

        /// <summary>
        /// 获取一个值，指示当前连接是否被保护
        /// </summary>
        /// <returns>True - 安全，false - 不安全</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// 通过名称获取服务器变量
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>服务器变量</returns>
        string ServerVariables(string name);

        /// <summary>
        ///获取主机位置
        /// </summary>
        /// <param name="useSsl">使用SSL</param>
        /// <returns>存储主机位置</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        string GetStoreLocation();

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store location</returns>
        string GetStoreLocation(bool useSsl);

        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file.</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        bool IsStaticResource(HttpRequest request);

        /// <summary>
        /// 修改查询字符串
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>New url</returns>
        string ModifyQueryString(string url, string queryStringModification, string anchor);

        /// <summary>
        /// Remove query string from url
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryString">Query string to remove</param>
        /// <returns>New url</returns>
        string RemoveQueryString(string url, string queryString);

        /// <summary>
        /// 通过名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">参数名称</param>
        /// <returns>查询字符串值</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// 重新启动应用程序域
        /// </summary>
        /// <param name="makeRedirect">一个值，表示重启后是否应该重定向</param>
        /// <param name="redirectUrl">重定向网址; 如果要重定向到当前页面URL，则为空字符串</param>
        void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "");

        /// <summary>
        /// 获取一个值，指示客户端是否被重定向到新位置
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// 获取或设置一个值，该值指示客户端是否使用POST重定向到新位置
        /// </summary>
        bool IsPostBeingDone { get; set; }
    }
}
