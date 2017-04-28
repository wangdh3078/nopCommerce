using System;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Seo;
using Nop.Core.Infrastructure;

namespace Nop.Web.Framework.Seo
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WwwRequirementAttribute : FilterAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            //不对子方法应用过滤器
            if (filterContext.IsChildAction)
                return;

            // 仅重定向GET请求，
            // 否则浏览器可能不会正确地传播动词和请求正文。
            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            //忽略本地规则
            if (filterContext.HttpContext.Request.IsLocal)
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;
            var seoSettings = EngineContext.Current.Resolve<SeoSettings>();

            switch (seoSettings.WwwRequirement)
            {
                case WwwRequirement.WithWww:
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    string url = webHelper.GetThisPageUrl(true);
                    var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                    if (currentConnectionSecured)
                    {
                        bool startsWith3W = url.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);
                        if (!startsWith3W)
                        {
                            url = url.Replace("https://", "https://www.");

                                //301（永久）重定向
                                filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    else
                    {
                        bool startsWith3W = url.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);
                        if (!startsWith3W)
                        {
                            url = url.Replace("http://", "http://www.");

                                //301（永久）重定向
                                filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                }
                    break;
                case WwwRequirement.WithoutWww:
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    string url = webHelper.GetThisPageUrl(true);
                    var currentConnectionSecured = webHelper.IsCurrentConnectionSecured();
                    if (currentConnectionSecured)
                    {
                        bool startsWith3W = url.StartsWith("https://www.", StringComparison.OrdinalIgnoreCase);
                        if (startsWith3W)
                        {
                            url = url.Replace("https://www.", "https://");

                                //301（永久）重定向
                                filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    else
                    {
                        bool startsWith3W = url.StartsWith("http://www.", StringComparison.OrdinalIgnoreCase);
                        if (startsWith3W)
                        {
                            url = url.Replace("http://www.", "http://");

                                //301（永久）重定向
                                filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                }
                    break;
                case WwwRequirement.NoMatter:
                {
                    //do nothing
                }
                break;
                default:
                    throw new NopException("Not supported WwwRequirement parameter");
            }
        }
    }
}
