using System;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Nop.Core.Infrastructure
{
    /// <summary>
    /// NOP引擎接口
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        ///注入容器管理
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// 在nop环境中初始化组件和插件。
        /// </summary>
        /// <param name="config">配置信息</param>
        void Initialize(NopConfig config);

        /// <summary>
        /// 依赖解析
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// 依赖解析
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 依赖解析
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}
