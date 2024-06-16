using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IzakayaContext db;
        private readonly IProductService _service;
        public ProductsController(IzakayaContext context, IProductService service)
        {
            db = context;
            //_service = new ProductService(context);
            _service = service;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<ProductDto> Get()
        {
            //取得所有產品
            var products = _service.GetAll();
            return products;

        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ProductDto Get(int id)
        {
            //取得單一產品
            var product = _service.Get(id);
            return product;
        }
        // POST api/<ProductsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ProductsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ProductsController>/5
        //[HttpDelete("{fileName}")]
        //public void Delete(string fileName)
        //{
        //}
    }
}
