using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace ISPAN.Izakaya.BLL_Service_
{
    public class ProductService : IProductService
    {
        private readonly IzakayaContext db;
        public ProductService(IzakayaContext context)
        {
            db = context;
        }

        public ProductDto Get(int Id)
        {
            if (Id < 0) throw new ArgumentOutOfRangeException("發生錯誤");
            var product = db.Products.Find(Id);
            if (product == null) throw new ArgumentNullException("發生錯誤");
            if (!product.IsLaunched) throw new ArgumentException("發生錯誤");
            var category = db.ProductCategories.Find(product.CategoryId);
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                CategoryName= category.Name,
                DisplayOrder = product.DisplayOrder,
                Image=product.Image,
                ImageUrl=product.ImageUrl,
                Present=product.Present,
                UnitPrice=product.UnitPrice
            };
        }

        public IEnumerable<ProductDto> Get(int branchId, int categoryId)
        {
            var products = db.ProductStocks
                .AsNoTracking()
                .Where(x => x.BranchId == branchId && x.Stock > 0)
                .Include(x => x.Product)
                .Where(x => x.Product.CategoryId == categoryId && x.Product.IsLaunched)
                .ToList();
            return products.Select(x => new ProductDto
            {
                Id = x.Product.Id,
                Name = x.Product.Name,
                CategoryId = x.Product.CategoryId,
                CategoryName = x.Product.Category.Name,
                DisplayOrder = x.Product.DisplayOrder,
                Image = x.Product.Image,
                ImageUrl=x.Product.ImageUrl,
                Present = x.Product.Present,
                UnitPrice = x.Product.UnitPrice
            });
        }

        public IEnumerable<ProductDto> GetAll()
        {
            var products = db.Products.AsNoTracking().Where(x => x.IsLaunched).Include(x => x.Category).ToList();
            return products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                DisplayOrder = x.DisplayOrder,
                Image = x.Image,
                ImageUrl=x.ImageUrl,
                Present = x.Present,
                UnitPrice = x.UnitPrice
            });
        }
        public IEnumerable<ProductDto> GetWithBranch(int branchId, bool isSpecial)
        {
            IEnumerable<ProductStock> products;
            if (isSpecial)
            {
                var productInBranchId = db.ProductStocks
                    .AsNoTracking()
                    .Include(x => x.Product)
                    .Include(x => x.Product.Category)
                    .Where(x => x.BranchId == branchId && x.Stock > 0)
                    .ToList();
                var otherProductsId = db.ProductStocks
                    .AsNoTracking()
                    .Where(x => x.BranchId != branchId && x.Stock > 0)
                    .Select(x => x.ProductId)
                    .ToList();
                var targetId = productInBranchId.Select(x => x.ProductId).ToList();

                products = targetId.Except(otherProductsId).Join(productInBranchId, t => t, p => p.ProductId, (t, p) => new ProductStock
                {
                    ProductId = p.ProductId,
                    Product = new Product
                    {
                        Id = p.ProductId,
                        Name = p.Product.Name,
                        CategoryId = p.Product.CategoryId,
                        DisplayOrder = p.Product.DisplayOrder,
                        Image = p.Product.Image,
                        ImageUrl = p.Product.ImageUrl,
                        Present = p.Product.Present,
                        UnitPrice = p.Product.UnitPrice,
                        Category = new ProductCategory
                        {
                            Id = p.Product.CategoryId,
                            Name = p.Product.Category.Name
                        }
                    }
                });
            }
            else
            {

                products = db.ProductStocks
                   .AsNoTracking()
                   .Include(x => x.Product)
                   .Where(x => x.BranchId == branchId && x.Stock > 0 && x.Product.IsLaunched)
                   .ToList();
            }
            return products.Select(x => new ProductDto
            {
                Id = x.ProductId,
                Name = x.Product.Name,
                CategoryId = x.Product.CategoryId,
                CategoryName = x.Product.Category.Name,
                DisplayOrder = x.Product.DisplayOrder,
                Image = x.Product.Image,
                ImageUrl = x.Product.ImageUrl,
                Present = x.Product.Present,
                UnitPrice = x.Product.UnitPrice
            });
        }
    }
}
