using ISPAN.Izakaya.EFModels.Dtos;

namespace ISPAN.Izakaya.EFModels.Extensions
{
    public static class SeatCartExtensions
    {
        public static IEnumerable<SeatCartDto> OrderTimeBetween(this IEnumerable<SeatCartDto> seatCarts, DateTime start, DateTime end)
        {
            return seatCarts.Where(s => s.OrderTime >= start && s.OrderTime <= end);
        }

        public static IEnumerable<SeatCartDto> InSeatId(this IEnumerable<SeatCartDto> seatCarts, int? seatId)
        {
            return seatId.HasValue && seatId != 0
                ? seatCarts.Where(s => s.SeatId == seatId.Value)
                : seatCarts;
        }
        public static IEnumerable<SeatCartDto> InCartStatusId(this IEnumerable<SeatCartDto> seatCarts, int? cartStatusId)
        {
            return cartStatusId.HasValue && cartStatusId != 0
                ? seatCarts.Where(s => s.CartStatusId.Equals(cartStatusId))
                : seatCarts;
        }
        public static IEnumerable<SeatCartDto> SortOrderTime(this IEnumerable<SeatCartDto> seatCarts, bool mode = true)
        {
            return mode
                ? seatCarts.OrderBy(s => s.OrderTime)
                : seatCarts.OrderByDescending(s => s.OrderTime);
        }
        public static IEnumerable<SeatCartDto> InCartStatus(this IEnumerable<SeatCartDto> seatCarts, string cartStatus)
        {
            return string.IsNullOrEmpty(cartStatus)
                ? seatCarts
                : seatCarts.Where(s => s.CartStatus == cartStatus.Trim());
        }
    }
}
