
using System.Collections.Generic;

namespace Nop.Core
{
    /// <summary>
    /// 分页列表界面
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// 页面索引
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 总数
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 有上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 有下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
