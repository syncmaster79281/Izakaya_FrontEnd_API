using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithBranchController : ControllerBase
    {
        private readonly IzakayaContext db;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        public ProductWithBranchController(IzakayaContext context, IProductService service, IProductCategoryService productCategoryService)
        {
            db = context;
            _productService = service;
            _productCategoryService = productCategoryService;
        }

        // GET: api/<ProductWithBranchController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<ProductWithBranchController>/5
        [HttpGet("{branchId}")]
        public IEnumerable<ProductDto> Get(int branchId)
        {
            //取得特定分店的所有產品
            var products = _productService.GetWithBranch(branchId, false);
            return products;
        }

        // GET api/<ProductWithBranchController>/5
        [HttpGet("{branchId}/{categoryId}")]
        public IEnumerable<ProductDto> Get(int branchId, int categoryId)
        {
            //取得單一分店的特定類別的所有產品
            if (branchId <= 0) throw new ArgumentOutOfRangeException(nameof(branchId));
            if (categoryId <= 0) throw new ArgumentOutOfRangeException(nameof(categoryId));
            var category = _productCategoryService.Get(categoryId);
            if (category == null) throw new ArgumentNullException(nameof(categoryId));
            var products = _productService.Get(branchId, categoryId);
            return products;
        }
        // POST api/<ProductWithBranchController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ProductWithBranchController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ProductWithBranchController>/5
        //[HttpDelete("{id}")]
        //public List<string> Delete(int id)
        //{

        //}
    }
}
