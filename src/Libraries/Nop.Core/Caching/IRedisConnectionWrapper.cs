using System;
using System.Net;
using StackExchange.Redis;

namespace Nop.Core.Caching
{
    /// <summary>
    /// Redis连接包装接口
    /// </summary>
    public interface IRedisConnectionWrapper : IDisposable
    {
        /// <summary>
        /// 获取与redis中的数据库的连接
        /// </summary>
        /// <param name="db">数据库号 传递null使用默认值</param>
        /// <returns>Redis缓存数据库</returns>
        IDatabase GetDatabase(int? db = null);

        /// <summary>
        /// 获取单个服务器的配置API
        /// </summary>
        /// <param name="endPoint">网络端点</param>
        /// <returns>Redis服务</returns>
        IServer GetServer(EndPoint endPoint);

        /// <summary>
        ///获取服务器上定义的所有端点
        /// </summary>
        /// <returns>端点阵列</returns>
        EndPoint[] GetEndPoints();

        /// <summary>
        /// 删除数据库的所有键
        /// </summary>
        /// <param name="db">数据库号 传递null使用默认值<</param>
        void FlushDatabase(int? db = null);

        /// <summary>
        /// 使用Redis分发锁执行一些操作
        /// </summary>
        /// <param name="resource">我们锁定的东西</param>
        /// <param name="expirationTime">Redis自动过期的时间</param>
        /// <param name="action">通过锁定执行的操作</param>
        /// <returns>如果获得锁定并执行动作，则为true; 否则为false</returns>
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
