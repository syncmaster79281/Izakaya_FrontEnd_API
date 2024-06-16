using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Extensions
{
    public static class ReservationExtensions
    {
        public static IEnumerable<Reservation> TimeBetween(this IEnumerable<Reservation> reservations, DateTime? reservationTime)
        {
            return reservationTime.HasValue
                ? reservations.Where(s => s.ReservationTime == reservationTime.Value)
                : reservations;
        }

        public static IEnumerable<Reservation> IsBranchId(this IEnumerable<Reservation> reservations, int? branceId)
        {
            return branceId.HasValue && branceId > 0
                ? reservations.Where(s => s.BranchId == branceId.Value)
                : reservations;
        }
        public static IEnumerable<Reservation> IsMemberId(this IEnumerable<Reservation> reservations, int? memberId)
        {
            return memberId.HasValue && memberId > 0
                ? reservations.Where(s => s.MemberId == memberId.Value)
                : reservations;
        }
        public static IEnumerable<Reservation> InStatus(this IEnumerable<Reservation> reservations, string status)
        {
            return string.IsNullOrEmpty(status)
                ? reservations
                : reservations.Where(s => s.Status == status.Trim());
        }
    }
}
