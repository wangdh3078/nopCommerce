using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;

namespace Nop.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 依赖注入容器管理
    /// </summary>
    public class ContainerManager
    {
        /// <summary>
        /// 依赖注入容器
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">容器</param>
        public ContainerManager(IContainer container)
        {
            this._container = container;
        }

        /// <summary>
        ///获取依赖注入容器
        /// </summary>
        public virtual IContainer Container
        {
            get
            {
                return _container;
            }
        }

        /// <summary>
        /// 解析服务
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns>解析后的服务</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// 解析服务
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns>解析后的服务</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// 解析所有
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns>解析后的服务集合</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// 解析未注册的服务
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns>解析后的服务</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T:class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// 解析未注册的服务
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns>解析后的服务</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new NopException("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (NopException)
                {

                }
            }
            throw new NopException("No constructor  was found that had all the dependencies satisfied.");
        }

        /// <summary>
        ///尝试解析服务
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <param name="instance">解析后的服务</param>
        /// <returns>指示服务是否已成功解决的值</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        ///检查一些服务是否已注册（可以被解析）
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns></returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// Resolve optional
        /// </summary>
        /// <param name="serviceType">类型</param>
        /// <param name="scope">范围; 传递null以自动解析当前作用域</param>
        /// <returns>解析后的服务</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //没有指定范围
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// 获取当前范围
        /// </summary>
        /// <returns>范围</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                    return AutofacDependencyResolver.Current.RequestLifetimeScope;

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}
