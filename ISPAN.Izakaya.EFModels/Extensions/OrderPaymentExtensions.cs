using ISPAN.Izakaya.EFModels.Dtos;

namespace ISPAN.Izakaya.EFModels.Extensions
{
    public static class OrderPaymentExtensions
    {
        public static IEnumerable<OrderPaymentDto> OrderTimeBetween(this IEnumerable<OrderPaymentDto> orderPayments, DateTime start, DateTime end)
        {
            return orderPayments.Where(o => o.PaymentTime >= start && o.PaymentTime <= end);
        }

        public static IEnumerable<OrderPaymentDto> InMemberId(this IEnumerable<OrderPaymentDto> orderPayments, int? memberId)
        {
            return memberId.HasValue && memberId != 0
                ? orderPayments.Where(o => o.MemberId == memberId.Value)
                : orderPayments;
        }
        public static IEnumerable<OrderPaymentDto> InCombinedOrderId(this IEnumerable<OrderPaymentDto> orderPayments, int? combinedOrderId)
        {
            return combinedOrderId.HasValue && combinedOrderId != 0
                ? orderPayments.Where(o => o.CombinedOrderId == combinedOrderId.Value)
                : orderPayments;
        }
        public static IEnumerable<OrderPaymentDto> InPaymentMethodId(this IEnumerable<OrderPaymentDto> orderPayments, int? paymentMethodId)
        {
            return paymentMethodId.HasValue && paymentMethodId != 0
                ? orderPayments.Where(o => o.PaymentMethodId == paymentMethodId.Value)
                : orderPayments;
        }
        public static IEnumerable<OrderPaymentDto> InPaymentStatusId(this IEnumerable<OrderPaymentDto> orderPayments, int? paymentStatusId)
        {
            return paymentStatusId.HasValue && paymentStatusId != 0
                ? orderPayments.Where(o => o.PaymentStatusId == paymentStatusId.Value)
                : orderPayments;
        }
    }
}
