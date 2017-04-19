using System;
using System.Linq;
using System.Runtime.Caching;

namespace Nop.Core.Caching
{
    /// <summary>
    /// 代表HTTP请求之间的缓存管理器（长期缓存）
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// 缓存对象
        /// </summary>
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        ///获取或设置与指定键相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>与指定键相关联的值。</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 将指定的键和对象添加到缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        ///获取一个值，指示与指定键相关联的值是否被缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 从缓存中删除指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 通过模式删除缓存
        /// </summary>
        /// <param name="pattern">模式</param>
        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        /// 回收
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}