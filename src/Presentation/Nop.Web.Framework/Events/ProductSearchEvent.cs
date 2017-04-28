using System.Collections.Generic;

namespace Nop.Web.Framework.Events
{
    /// <summary>
    /// 产品搜索事件
    /// </summary>
    public class ProductSearchEvent
    {
        /// <summary>
        /// 搜索词
        /// </summary>
        public string SearchTerm { get; set; }
        /// <summary>
        /// 搜索描述
        /// </summary>
        public bool SearchInDescriptions { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public IList<int> CategoryIds { get; set; }
        /// <summary>
        /// 制造商编号
        /// </summary>
        public int ManufacturerId { get; set; }
        /// <summary>
        /// 语言ID
        /// </summary>
        public int WorkingLanguageId { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int VendorId { get; set; }
    }
}
