using System;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Services.Common;

namespace Nop.Web.Framework
{
    /// <summary>
    /// 最近访问的页面属性
    /// </summary>
    public class StoreLastVisitedPageAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// action执行时
        /// </summary>
        /// <param name="filterContext">上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //不对子方法应用过滤器
            if (filterContext.IsChildAction)
                return;

            //only GET requests
            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var customerSettings = EngineContext.Current.Resolve<CustomerSettings>();
            if (!customerSettings.StoreLastVisitedPage)
                return;

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var pageUrl = webHelper.GetThisPageUrl(true);
            if (!string.IsNullOrEmpty(pageUrl))
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();

                var previousPageUrl = workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.LastVisitedPage);
                if (!pageUrl.Equals(previousPageUrl))
                {
                    genericAttributeService.SaveAttribute(workContext.CurrentCustomer, SystemCustomerAttributeNames.LastVisitedPage, pageUrl);
                }
            }
        }
    }
}
