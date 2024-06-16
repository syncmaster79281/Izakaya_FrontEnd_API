namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class HotProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int TotalCount { get; set; } = 0;
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string Present { get; set; }
    }
    public class Sales
    {
        public int ProductId { get; set; }
        public int Count { get; set; } = 0;
    }
    public class SearchCondition
    {
        public int Count { get; set; } = 5;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public int Days { get; set; } = 7;
    }
}
