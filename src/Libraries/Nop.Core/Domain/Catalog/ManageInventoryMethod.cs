namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// ������ķ���
    /// </summary>
    public enum ManageInventoryMethod
    {
        /// <summary>
        /// �����ٲ�Ʒ�Ŀ��
        /// </summary>
        DontManageStock = 0,
        /// <summary>
        /// ���ٲ�Ʒ�Ŀ��
        /// </summary>
        ManageStock = 1,
        /// <summary>
        ///���ٲ�Ʒ����Ʒ���ԵĿ��
        /// </summary>
        ManageStockByAttributes = 2,
    }
}
