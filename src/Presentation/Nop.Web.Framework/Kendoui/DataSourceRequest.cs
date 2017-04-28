namespace Nop.Web.Framework.Kendoui
{
    /// <summary>
    /// 数据源请求
    /// </summary>
    public class DataSourceRequest
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataSourceRequest()
        {
            this.Page = 1;
            this.PageSize = 10;
        }
    }
}
