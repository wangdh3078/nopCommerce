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
        /// ��ȡ�������̵��ʶ��
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// ��ȡ�����ò�Ʒ��ʶ��
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ��ȡ�����ÿͻ���ʶ��
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// ��ȡ������ʵ�����������ں�ʱ��
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// ��ȡ�����ò�Ʒ
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// ��ȡ�����ÿͻ�
        /// </summary>
        public virtual Customer Customer { get; set; }

    }

}
