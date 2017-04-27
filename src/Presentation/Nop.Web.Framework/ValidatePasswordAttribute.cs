using System;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;

namespace Nop.Web.Framework
{
    /// <summary>
    /// 过滤器属性以验证客户密码到期
    /// </summary>
    public class ValidatePasswordAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在执行Action方法之前，由ASP.NET MVC框架调用
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //不对子方法应用过滤器
            if (filterContext.IsChildAction)
                return;

            var actionName = filterContext.ActionDescriptor.ActionName;
            if (string.IsNullOrEmpty(actionName) || actionName.Equals("ChangePassword", StringComparison.InvariantCultureIgnoreCase))
                return;

            var controllerName = filterContext.Controller.ToString();
            if (string.IsNullOrEmpty(controllerName) || controllerName.Equals("Customer", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //获取当前用户
            var customer = EngineContext.Current.Resolve<IWorkContext>().CurrentCustomer;

            //检查密码到期
            if (customer.PasswordIsExpired())
            {
                var changePasswordUrl = new UrlHelper(filterContext.RequestContext).RouteUrl("CustomerChangePassword");
                filterContext.Result = new RedirectResult(changePasswordUrl);
            }
        }
    }
}
