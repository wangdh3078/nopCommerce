using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Nop.Services.Security;

namespace Nop.Web.Framework.Controllers
{
    /// <summary>
    /// 管理授权属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited=true, AllowMultiple=true)]
    public class AdminAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        //不验证
        private readonly bool _dontValidate;

        /// <summary>
        /// 构造函数
        /// </summary>

        public AdminAuthorizeAttribute()
            : this(false)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dontValidate">不验证</param>
        public AdminAuthorizeAttribute(bool dontValidate)
        {
            this._dontValidate = dontValidate;
        }

        /// <summary>
        /// 处理未经授权的请求
        /// </summary>
        /// <param name="filterContext"></param>
        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
        /// <summary>
        /// 获取管理授权属性
        /// </summary>
        /// <param name="descriptor">动作描述符</param>
        /// <returns></returns>
        private IEnumerable<AdminAuthorizeAttribute> GetAdminAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true))
                .OfType<AdminAuthorizeAttribute>();
        }

        /// <summary>
        /// 是否请求管理页面
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool IsAdminPageRequested(AuthorizationContext filterContext)
        {
            var adminAttributes = GetAdminAuthorizeAttributes(filterContext.ActionDescriptor);
            if (adminAttributes != null && adminAttributes.Any())
                return true;
            return false;
        }
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_dontValidate)
                return;

            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException("You cannot use [AdminAuthorize] attribute when a child action cache is active");

            if (IsAdminPageRequested(filterContext))
            {
                if (!this.HasAdminAccess())
                    this.HandleUnauthorizedRequest(filterContext);
            }
        }
        /// <summary>
        /// 具有管理员权限
        /// </summary>
        /// <returns></returns>
        public virtual bool HasAdminAccess()
        {
            var permissionService = EngineContext.Current.Resolve<IPermissionService>();
            bool result = permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel);
            return result;
        }
    }
}
