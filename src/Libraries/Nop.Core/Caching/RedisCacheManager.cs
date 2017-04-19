using System;
using System.Text;
using Newtonsoft.Json;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using StackExchange.Redis;

namespace Nop.Core.Caching
{
    /// <summary>
    /// 表示一种对redis存储缓存管理器(http://redis.io/).
    /// 大多数情况下，它将在Web或Azure中运行时使用
    /// 当然也可以在任何服务器或环境中使用
    /// </summary>
    public partial class RedisCacheManager : ICacheManager
    {
        #region 字段
        /// <summary>
        /// Redis链接包装
        /// </summary>
        private readonly IRedisConnectionWrapper _connectionWrapper;
        /// <summary>
        /// Redis数据库
        /// </summary>
        private readonly IDatabase _db;
        /// <summary>
        /// 缓存管理对象
        /// </summary>
        private readonly ICacheManager _perRequestCacheManager;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="connectionWrapper">Redis链接包装</param>

        public RedisCacheManager(NopConfig config, IRedisConnectionWrapper connectionWrapper)
        {
            if (String.IsNullOrEmpty(config.RedisCachingConnectionString))
                throw new Exception("Redis connection string is empty");

            // ConnectionMultiplexer.Connect只应该被调用一次，并且被共享
            this._connectionWrapper = connectionWrapper;

            this._db = _connectionWrapper.GetDatabase();
            this._perRequestCacheManager = EngineContext.Current.Resolve<ICacheManager>();
        }

        #endregion

        #region 方法

        /// <summary>
        ///获取或设置与指定键相关联的值。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>与指定键相关联的值。</returns>
        public virtual T Get<T>(string key)
        {
            //这里的性能变化不大
            //我们使用“PerRequestCacheManager”将当前HTTP请求的加载对象缓存在内存中。
            //这样我们将不会每次HTTP请求连接到Redis服务器500次（例如每次加载语言环境或设置）
            if (_perRequestCacheManager.IsSet(key))
                return _perRequestCacheManager.Get<T>(key);

            var rValue = _db.StringGet(key);
            if (!rValue.HasValue)
                return default(T);
            var result = Deserialize<T>(rValue);

            _perRequestCacheManager.Set(key, result, 0);
            return result;
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

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }

        /// <summary>
        ///获取一个值，指示与指定键相关联的值是否被缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            //little performance workaround here:
            //we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server 500 times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCacheManager.IsSet(key))
                return true;

            return _db.KeyExists(key);
        }

        /// <summary>
        ///从缓存中删除指定键的值
        /// </summary>
        /// <param name="key">键</param>
        public virtual void Remove(string key)
        {
            _db.KeyDelete(key);
            _perRequestCacheManager.Remove(key);
        }

        /// <summary>
        /// 通过模式删除缓存对象
        /// </summary>
        /// <param name="pattern">模式</param>
        public virtual void RemoveByPattern(string pattern)
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);
                var keys = server.Keys(database: _db.Database, pattern: "*" + pattern + "*");
                foreach (var key in keys)
                    Remove(key);
            }
        }

        /// <summary>
        /// 清除所有缓存对象
        /// </summary>
        public virtual void Clear()
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);
                //我们可以使用下面的代码（注释）
                //但它需要管理权限 - “，allowAdmin = true”
                //server.FlushDatabase();

                //这就是为什么我们现在简单地遍历所有元素
                var keys = server.Keys(database: _db.Database);
                foreach (var key in keys)
                    Remove(key);
            }
        }

        /// <summary>
        /// 回收
        /// </summary>
        public virtual void Dispose()
        {
            //if (_connectionWrapper != null)
            //    _connectionWrapper.Dispose();
        }

        #endregion

        #region Utilities
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="item">序列化对象</param>
        /// <returns></returns>
        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="serializedObject">反序列化对象</param>
        /// <returns></returns>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion

    }
}
