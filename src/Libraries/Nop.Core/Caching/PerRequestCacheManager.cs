using System.Collections;
using System.Linq;
using System.Web;

namespace Nop.Core.Caching
{
    /// <summary>
    /// 代表HTTP请求中缓存的管理员（短期缓存）
    /// </summary>
    public partial class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }

        /// <summary>
        /// 创建NopRequestCache类的新实例
        /// </summary>
        protected virtual IDictionary GetItems()
        {
            if (_context != null)
                return _context.Items;

            return null;
        }

        /// <summary>
        ///获取或设置与指定键相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>与指定键相关联的值。</returns>
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        ///与指定键相关联的值。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        /// <summary>
        /// 获取一个值，该值指示与指定键关联的值是否缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;
            
            return (items[key] != null);
        }

        /// <summary>
        /// 从缓存中移除指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }

        /// <summary>
        /// 通过模式删除缓存对象
        /// </summary>
        /// <param name="pattern">模式</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public virtual void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Clear();
        }

        /// <summary>
        /// 缓存
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
