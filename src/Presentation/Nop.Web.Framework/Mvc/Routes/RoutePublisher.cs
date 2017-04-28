using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;

namespace Nop.Web.Framework.Mvc.Routes
{
    /// <summary>
    /// 路由发布
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {
        protected readonly ITypeFinder typeFinder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="typeFinder">类型查找对象</param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        /// <summary>
        ///通过位于程序集中的某种类型查找插件描述符
        /// </summary>
        /// <param name="providerType">驱动类型</param>
        /// <returns>插件描述</returns>
        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException("providerType");

            foreach (var plugin in PluginManager.ReferencedPlugins)
            {
                if (plugin.ReferencedAssembly == null)
                    continue;

                if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
                    return plugin;
            }

            return null;
        }

        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                //忽略未安装的插件
                var plugin = FindPlugin(providerType);
                if (plugin != null && !plugin.Installed)
                    continue;

                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }
            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterRoutes(routes));
        }
    }
}
