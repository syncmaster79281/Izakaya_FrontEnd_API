namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public int SeatId { get; set; }
        public string? SeatName { get; set; }

        public int? MemberId { get; set; }

        public DateTime ReservationTime { get; set; }

        public string Tel { get; set; }

        public string Status { get; set; }

        public int Qty { get; set; }

        public string Name { get; set; }

        public string FillUp { get; set; }

        public string Email { get; set; }

        public int Adult { get; set; }

        public int Child { get; set; }

        public string Message { get; set; }

        public string Random { get; set; }
    }
    public class SearchReservationDto
    {
        public DateTime? ReservationTime { get; set; }
        public int? BranchId { get; set; }
        public int? MemberId { get; set; }
        public string? Status { get; set; }

    }

    public class ReservationPutDto
    {
        public int Id { get; set; }

        public string Random { get; set; }

        public string Message { get; set; }

        public int Adult { get; set; }

        public int Child { get; set; }
    }
}
