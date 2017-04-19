namespace Nop.Core.Caching
{
    /// <summary>
    /// ����һ��NopNullCache��û�л��棩
    /// </summary>
    public partial class NopNullCache : ICacheManager
    {
        /// <summary>
        /// ��ȡ��������ָ�����������ֵ��
        /// </summary>
        /// <typeparam name="T">��</typeparam>
        /// <param name="key">��</param>
        /// <returns>��ָ�����������ֵ��</returns>
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        ///��ָ���ļ��Ͷ�����ӵ����档
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="data">����</param>
        /// <param name="cacheTime">����ʱ��</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        /// <summary>
        /// ��ȡһ��ֵ��ָʾ��ָ�����������ֵ�Ƿ񱻻���
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <summary>
        /// �ӻ�����ɾ��ָ������ֵ
        /// </summary>
        /// <param name="key">��</param>
        public virtual void Remove(string key)
        {
        }

        /// <summary>
        /// ͨ��ģʽɾ������
        /// </summary>
        /// <param name="pattern">ģʽ</param>
        public virtual void RemoveByPattern(string pattern)
        {
        }

        /// <summary>
        /// ������л���
        /// </summary>
        public virtual void Clear()
        {
        }

        /// <summary>
        /// ����
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}