namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 产品评审批准活动
    /// </summary>
    public class ProductReviewApprovedEvent
    {
        public ProductReviewApprovedEvent(ProductReview productReview)
        {
            this.ProductReview = productReview;
        }

        /// <summary>
        /// 产品评论
        /// </summary>
        public ProductReview ProductReview { get; private set; }
    }
}