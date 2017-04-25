namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// 价格范围
    /// </summary>
    public partial class PriceRange
    {
        /// <summary>
        /// From
        /// </summary>
        public decimal? From { get; set; }
        /// <summary>
        /// To
        /// </summary>
        public decimal? To { get; set; }
    }
}
