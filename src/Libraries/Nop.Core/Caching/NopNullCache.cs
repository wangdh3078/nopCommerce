namespace Nop.Core.Caching
{
    /// <summary>
    /// 代表一个NopNullCache（没有缓存）
    /// </summary>
    public partial class NopNullCache : ICacheManager
    {
        /// <summary>
        /// 获取或设置与指定键相关联的值。
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="key">键</param>
        /// <returns>与指定键相关联的值。</returns>
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        ///将指定的键和对象添加到缓存。
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        /// <param name="cacheTime">缓存时间</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        /// <summary>
        /// 获取一个值，指示与指定键相关联的值是否被缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <summary>
        /// 从缓存中删除指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
        }

        /// <summary>
        /// 通过模式删除缓存
        /// </summary>
        /// <param name="pattern">模式</param>
        public virtual void RemoveByPattern(string pattern)
        {
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// 回收
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}