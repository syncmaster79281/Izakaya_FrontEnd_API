namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class OrderListDto
    {
        public OrderListDto()
        {
            Packages = new List<PackageDto>();
        }

        public int Amount { get; set; }
        public string Currency { get; set; } = "TWD";
        public string OrderId { get; set; }

        public List<PackageDto> Packages { get; set; }

    }
    public class PackageDto
    {
        public PackageDto()
        {
            Products = new List<ProductItem>();
        }
        public string Id { get; set; }
        public int Amount => Products.Sum(x => x.Price * x.Quantity);
        public string Name { get; set; }

        public List<ProductItem> Products { get; set; }
    }
    public class ProductItem
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
