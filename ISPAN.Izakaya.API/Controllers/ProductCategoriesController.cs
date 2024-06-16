using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IzakayaContext _Izakaya;
        private readonly IProductCategoryService _service;

        public ProductCategoriesController(IzakayaContext Izakaya, IProductCategoryService server)
        {
            _Izakaya = Izakaya;
            _service = server;
        }

        // GET: api/<ProductCategoriesController>
        [HttpGet]
        public IEnumerable<ProductCategoryDto> Get()
        {
            //取得所有產品分類
            var categories = _service.Get();
            return categories;
        }

        // GET api/<ProductCategoriesController>/5
        [HttpGet("{id}")]
        public IEnumerable<ProductDto> Get(int id)
        {
            //取得單一分類的所有產品
            var category = _service.Get(id);
            if (category == null) throw new ArgumentNullException(nameof(id));
            var products = _service.GetProduct(id);
            return products;
        }

        // POST api/<ProductCategoriesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ProductCategoriesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ProductCategoriesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{

        //}
    }
}
