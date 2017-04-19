using System;
using System.Net;
using StackExchange.Redis;

namespace Nop.Core.Caching
{
    /// <summary>
    /// Redis���Ӱ�װ�ӿ�
    /// </summary>
    public interface IRedisConnectionWrapper : IDisposable
    {
        /// <summary>
        /// ��ȡ��redis�е����ݿ������
        /// </summary>
        /// <param name="db">���ݿ�� ����nullʹ��Ĭ��ֵ</param>
        /// <returns>Redis�������ݿ�</returns>
        IDatabase GetDatabase(int? db = null);

        /// <summary>
        /// ��ȡ����������������API
        /// </summary>
        /// <param name="endPoint">����˵�</param>
        /// <returns>Redis����</returns>
        IServer GetServer(EndPoint endPoint);

        /// <summary>
        ///��ȡ�������϶�������ж˵�
        /// </summary>
        /// <returns>�˵�����</returns>
        EndPoint[] GetEndPoints();

        /// <summary>
        /// ɾ�����ݿ�����м�
        /// </summary>
        /// <param name="db">���ݿ�� ����nullʹ��Ĭ��ֵ<</param>
        void FlushDatabase(int? db = null);

        /// <summary>
        /// ʹ��Redis�ַ���ִ��һЩ����
        /// </summary>
        /// <param name="resource">���������Ķ���</param>
        /// <param name="expirationTime">Redis�Զ����ڵ�ʱ��</param>
        /// <param name="action">ͨ������ִ�еĲ���</param>
        /// <returns>������������ִ�ж�������Ϊtrue; ����Ϊfalse</returns>
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
