//Contributor : MVCContrib

namespace Nop.Web.Framework.UI.Paging
{
    /// <summary>
    /// 已经拆分成页面的对象的集合。
    /// </summary>
    public interface IPageableModel
    {
        /// <summary>
        /// 当前页面索引（从0开始）
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        int PageNumber { get; }
        /// <summary>
        /// 每页中的项目数。
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 总数。
        /// </summary>
        int TotalItems { get; }
        /// <summary>
        /// 总页数。
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 页面中第一个项目的索引。
        /// </summary>
        int FirstItem { get; }
        /// <summary>
        /// 页面最后一个项目的索引。
        /// </summary>
        int LastItem { get; }
        /// <summary>
        /// 当前页面之前是否有页面。
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 是否有当前页面之后的页面。
        /// </summary>
        bool HasNextPage { get; }
	}


    /// <summary>
    /// 反省 <see cref="IPageableModel"/>
    /// </summary>
    /// <typeparam name="T">正在分页的对象类型</typeparam>
    public interface IPagination<T> : IPageableModel
	{

	}
}