using System;

namespace Nop.Core.Caching
{
    /// <summary>
    /// 缓存管理接口
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// 获取或设置与指定键相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>与指定键相关联的值。</returns>
        T Get<T>(string key);

        /// <summary>
        /// 将指定的键和对象添加到缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cacheTime">缓存时间</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// 获取一个值，指示与指定键相关联的值是否被缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>true-已缓存，false-未缓存</returns>
        bool IsSet(string key);

        /// <summary>
        /// 从缓存中删除指定键的值
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 通过指定模式删除缓存
        /// </summary>
        /// <param name="pattern">模式</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        void Clear();
    }
}
