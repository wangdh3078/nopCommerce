using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nop.Core.Caching
{
    /// <summary>
    /// 缓存扩展
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        ///获取缓存项目。 如果尚未缓存，则加载并缓存它
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存管理</param>
        /// <param name="key">键</param>
        /// <param name="acquire">如果不在缓存中，则加载项目的功能</param>
        /// <returns>缓存数据</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// 获取缓存项目。 如果尚未缓存，则加载并缓存它
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存管理</param>
        /// <param name="key">键</param>
        /// <param name="cacheTime">缓存时间（分钟）（0 - 不缓存）</param>
        /// <param name="acquire">如果不在缓存中，则加载项目的功能</param>
        /// <returns>缓存数据</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        /// <summary>
        /// 通过模式移除缓存
        /// </summary>
        /// <param name="cacheManager">缓存对象</param>
        /// <param name="pattern">模式</param>
        /// <param name="keys">缓存中的所有键</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }
    }
}
