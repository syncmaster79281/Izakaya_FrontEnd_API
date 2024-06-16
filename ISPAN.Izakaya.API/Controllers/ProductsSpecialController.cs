using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.AspNetCore.Mvc;
using Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsSpecialController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IzakayaContext db;
        public ProductsSpecialController(IzakayaContext context, IProductService service)
        {
            _service = service;
            db = context;
        }

        //GET: api/<ProductsSpecialController>
        [HttpGet]
        public string Get()
        {
            //string rootPath = Directory.GetCurrentDirectory();
            //string fullPath = Path.Combine(rootPath, "image"); // 組合出完整的資料夾路徑

            //var isDownload = GoogleDriveHelper.DownloadFile("xxoqbeme.cry.jpg", fullPath);
            //if (isDownload)
            //{
            //GoogleDriveHelper.UploadFile("xxoqbeme.cry.jpg");
            //}
            //var url = GoogleDriveHelper.GetFileUrl();
            //return url;
            var password = "jght255Y4H5";
            var salt = HashHalper.GetSalt(30);
            var newPassword = HashHalper.ToSHA256(password, salt);
            return newPassword;
        }

        // GET api/<ProductsSpecialController>/5
        [HttpGet("{branchId}")]
        public IEnumerable<ProductDto> Get(int branchId)
        {
            var products = _service.GetWithBranch(branchId, true);
            return products;
        }
        // POST api/<ProductsSpecialController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ProductsSpecialController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ProductsSpecialController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
