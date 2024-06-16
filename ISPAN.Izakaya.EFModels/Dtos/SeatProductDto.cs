namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class SeatProductDto
    {
        public int? Id { get; set; }

        public int? BranchId { get; set; }
        public int? CategoryId { get; set; }

        public string? Name { get; set; }

        public int? Stock { get; set; }

        public string? Image { get; set; }
        public string? ImageUrl { get; set; }

        public int? UnitPrice { get; set; }

        public string? Present { get; set; }

        public bool IsLaunched { get; set; }
    }
}
