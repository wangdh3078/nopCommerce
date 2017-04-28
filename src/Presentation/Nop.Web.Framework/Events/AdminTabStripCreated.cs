using System.Collections.Generic;
using System.Web.Mvc;

namespace Nop.Web.Framework.Events
{
    /// <summary>
    /// 管理标签创建事件
    /// </summary>
    public class AdminTabStripCreated
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="helper">HTML帮助类</param>
        /// <param name="tabStripName">标签名称</param>
        public AdminTabStripCreated(HtmlHelper helper, string tabStripName)
        {
            this.Helper = helper;
            this.TabStripName = tabStripName;
            this.BlocksToRender = new List<MvcHtmlString>();
        }
        /// <summary>
        /// HTML帮助类
        /// </summary>
        public HtmlHelper Helper { get; private set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TabStripName { get; private set; }
        public IList<MvcHtmlString> BlocksToRender { get; set; }
    }
}
