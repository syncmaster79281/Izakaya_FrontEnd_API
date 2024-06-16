using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public OrdersController(IzakayaContext context)
        {
            _context = context;
        }

        //取得熱門商品
        // GET: api/<OrdersController>
        [HttpGet("GetHotProduct")]
        public IEnumerable<HotProductDto> GetHotProduct([FromQuery] SearchCondition dto)
        {
            var query = GetHotItem(dto);
            return query;
        }

        private IEnumerable<HotProductDto> GetHotItem(SearchCondition condition)
        {
            var searchCount = condition.Count;
            var start = condition.StartTime.AddDays(0 - Math.Abs(condition.Days));
            var end = new DateTime(condition.StartTime.Year, condition.StartTime.Month, condition.StartTime.Day, 23, 59, 59);

            var orderList = _context.OrderDetails
                .AsNoTracking()
                .Include(x => x.Order)
                .Where(x => x.Order.CreateTime >= start && x.Order.CreateTime <= end)
                .GroupBy(x => x.ProductId)
                .Select(x => new Sales
                {
                    ProductId = x.Key,
                    Count = x.Sum(y => y.Qty)
                })
                .OrderByDescending(x => x.Count)
                .Take(searchCount)
                .ToList();

            var result = new List<HotProductDto>();

            foreach (var item in orderList)
            {
                var data = _context.Products
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Where(p => p.Id == item.ProductId)
                    .Select(p => new HotProductDto
                    {
                        ProductId = item.ProductId,
                        ProductName = p.Name,
                        TotalCount = item.Count,
                        Image = p.Image,
                        ImageUrl = p.ImageUrl,
                        CategoryName = p.Category.Name,
                        CategoryId = p.CategoryId,
                        Present = p.Present,
                        UnitPrice = p.UnitPrice,
                    }).Single();

                result.Add(data);
            }
            return result;
        }
        [HttpGet("GetHotProductTest")]
        public IEnumerable<HotProductDto> GetHotProductTest([FromQuery] SearchCondition dto)
        {
            var searchCount = dto.Count;
            var start = dto.StartTime.AddDays(0 - Math.Abs(dto.Days));
            var end = new DateTime(dto.StartTime.Year, dto.StartTime.Month, dto.StartTime.Day, 23, 59, 59);

            var orderList = _context.OrderDetails
                .AsNoTracking()
                .Include(x => x.Order)
                .Where(x => x.Order.CreateTime >= start && x.Order.CreateTime <= end)
                .GroupBy(x => x.ProductId)
                .Select(x => new Sales
                {
                    ProductId = x.Key,
                    Count = x.Sum(y => y.Qty)
                })
                .OrderByDescending(x => x.Count)
                .Take(searchCount)
                .ToList();

            var result = new List<HotProductDto>();

            foreach (var item in orderList)
            {

                var data = _context.Products.AsNoTracking()
                    .Include(p => p.Category)
                    .Where(p => p.Id == item.ProductId)
                    .Select(p => new HotProductDto
                    {
                        ProductId = item.ProductId,
                        ProductName = p.Name,
                        TotalCount = item.Count,
                        Image = p.Image,
                        ImageUrl = p.ImageUrl,
                        CategoryName = p.Category.Name,
                        CategoryId = p.CategoryId,
                        Present = p.Present
                    }).Single();

                result.Add(data);
            }

            return result;
        }

        //// GET api/<OrdersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<OrdersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<OrdersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<OrdersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
