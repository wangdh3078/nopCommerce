
using System.Web.Routing;
using Nop.Services.Seo;

namespace Nop.Web.Framework.Seo
{
    /// <summary>
    /// 事件处理未知的URL记录实体名称
    /// </summary>
    public class CustomUrlRecordEntityNameRequested
    {
        public CustomUrlRecordEntityNameRequested(RouteData routeData, UrlRecordService.UrlRecordForCaching urlRecord)
        {
            this.RouteData = routeData;
            this.UrlRecord = urlRecord;
        }

        public RouteData RouteData { get; private set; }
        public UrlRecordService.UrlRecordForCaching UrlRecord { get; private set; }
    }
}
