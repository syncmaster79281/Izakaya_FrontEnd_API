namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required string Name { get; set; }
        public int UnitPrice { get; set; }
        public string? Image { get; set; }
        public string ImageUrl { get; set; }
        public string? Present { get; set; }
        public int DisplayOrder { get; set; }
    }
}
