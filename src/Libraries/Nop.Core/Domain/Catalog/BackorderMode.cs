namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a backorder mode
    /// </summary>
    public enum BackorderMode
    {
        /// <summary>
        /// û�к���
        /// </summary>
        NoBackorders = 0,
        /// <summary>
        /// ������������0
        /// </summary>
        AllowQtyBelow0 = 1,
        /// <summary>
        ///������������0��֪ͨ�ͻ�
        /// </summary>
        AllowQtyBelow0AndNotifyCustomer = 2,
    }
}
