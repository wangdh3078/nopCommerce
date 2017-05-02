using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;
using Nop.Web.Framework.Localization;

namespace Nop.Web.Framework
{
    /// <summary>
    /// 如果启用了“具有多语言的SEO友好URL”设置，则确保商店URL包含语言SEO代码的属性
    /// </summary>
    public class LanguageSeoCodeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            //不对子方法应用过滤器
            if (filterContext.IsChildAction)
                return;

            //只有GET请求
            if (!string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            var localizationSettings = EngineContext.Current.Resolve<LocalizationSettings>();
            if (!localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                return;

            //确保此路由已注册并可本地化（RouteProvider.cs中的LocalizedRoute）
            if (filterContext.RouteData == null || filterContext.RouteData.Route == null || !(filterContext.RouteData.Route is LocalizedRoute))
                return;


            //处理当前URL
            var pageUrl = request.RawUrl;
            string applicationPath = request.ApplicationPath;
            if (pageUrl.IsLocalizedUrl(applicationPath, true))
            {
                //已经本地化的网址
                //让我们确保这种语言存在
                var seoCode = pageUrl.GetLanguageSeoCodeFromUrl(applicationPath, true);
                
                var languageService = EngineContext.Current.Resolve<ILanguageService>();
                var language = languageService.GetAllLanguages()
                    .FirstOrDefault(l => seoCode.Equals(l.UniqueSeoCode, StringComparison.InvariantCultureIgnoreCase));
                if (language != null && language.Published)
                {
                    //存在
                    return;
                }
                else
                {
                    //不存在 重定向到原始页面（不是永久的）
                    pageUrl = pageUrl.RemoveLanguageSeoCodeFromRawUrl(applicationPath);
                    filterContext.Result = new RedirectResult(pageUrl);
                }
            }
            //添加语言代码到URL
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            pageUrl = pageUrl.AddLanguageSeoCodeToRawUrl(applicationPath, workContext.WorkingLanguage);
            //301（永久）重定向
            filterContext.Result = new RedirectResult(pageUrl, true);
        }
    }
}
