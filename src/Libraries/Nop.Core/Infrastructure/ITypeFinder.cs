using System;
using System.Collections.Generic;
using System.Reflection;

namespace Nop.Core.Infrastructure
{
    /// <summary>
    /// 类型查找
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns></returns>
        IList<Assembly> GetAssemblies();
        /// <summary>
        /// 通过类型查找类
        /// </summary>
        /// <param name="assignTypeFrom">查找类型</param>
        /// <param name="onlyConcreteClasses">只查找具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);
        /// <summary>
        /// 通过类型查找类
        /// </summary>
        /// <param name="assignTypeFrom">查找类型</param>
        /// <param name="assemblies">程序集集合</param>
        /// <param name="onlyConcreteClasses">只查找具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
        /// <summary>
        /// 通过类型查找类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="onlyConcreteClasses">只查找具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// 通过类型查找类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="assemblies">程序集集合</param>
        /// <param name="onlyConcreteClasses">只查找具体类</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
