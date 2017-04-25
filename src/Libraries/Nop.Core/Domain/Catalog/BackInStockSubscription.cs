using System;
using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a back in stock subscription
    /// </summary>
    public partial class BackInStockSubscription : BaseEntity
    {
        /// <summary>
        /// 获取或设置商店标识符
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 获取或设置产品标识符
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 获取或设置客户标识符
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 获取或设置实例创建的日期和时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 获取或设置产品
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// 获取或设置客户
        /// </summary>
        public virtual Customer Customer { get; set; }

    }

}
