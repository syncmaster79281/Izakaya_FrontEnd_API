using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Extensions;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatCartsController : ControllerBase
    {
        private readonly IzakayaContext _context;
        public SeatCartsController(IzakayaContext context)
        {
            _context = context;
        }
        // GET: api/<SeatCartsController> 查詢
        [HttpGet]
        public IEnumerable<SeatCartDto> Get([FromQuery] SeatCartDto dto)
        {

            var query = _context.SeatCarts.AsNoTracking()
                .Select(s => new SeatCartDto
                {
                    Id = s.Id,
                    SeatId = s.SeatId,
                    ProductId = s.ProductId,
                    ProductName = s.Product.Name,
                    CartStatusId = s.CartStatusId,
                    UnitPrice = s.UnitPrice,
                    Qty = s.Qty,
                    Notes = s.Notes,
                    OrderTime = s.OrderTime,
                    CartStatus = s.CartStatus.Status,
                })
                .InSeatId(dto.SeatId)
                .InCartStatus(dto.CartStatus)
                .OrderTimeBetween(dto.OrderTime.Value.AddHours(-10), dto.OrderTime.Value.AddMinutes(10))
                .ToList();

            return query;
        }

        // GET api/<SeatCartsController>/5 查詢
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var seatCart = _context.SeatCarts.Find(id);
                if (seatCart == null)
                {
                    return NotFound("查無資料");
                }

                var seatCartDto = seatCart.ToDto();
                return Ok(seatCartDto);

            }
            catch (Exception e)
            {
                return StatusCode(500, "查無資料");
            }
        }


        // GET api/<SeatCartsController>/5 查詢  (參數小寫開頭) 根據類別查詢產品
        [HttpGet("Getpruduct")]
        public IEnumerable<SeatProductDto> GetProduct([FromQuery] SeatProductDto dto)
        {
            var query = _context.ProductStocks
                .AsNoTracking()
                .Include(p => p.Product)
                .Select(p => new SeatProductDto
                {
                    Id = p.ProductId,
                    BranchId = p.BranchId,
                    CategoryId = p.Product.CategoryId,
                    Name = p.Product.Name,
                    Stock = p.Stock,
                    Image = p.Product.Image,
                    ImageUrl = p.Product.ImageUrl,
                    UnitPrice = p.Product.UnitPrice,
                    Present = p.Product.Present,
                    IsLaunched = p.Product.IsLaunched,

                })
                .InCategoryId(dto.CategoryId)
                .InBranchId(dto.BranchId)
                .IsLaunched(dto.IsLaunched)
                .HaveStock(dto.Stock);
            return query;
        }


        // GET api/<SeatCartsController>/5 查詢  (參數小寫開頭) 根據類別查詢產品
        [HttpGet("GetProductCategory")]
        public IEnumerable<SeatProductCategoryDto> GetProductCategory()
        {
            var query = _context.ProductCategories.Select(p => p.ToDto());
            return query;
        }


        // POST api/<SeatCartsController>  新增
        [HttpPost]
        public string Post([FromBody] IEnumerable<SeatCartDto> dto)
        {
            try
            {
                var seatCarts = dto.Select(s => s.ToEntity()).ToList();
                seatCarts.ForEach(s => _context.SeatCarts.Add(s));
                _context.SaveChanges();
                return $"點餐成功";
            }
            catch (ArgumentException e)
            {
                return $"點餐失敗,{e.Message}";
            }
        }

        // PUT api/<SeatCartsController>/5     更新
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] SeatCartDto dto)
        {
            //網址列id要和物件內一樣
            if (id != dto.Id)
            {
                return "修改點餐資訊失敗";
            }

            SeatCart entity = _context.SeatCarts.Find(dto.Id);

            if (entity != null)
            {
                try
                {
                    var data = dto.ToEntity();

                    entity.SeatId = data.SeatId;
                    entity.ProductId = data.ProductId;
                    entity.CartStatusId = data.CartStatusId;
                    entity.UnitPrice = data.UnitPrice;
                    entity.Qty = data.Qty;
                    entity.Notes = data.Notes;
                    entity.OrderTime = data.OrderTime;

                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();

                    return "修改點餐資訊成功";
                }
                catch (ArgumentException e)
                {
                    return $"修改點餐資訊失敗,{e.Message}";
                }
            }
            else
            {
                return "修改點餐資訊失敗";
            }
        }

        // DELETE api/<SeatCartsController>/5  刪除
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var seatCart = _context.SeatCarts.Find(id);

            if (seatCart == null)
            {
                return "刪除點餐失敗!";
            }

            try
            {
                _context.SeatCarts.Remove(seatCart);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return "刪除點餐失敗!";
            }

            return "刪除點餐成功!";
        }
    }
}
