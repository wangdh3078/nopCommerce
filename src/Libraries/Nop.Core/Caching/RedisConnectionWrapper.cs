using System;
using System.Linq;
using System.Net;
using Nop.Core.Configuration;
using RedLock;
using StackExchange.Redis;

namespace Nop.Core.Caching
{
    /// <summary>
    /// Redis连接包装器实现
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        #region 字段

        private readonly NopConfig _config;
        private readonly Lazy<string> _connectionString;

        private volatile ConnectionMultiplexer _connection;
        private volatile RedisLockFactory _redisLockFactory;
        private readonly object _lock = new object();

        #endregion

        #region 构造函数

        public RedisConnectionWrapper(NopConfig config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
            this._redisLockFactory = CreateRedisLockFactory();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取与redis中的数据库的交互式连接
        /// </summary>
        /// <param name="db">数据库号 传递null使用默认值</param>
        /// <returns>Redis缓存数据库</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1); //_settings.DefaultDb);
        }

        /// <summary>
        ///获取单个服务器的配置API
        /// </summary>
        /// <param name="endPoint">网络端点</param>
        /// <returns>Redis服务</returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// 获取服务器上定义的所有端点
        /// </summary>
        /// <returns>端点阵列</returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// 删除数据库的所有键
        /// </summary>
        /// <param name="db">数据库号 传递null使用默认值</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1); //_settings.DefaultDb);
            }
        }

        /// <summary>
        /// 使用Redis分发锁执行一些操作
        /// </summary>
        /// <param name="resource">我们锁定的东西</param>
        /// <param name="expirationTime">Redis自动过期的时间</param>
        /// <param name="action">通过锁定执行的操作</param>
        /// <returns>如果获得锁定并执行动作，则为true; 否则为false</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            //use RedLock library
            using (var redisLock = _redisLockFactory.Create(resource, expirationTime))
            {
                //确保获得锁
                if (!redisLock.IsAcquired)
                    return false;

                //执行动作
                action();
                return true;
            }
        }

        /// <summary>
        /// 释放与此对象关联的所有资源
        /// </summary>
        public void Dispose()
        {
            //释放ConnectionMultiplexer
            if (_connection != null)
                _connection.Dispose();

            //释放RedisLockFactory
            if (_redisLockFactory != null)
                _redisLockFactory.Dispose();
        }

        #endregion

        #region Utilities

        /// <summary>
        ///从配置获取连接字符串到Redis缓存
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        /// <summary>
        /// 连接到Redis服务器
        /// </summary>
        /// <returns></returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                if (_connection != null)
                {
                    //连接断开。 处理连接...
                    _connection.Dispose();
                }

                //创建Redis Connection的新实例
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// 创建RedisLockFactory的实例
        /// </summary>
        /// <returns>RedisLockFactory</returns>
        protected RedisLockFactory CreateRedisLockFactory()
        {
            //获取密码和值是否使用连接字符串中的ssl
            var password = string.Empty;
            var useSsl = false;
            foreach (var option in GetConnectionString().Split(',').Where(option => option.Contains('=')))
            {
                switch (option.Substring(0, option.IndexOf('=')).Trim().ToLowerInvariant())
                {
                    case "password":
                        password = option.Substring(option.IndexOf('=') + 1).Trim();
                        break;
                    case "ssl":
                        bool.TryParse(option.Substring(option.IndexOf('=') + 1).Trim(), out useSsl);
                        break;
                }
            }

            //创建RedisLockFactory使用Redlock分布式锁定算法
            return new RedisLockFactory(GetEndPoints().Select(endPoint => new RedisLockEndPoint
            {
                EndPoint = endPoint,
                Password = password,
                Ssl = useSsl
            }));
        }

        #endregion

        
    }
}
