using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nop.Core.Caching
{
    /// <summary>
    /// ������չ
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        ///��ȡ������Ŀ�� �����δ���棬����ز�������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="cacheManager">�������</param>
        /// <param name="key">��</param>
        /// <param name="acquire">������ڻ����У��������Ŀ�Ĺ���</param>
        /// <returns>��������</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// ��ȡ������Ŀ�� �����δ���棬����ز�������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="cacheManager">�������</param>
        /// <param name="key">��</param>
        /// <param name="cacheTime">����ʱ�䣨���ӣ���0 - �����棩</param>
        /// <param name="acquire">������ڻ����У��������Ŀ�Ĺ���</param>
        /// <returns>��������</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        /// <summary>
        /// ͨ��ģʽ�Ƴ�����
        /// </summary>
        /// <param name="cacheManager">�������</param>
        /// <param name="pattern">ģʽ</param>
        /// <param name="keys">�����е����м�</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }
    }
}
