using System;
using System.Web;
using System.Web.Routing;

namespace Nop.Web.Framework.Mvc.Routes
{
    /// <summary>
    /// GUID约束
    /// </summary>
    public class GuidConstraint : IRouteConstraint
    {
        //允许空
        private readonly bool _allowEmpty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="allowEmpty">允许空</param>
        public GuidConstraint(bool allowEmpty)
        {
            this._allowEmpty = allowEmpty;
        }
        /// <summary>
        /// 确定 URL 参数是否包含此约束的有效值。
        /// </summary>
        /// <param name="httpContext">一个对象，封装有关 HTTP 请求的信息。</param>
        /// <param name="route">此约束所属的对象。</param>
        /// <param name="parameterName">正在检查的参数的名称。</param>
        /// <param name="values"> 一个包含 URL 的参数的对象。。</param>
        /// <param name="routeDirection">一个对象，指示在处理传入请求或生成 URL 时，是否正在执行约束检查。</param>
        /// <returns>如果 URL 参数包含有效值，则为 true；否则为 false。</returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                string stringValue = values[parameterName] != null ? values[parameterName].ToString() : null;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    Guid guidValue;

                    return Guid.TryParse(stringValue, out guidValue) && 
                        (_allowEmpty || guidValue != Guid.Empty);
                }
            }

            return false;
        }
    }
}
