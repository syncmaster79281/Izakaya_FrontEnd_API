using ISPAN.Izakaya.EFModels.Dtos;

namespace ISPAN.Izakaya.EFModels.Extensions
{
    public static class SeatExtensions
    {
        public static IEnumerable<SeatDto> InBranchId(this IEnumerable<SeatDto> seats, int? branchId)
        {
            return branchId.HasValue && branchId != 0
                ? seats.Where(s => s.BranchId == branchId.Value)
                : seats;
        }
        public static IEnumerable<SeatDto> InSeatId(this IEnumerable<SeatDto> seats, int? seatId)
        {
            return seatId.HasValue && seatId != 0
                ? seats.Where(s => s.Id == seatId.Value)
                : seats;
        }
        public static IEnumerable<SeatDto> ContainsName(this IEnumerable<SeatDto> seats, string name)
        {
            return string.IsNullOrEmpty(name)
            ? seats
            : seats.Where(s => s.Name.Contains(name.Trim()));
        }
    }
}
