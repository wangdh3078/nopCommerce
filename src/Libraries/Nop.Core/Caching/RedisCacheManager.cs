using System;
using System.Text;
using Newtonsoft.Json;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using StackExchange.Redis;

namespace Nop.Core.Caching
{
    /// <summary>
    /// ��ʾһ�ֶ�redis�洢���������(http://redis.io/).
    /// ���������£�������Web��Azure������ʱʹ��
    /// ��ȻҲ�������κη������򻷾���ʹ��
    /// </summary>
    public partial class RedisCacheManager : ICacheManager
    {
        #region �ֶ�
        /// <summary>
        /// Redis���Ӱ�װ
        /// </summary>
        private readonly IRedisConnectionWrapper _connectionWrapper;
        /// <summary>
        /// Redis���ݿ�
        /// </summary>
        private readonly IDatabase _db;
        /// <summary>
        /// ����������
        /// </summary>
        private readonly ICacheManager _perRequestCacheManager;

        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="config">����</param>
        /// <param name="connectionWrapper">Redis���Ӱ�װ</param>

        public RedisCacheManager(NopConfig config, IRedisConnectionWrapper connectionWrapper)
        {
            if (String.IsNullOrEmpty(config.RedisCachingConnectionString))
                throw new Exception("Redis connection string is empty");

            // ConnectionMultiplexer.ConnectֻӦ�ñ�����һ�Σ����ұ�����
            this._connectionWrapper = connectionWrapper;

            this._db = _connectionWrapper.GetDatabase();
            this._perRequestCacheManager = EngineContext.Current.Resolve<ICacheManager>();
        }

        #endregion

        #region ����

        /// <summary>
        ///��ȡ��������ָ�����������ֵ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <returns>��ָ�����������ֵ��</returns>
        public virtual T Get<T>(string key)
        {
            //��������ܱ仯����
            //����ʹ�á�PerRequestCacheManager������ǰHTTP����ļ��ض��󻺴����ڴ��С�
            //�������ǽ�����ÿ��HTTP�������ӵ�Redis������500�Σ�����ÿ�μ������Ի��������ã�
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
        /// ��ָ���ļ��Ͷ�����ӵ����档
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="data">����</param>
        /// <param name="cacheTime">����ʱ��</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }

        /// <summary>
        ///��ȡһ��ֵ��ָʾ��ָ�����������ֵ�Ƿ񱻻���
        /// </summary>
        /// <param name="key">��</param>
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
        ///�ӻ�����ɾ��ָ������ֵ
        /// </summary>
        /// <param name="key">��</param>
        public virtual void Remove(string key)
        {
            _db.KeyDelete(key);
            _perRequestCacheManager.Remove(key);
        }

        /// <summary>
        /// ͨ��ģʽɾ���������
        /// </summary>
        /// <param name="pattern">ģʽ</param>
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
        /// ������л������
        /// </summary>
        public virtual void Clear()
        {
            foreach (var ep in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(ep);
                //���ǿ���ʹ������Ĵ��루ע�ͣ�
                //������Ҫ����Ȩ�� - ����allowAdmin = true��
                //server.FlushDatabase();

                //�����Ϊʲô�������ڼ򵥵ر�������Ԫ��
                var keys = server.Keys(database: _db.Database);
                foreach (var key in keys)
                    Remove(key);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public virtual void Dispose()
        {
            //if (_connectionWrapper != null)
            //    _connectionWrapper.Dispose();
        }

        #endregion

        #region Utilities
        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="item">���л�����</param>
        /// <returns></returns>
        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return Encoding.UTF8.GetBytes(jsonString);
        }
        /// <summary>
        /// �����л�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="serializedObject">�����л�����</param>
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
