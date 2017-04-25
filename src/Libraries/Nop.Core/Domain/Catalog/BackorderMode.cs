namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a backorder mode
    /// </summary>
    public enum BackorderMode
    {
        /// <summary>
        /// 没有后退
        /// </summary>
        NoBackorders = 0,
        /// <summary>
        /// 允许数量低于0
        /// </summary>
        AllowQtyBelow0 = 1,
        /// <summary>
        ///允许数量低于0并通知客户
        /// </summary>
        AllowQtyBelow0AndNotifyCustomer = 2,
    }
}
