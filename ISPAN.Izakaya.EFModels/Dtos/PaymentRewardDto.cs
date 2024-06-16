namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class PaymentRewardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProductId { get; set; }
        public string Condition { get; set; }
        public decimal? DiscountMethod { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsUsed { get; set; }
        public string Description { get; set; }
        public int? CouponId { get; set; }
        public int? MemberId { get; set; }
        public int? Qty { get; set; }

    }
}
