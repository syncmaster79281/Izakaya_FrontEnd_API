
using ISPAN.Izakaya.EFModels.Dtos;

namespace ISPAN.Izakaya.IBLL_IService_
{
    public interface IProductCategoryService
    {
        IEnumerable<ProductCategoryDto> Get();
        IEnumerable<ProductDto> GetProduct(int id);
        ProductCategoryDto Get(int id);

    }
}
