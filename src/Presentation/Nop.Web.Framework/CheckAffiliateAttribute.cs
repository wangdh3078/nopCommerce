using System;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Affiliates;
using Nop.Core.Infrastructure;
using Nop.Services.Affiliates;
using Nop.Services.Customers;

namespace Nop.Web.Framework
{
    /// <summary>
    /// 检查会员属性
    /// </summary>
    public class CheckAffiliateAttribute : ActionFilterAttribute
    {
        private const string AFFILIATE_ID_QUERY_PARAMETER_NAME = "affiliateid";
        private const string AFFILIATE_FRIENDLYURLNAME_QUERY_PARAMETER_NAME = "affiliate";

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

            Affiliate affiliate = null;

            if (request.QueryString != null)
            {
                //尝试通过ID查找（“affiliateId”参数）
                if (request.QueryString[AFFILIATE_ID_QUERY_PARAMETER_NAME] != null)
                {
                    var affiliateId = Convert.ToInt32(request.QueryString[AFFILIATE_ID_QUERY_PARAMETER_NAME]);
                    if (affiliateId > 0)
                    {
                        var affiliateService = EngineContext.Current.Resolve<IAffiliateService>();
                        affiliate = affiliateService.GetAffiliateById(affiliateId);
                    }
                }
                //尝试通过友好名称（“affiliate”参数）查找
                else if (request.QueryString[AFFILIATE_FRIENDLYURLNAME_QUERY_PARAMETER_NAME] != null)
                {
                    var friendlyUrlName = request.QueryString[AFFILIATE_FRIENDLYURLNAME_QUERY_PARAMETER_NAME];
                    if (!string.IsNullOrEmpty(friendlyUrlName))
                    {
                        var affiliateService = EngineContext.Current.Resolve<IAffiliateService>();
                        affiliate = affiliateService.GetAffiliateByFriendlyUrlName(friendlyUrlName);
                    }
                }
            }


            if (affiliate != null && !affiliate.Deleted && affiliate.Active)
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                if (workContext.CurrentCustomer.AffiliateId != affiliate.Id)
                {
                    workContext.CurrentCustomer.AffiliateId = affiliate.Id;
                    var customerService = EngineContext.Current.Resolve<ICustomerService>();
                    customerService.UpdateCustomer(workContext.CurrentCustomer);
                }
            }
        }
    }
}
