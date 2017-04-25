namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 库存管理的方法
    /// </summary>
    public enum ManageInventoryMethod
    {
        /// <summary>
        /// 不跟踪产品的库存
        /// </summary>
        DontManageStock = 0,
        /// <summary>
        /// 跟踪产品的库存
        /// </summary>
        ManageStock = 1,
        /// <summary>
        ///跟踪产品按产品属性的库存
        /// </summary>
        ManageStockByAttributes = 2,
    }
}
