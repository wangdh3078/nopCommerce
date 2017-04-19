using System;

namespace Nop.Core.Caching
{
    /// <summary>
    /// �������ӿ�
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// ��ȡ��������ָ�����������ֵ��
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="key">��</param>
        /// <returns>��ָ�����������ֵ��</returns>
        T Get<T>(string key);

        /// <summary>
        /// ��ָ���ļ��Ͷ�����ӵ����档
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="data">����</param>
        /// <param name="cacheTime">����ʱ��</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// ��ȡһ��ֵ��ָʾ��ָ�����������ֵ�Ƿ񱻻���
        /// </summary>
        /// <param name="key">��</param>
        /// <returns>true-�ѻ��棬false-δ����</returns>
        bool IsSet(string key);

        /// <summary>
        /// �ӻ�����ɾ��ָ������ֵ
        /// </summary>
        /// <param name="key">��</param>
        void Remove(string key);

        /// <summary>
        /// ͨ��ָ��ģʽɾ������
        /// </summary>
        /// <param name="pattern">ģʽ</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// ������л�������
        /// </summary>
        void Clear();
    }
}
