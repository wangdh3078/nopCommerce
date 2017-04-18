using System.Web;

namespace Nop.Core
{
    /// <summary>
    /// Web������
    /// </summary>
    public partial interface IWebHelper
    {
        /// <summary>
        /// ��ȡURL����
        /// </summary>
        /// <returns>URL����</returns>
        string GetUrlReferrer();

        /// <summary>
        /// ��ȡ������IP��ַ
        /// </summary>
        /// <returns>IP��ַ</returns>
        string GetCurrentIpAddress();

        /// <summary>
        ///��ȡҳ������
        /// </summary>
        /// <param name="includeQueryString">ָʾ�Ƿ������ѯ�ַ�����ֵ</param>
        /// <returns>ҳ������</returns>
        string GetThisPageUrl(bool includeQueryString);

        /// <summary>
        /// ��ȡҳ������
        /// </summary>
        /// <param name="includeQueryString">ָʾ�Ƿ������ѯ�ַ�����ֵ</param>
        /// <param name="useSsl">ָʾ�Ƿ���SSL����ҳ��ֵ</param>
        /// <returns>ҳ������</returns>
        string GetThisPageUrl(bool includeQueryString, bool useSsl);

        /// <summary>
        /// ��ȡһ��ֵ��ָʾ��ǰ�����Ƿ񱻱���
        /// </summary>
        /// <returns>True - ��ȫ��false - ����ȫ</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// ͨ�����ƻ�ȡ����������
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>����������</returns>
        string ServerVariables(string name);

        /// <summary>
        ///��ȡ����λ��
        /// </summary>
        /// <param name="useSsl">ʹ��SSL</param>
        /// <returns>�洢����λ��</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        string GetStoreLocation();

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store location</returns>
        string GetStoreLocation(bool useSsl);

        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file.</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        bool IsStaticResource(HttpRequest request);

        /// <summary>
        /// �޸Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>New url</returns>
        string ModifyQueryString(string url, string queryStringModification, string anchor);

        /// <summary>
        /// Remove query string from url
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryString">Query string to remove</param>
        /// <returns>New url</returns>
        string RemoveQueryString(string url, string queryString);

        /// <summary>
        /// ͨ�����ƻ�ȡ��ѯ�ַ���ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">��������</param>
        /// <returns>��ѯ�ַ���ֵ</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// ��������Ӧ�ó�����
        /// </summary>
        /// <param name="makeRedirect">һ��ֵ����ʾ�������Ƿ�Ӧ���ض���</param>
        /// <param name="redirectUrl">�ض�����ַ; ���Ҫ�ض��򵽵�ǰҳ��URL����Ϊ���ַ���</param>
        void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "");

        /// <summary>
        /// ��ȡһ��ֵ��ָʾ�ͻ����Ƿ��ض�����λ��
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�ͻ����Ƿ�ʹ��POST�ض�����λ��
        /// </summary>
        bool IsPostBeingDone { get; set; }
    }
}
