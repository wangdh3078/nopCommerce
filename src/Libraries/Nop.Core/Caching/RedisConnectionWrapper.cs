using System;
using System.Linq;
using System.Net;
using Nop.Core.Configuration;
using RedLock;
using StackExchange.Redis;

namespace Nop.Core.Caching
{
    /// <summary>
    /// Redis���Ӱ�װ��ʵ��
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        #region �ֶ�

        private readonly NopConfig _config;
        private readonly Lazy<string> _connectionString;

        private volatile ConnectionMultiplexer _connection;
        private volatile RedisLockFactory _redisLockFactory;
        private readonly object _lock = new object();

        #endregion

        #region ���캯��

        public RedisConnectionWrapper(NopConfig config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(GetConnectionString);
            this._redisLockFactory = CreateRedisLockFactory();
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ȡ��redis�е����ݿ�Ľ���ʽ����
        /// </summary>
        /// <param name="db">���ݿ�� ����nullʹ��Ĭ��ֵ</param>
        /// <returns>Redis�������ݿ�</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1); //_settings.DefaultDb);
        }

        /// <summary>
        ///��ȡ����������������API
        /// </summary>
        /// <param name="endPoint">����˵�</param>
        /// <returns>Redis����</returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// ��ȡ�������϶�������ж˵�
        /// </summary>
        /// <returns>�˵�����</returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// ɾ�����ݿ�����м�
        /// </summary>
        /// <param name="db">���ݿ�� ����nullʹ��Ĭ��ֵ</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1); //_settings.DefaultDb);
            }
        }

        /// <summary>
        /// ʹ��Redis�ַ���ִ��һЩ����
        /// </summary>
        /// <param name="resource">���������Ķ���</param>
        /// <param name="expirationTime">Redis�Զ����ڵ�ʱ��</param>
        /// <param name="action">ͨ������ִ�еĲ���</param>
        /// <returns>������������ִ�ж�������Ϊtrue; ����Ϊfalse</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            //use RedLock library
            using (var redisLock = _redisLockFactory.Create(resource, expirationTime))
            {
                //ȷ�������
                if (!redisLock.IsAcquired)
                    return false;

                //ִ�ж���
                action();
                return true;
            }
        }

        /// <summary>
        /// �ͷ���˶��������������Դ
        /// </summary>
        public void Dispose()
        {
            //�ͷ�ConnectionMultiplexer
            if (_connection != null)
                _connection.Dispose();

            //�ͷ�RedisLockFactory
            if (_redisLockFactory != null)
                _redisLockFactory.Dispose();
        }

        #endregion

        #region Utilities

        /// <summary>
        ///�����û�ȡ�����ַ�����Redis����
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        /// <summary>
        /// ���ӵ�Redis������
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
                    //���ӶϿ��� ��������...
                    _connection.Dispose();
                }

                //����Redis Connection����ʵ��
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// ����RedisLockFactory��ʵ��
        /// </summary>
        /// <returns>RedisLockFactory</returns>
        protected RedisLockFactory CreateRedisLockFactory()
        {
            //��ȡ�����ֵ�Ƿ�ʹ�������ַ����е�ssl
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

            //����RedisLockFactoryʹ��Redlock�ֲ�ʽ�����㷨
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
