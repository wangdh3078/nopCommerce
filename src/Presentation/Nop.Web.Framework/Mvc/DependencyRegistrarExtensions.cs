using System;
using Autofac;
using Nop.Core.Data;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;

namespace Nop.Web.Framework.Mvc
{
    /// <summary>
    /// 依赖注入的扩展
    /// </summary>
    public static class DependencyRegistrarExtensions
    {
        /// <summary>
        /// 注册插件的自定义数据上下文
        /// </summary>
        /// <typeparam name="T">实现IDbContext的类</typeparam>
        /// <param name="dependencyRegistrar">依赖注入</param>
        /// <param name="builder">Builder</param>
        /// <param name="contextName">上下文名称</param>
        public static void RegisterPluginDataContext<T>(this IDependencyRegistrar dependencyRegistrar,
            ContainerBuilder builder, string contextName)
             where T: IDbContext
        {
            //data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();

            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                //注册命名上下文
                builder.Register(c => (IDbContext)Activator.CreateInstance(typeof(T), new object[] { dataProviderSettings.DataConnectionString }))
                    .Named<IDbContext>(contextName)
                    .InstancePerLifetimeScope();

                builder.Register(c => (T)Activator.CreateInstance(typeof(T), new object[] { dataProviderSettings.DataConnectionString }))
                    .InstancePerLifetimeScope();
            }
            else
            {
                //注册命名上下文
                builder.Register(c => (T)Activator.CreateInstance(typeof(T), new object[] { c.Resolve<DataSettings>().DataConnectionString }))
                    .Named<IDbContext>(contextName)
                    .InstancePerLifetimeScope();

                builder.Register(c => (T)Activator.CreateInstance(typeof(T), new object[] { c.Resolve<DataSettings>().DataConnectionString }))
                    .InstancePerLifetimeScope();
            }
        }
    }
}
