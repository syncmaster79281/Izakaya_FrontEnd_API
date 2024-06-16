using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.EntityFrameworkCore;

namespace ISPAN.Izakaya.BLL_Service_
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IzakayaContext db;
        public ProductCategoryService(IzakayaContext izakayaContext)
        {
            db = izakayaContext;
        }

        public IEnumerable<ProductCategoryDto> Get()
        {
            var categories = db.ProductCategories.Select(x => new ProductCategoryDto { Id = x.Id, Name = x.Name, Url = x.Url }).ToList();
            return categories;
        }

        public ProductCategoryDto Get(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            var category = db.ProductCategories.Find(id);
            if (category == null) throw new ArgumentNullException(nameof(id));
            return new ProductCategoryDto { Name = category.Name, Id = id, Url = category.Url };
        }

        public IEnumerable<ProductDto> GetProduct(int id)
        {
            var products = db.Products
                .AsNoTracking()
                .Where(x => x.CategoryId == id && x.IsLaunched)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    DisplayOrder = x.DisplayOrder,
                    Image = x.Image,
                    Present = x.Present,
                    UnitPrice = x.UnitPrice
                }).ToList();
            return products;
        }
    }
}
