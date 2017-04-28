namespace Nop.Web.Framework.Security
{
    public enum SslRequirement
    {
        /// <summary>
        /// 页面应该被保护
        /// </summary>
        Yes,
        /// <summary>
        /// 页面不应该被保护
        /// </summary>
        No,
        /// <summary>
        /// 没关系（按请求）
        /// </summary>
        NoMatter,
    }
}
