using ISPAN.Izakaya.EFModels.Dtos;

namespace ISPAN.Izakaya.IBLL_IService_
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAll();
        ProductDto Get(int Id);
        IEnumerable<ProductDto> GetWithBranch(int branchId, bool isSpecial);
        IEnumerable<ProductDto> Get(int branchId, int categoryId);
    }
}
