//Contributor : MVCContrib

namespace Nop.Web.Framework.UI.Paging
{
    /// <summary>
    /// �Ѿ���ֳ�ҳ��Ķ���ļ��ϡ�
    /// </summary>
    public interface IPageableModel
    {
        /// <summary>
        /// ��ǰҳ����������0��ʼ��
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// ��ǰҳ�루��1��ʼ��
        /// </summary>
        int PageNumber { get; }
        /// <summary>
        /// ÿҳ�е���Ŀ����
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// ������
        /// </summary>
        int TotalItems { get; }
        /// <summary>
        /// ��ҳ����
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// ҳ���е�һ����Ŀ��������
        /// </summary>
        int FirstItem { get; }
        /// <summary>
        /// ҳ�����һ����Ŀ��������
        /// </summary>
        int LastItem { get; }
        /// <summary>
        /// ��ǰҳ��֮ǰ�Ƿ���ҳ�档
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// �Ƿ��е�ǰҳ��֮���ҳ�档
        /// </summary>
        bool HasNextPage { get; }
	}


    /// <summary>
    /// ��ʡ <see cref="IPageableModel"/>
    /// </summary>
    /// <typeparam name="T">���ڷ�ҳ�Ķ�������</typeparam>
    public interface IPagination<T> : IPageableModel
	{

	}
}