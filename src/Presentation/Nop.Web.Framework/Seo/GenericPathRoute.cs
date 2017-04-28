using System;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Services.Events;
using Nop.Services.Seo;
using Nop.Web.Framework.Localization;

namespace Nop.Web.Framework.Seo
{
    /// <summary>
    /// 提供定义SEO友好路由的属性和方法，以及获取有关路由的信息。
    /// </summary>
    public partial class GenericPathRoute : LocalizedRoute
    {
        #region 构造函数

        /// <summary>
        /// 使用指定的URL模式和处理程序类初始化System.Web.Routing.Route类的新实例。
        /// </summary>
        /// <param name="url">路由的URL模式。</param>
        /// <param name="routeHandler">处理路由请求的对象。</param>
        public GenericPathRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        /// <summary>
        ///使用指定的URL模式，处理程序类和默认参数值初始化System.Web.Routing.Route类的新实例。
        /// </summary>
        /// <param name="url">路由的URL模式。</param>
        /// <param name="defaults">如果URL不包含所有参数，则使用的值。</param>
        /// <param name="routeHandler">处理路由请求的对象。</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        /// <summary>
        /// 初始化System.Web.Routing.Route类的新实例，使用指定的URL模式，处理程序类，默认参数值和约束。
        /// </summary>
        /// <param name="url">路由的URL模式。</param>
        /// <param name="defaults">如果URL不包含所有参数，则使用的值。</param>
        /// <param name="constraints">指定URL参数有效值的正则表达式。</param>
        /// <param name="routeHandler">处理路由请求的对象。</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        /// <summary>
        /// 使用指定的URL模式，处理程序类，默认参数值，约束和自定义值初始化System.Web.Routing.Route类的新实例。
        /// </summary>
        /// <param name="url">路由的URL模式。</param>
        /// <param name="defaults">如果URL不包含所有参数，则使用的值。</param>
        /// <param name="constraints">指定URL参数有效值的正则表达式。</param>
        /// <param name="dataTokens">传递给路由处理程序但不用于确定路由是否匹配特定URL模式的自定义值。 路由处理程序可能需要这些值来处理请求。</param>
        /// <param name="routeHandler">处理路由请求的对象。</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        #endregion

        #region 方法

        /// <summary>
        /// 返回有关请求路由的信息。
        /// </summary>
        /// <param name="httpContext">封装有关HTTP请求的信息的对象。</param>
        /// <returns>
        /// 包含路由定义中的值的对象。
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData data = base.GetRouteData(httpContext);
            if (data != null && DataSettingsHelper.DatabaseIsInstalled())
            {
                var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
                var slug = data.Values["generic_se_name"] as string;
                //性能优化。
                //我们在这里加载缓存版本。 它减少了每个页面加载的SQL请求数
                var urlRecord = urlRecordService.GetBySlugCached(slug);
                //注释上面的一行，取消注释下面的行以禁用此性能“解决方法”
                //var urlRecord = urlRecordService.GetBySlug(slug);
                if (urlRecord == null)
                {
                    //no URL record found

                    //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    //var response = httpContext.Response;
                    //response.Status = "302 Found";
                    //response.RedirectLocation = webHelper.GetStoreLocation(false);
                    //response.End();
                    //return null;

                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                    return data;
                }
                //ensure that URL record is active
                if (!urlRecord.IsActive)
                {
                    //URL record is not active. let's find the latest one
                    var activeSlug = urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, urlRecord.LanguageId);
                    if (string.IsNullOrWhiteSpace(activeSlug))
                    {
                        //no active slug found

                        //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        //var response = httpContext.Response;
                        //response.Status = "302 Found";
                        //response.RedirectLocation = webHelper.GetStoreLocation(false);
                        //response.End();
                        //return null;

                        data.Values["controller"] = "Common";
                        data.Values["action"] = "PageNotFound";
                        return data;
                    }

                    //the active one is found
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    var response = httpContext.Response;
                    response.Status = "301 Moved Permanently";
                    response.RedirectLocation = string.Format("{0}{1}", webHelper.GetStoreLocation(), activeSlug);
                    response.End();
                    return null;
                }

                //ensure that the slug is the same for the current language
                //otherwise, it can cause some issues when customers choose a new language but a slug stays the same
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var slugForCurrentLanguage = SeoExtensions.GetSeName(urlRecord.EntityId, urlRecord.EntityName, workContext.WorkingLanguage.Id);
                if (!string.IsNullOrEmpty(slugForCurrentLanguage) && 
                    !slugForCurrentLanguage.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                {
                    //we should make not null or "" validation above because some entities does not have SeName for standard (ID=0) language (e.g. news, blog posts)
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    var response = httpContext.Response;
                    //response.Status = "302 Found";
                    response.Status = "302 Moved Temporarily";
                    response.RedirectLocation = string.Format("{0}{1}", webHelper.GetStoreLocation(), slugForCurrentLanguage);
                    response.End();
                    return null;
                }

                //process URL
                switch (urlRecord.EntityName.ToLowerInvariant())
                {
                    case "product":
                        {
                            data.Values["controller"] = "Product";
                            data.Values["action"] = "ProductDetails";
                            data.Values["productid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "category":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Category";
                            data.Values["categoryid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "manufacturer":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Manufacturer";
                            data.Values["manufacturerid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "vendor":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Vendor";
                            data.Values["vendorid"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "newsitem":
                        {
                            data.Values["controller"] = "News";
                            data.Values["action"] = "NewsItem";
                            data.Values["newsItemId"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "blogpost":
                        {
                            data.Values["controller"] = "Blog";
                            data.Values["action"] = "BlogPost";
                            data.Values["blogPostId"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    case "topic":
                        {
                            data.Values["controller"] = "Topic";
                            data.Values["action"] = "TopicDetails";
                            data.Values["topicId"] = urlRecord.EntityId;
                            data.Values["SeName"] = urlRecord.Slug;
                        }
                        break;
                    default:
                        {
                            //no record found

                            //generate an event this way developers could insert their own types
                            EngineContext.Current.Resolve<IEventPublisher>()
                                .Publish(new CustomUrlRecordEntityNameRequested(data, urlRecord));
                        }
                        break;
                }
            }
            return data;
        }

        #endregion
    }
}