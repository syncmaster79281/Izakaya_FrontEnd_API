using ISPAN.Izakaya.EFModels.Dtos;

namespace ISPAN.Izakaya.EFModels.Extensions
{
    public static class SeatProductExtensions
    {
        public static IEnumerable<SeatProductDto> InCategoryId(this IEnumerable<SeatProductDto> products, int? categoryId)
        {
            return categoryId.HasValue && categoryId != 0
                ? products.Where(s => s.CategoryId == categoryId.Value)
                : products;
        }
        public static IEnumerable<SeatProductDto> HaveStock(this IEnumerable<SeatProductDto> products, int? stock)
        {
            return stock.HasValue
                ? products.Where(s => s.Stock >= stock.Value)
                : products;
        }
        public static IEnumerable<SeatProductDto> InBranchId(this IEnumerable<SeatProductDto> products, int? branchId)
        {
            return branchId.HasValue && branchId != 0
                ? products.Where(s => s.BranchId == branchId.Value)
                : products;
        }
        public static IEnumerable<SeatProductDto> IsLaunched(this IEnumerable<SeatProductDto> products, bool IsLaunched)
        {
            return products.Where(s => s.IsLaunched == IsLaunched);
        }
    }
}
