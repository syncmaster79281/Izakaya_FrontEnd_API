using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Extensions;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentsController : ControllerBase
    {
        private readonly IzakayaContext _context;
        public OrderPaymentsController(IzakayaContext context)
        {
            _context = context;
        }

        // GET: api/<OrderPaymentsController>
        [HttpGet]
        public IActionResult Get([FromQuery] OrderPaymentDto dto)
        {
            try
            {
                var dayStart = dto.PaymentTime.Date;
                var dayEnd = dto.PaymentTime.Date.AddDays(1).AddTicks(-1);

                var query = _context.OrderPayments.AsNoTracking()
                    .Include(o => o.PaymentStatus)
                    .Select(o => new OrderPaymentDto
                    {
                        Id = o.Id,
                        MemberId = o.MemberId,
                        CombinedOrderId = o.CombinedOrderId,
                        PaymentMethodId = o.PaymentMethodId,
                        PaymentStatusId = o.PaymentStatusId,
                        PaymentTime = o.PaymentTime,
                        TotalAmount = o.TotalAmount,
                        Discount = o.Discount,
                        NetAmount = o.NetAmount,
                        PaymentStatus = o.PaymentStatus.Status
                    })
                    .InMemberId(dto.MemberId)
                    .InCombinedOrderId(dto.CombinedOrderId)
                    .InPaymentMethodId(dto.PaymentMethodId)
                    .InPaymentStatusId(dto.PaymentStatusId)
                    .OrderTimeBetween(dayStart, dayEnd)
                    .ToList();

                if (query.Count == 0)
                {
                    return NotFound("查無資料");
                }

                return Ok(query);
            }
            catch (FormatException ex)
            {
                return BadRequest($"參數錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "網路錯誤");
            }
        }

        // GET api/<OrderPaymentsController>/5
        [HttpGet("{memberId}")]
        public IActionResult Get(int memberId)
        {
            try
            {
                var query = _context.OrderPayments.AsNoTracking()
                    .Include(o => o.PaymentStatus)
                    .Where(o => o.MemberId == memberId)
                    .Select(o => new OrderPaymentDto
                    {
                        Id = o.Id,
                        MemberId = o.MemberId,
                        CombinedOrderId = o.CombinedOrderId,
                        PaymentMethodId = o.PaymentMethodId,
                        PaymentStatusId = o.PaymentStatusId,
                        PaymentTime = o.PaymentTime,
                        TotalAmount = o.NetAmount,
                        PaymentStatus = o.PaymentStatus.Status,
                        PaymentMethod = o.PaymentMethod.Method
                    }).ToList();

                if (query.Count == 0)
                {
                    return NotFound("查無資料");
                }

                return Ok(query);
            }
            catch (FormatException ex)
            {
                return BadRequest($"參數錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "網路錯誤");
            }
        }


        // GET api/<OrderPaymentsController>/5
        [HttpGet("GetOrder/{combinedOrderId}")]
        public IActionResult GetOrder(int combinedOrderId)
        {
            try
            {
                var query = _context.OrderPayments.AsNoTracking()
                    .Include(o => o.PaymentStatus)
                    .Where(o => o.CombinedOrderId == combinedOrderId)
                    .Select(o => new OrderPaymentDto
                    {
                        Id = o.Id,
                        MemberId = o.MemberId,
                        CombinedOrderId = o.CombinedOrderId,
                        PaymentMethodId = o.PaymentMethodId,
                        PaymentStatusId = o.PaymentStatusId,
                        PaymentTime = o.PaymentTime,
                        NetAmount = o.NetAmount,
                        PaymentStatus = o.PaymentStatus.Status,
                        PaymentMethod = o.PaymentMethod.Method
                    }).SingleOrDefault();

                if (query == null)
                {
                    return NotFound("查無資料");
                }

                return Ok(query);
            }
            catch (FormatException ex)
            {
                return BadRequest($"參數錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "網路錯誤");
            }
        }

        // GET api/<OrderPaymentsController>/GetRewards/5
        [HttpGet("GetRewards/{memberId}")]
        public IActionResult GetRewards(int memberId)
        {
            try
            {
                var query = _context.Rewards
                    .AsNoTracking()
                    .Include(r => r.Coupon)
                    .Where(r => r.MemberId == memberId && r.Coupon.DiscountMethod >= 0)
                    .Select(r => new PaymentRewardDto
                    {
                        Id = r.Id,
                        Name = r.Coupon.Name,
                        DiscountMethod = r.Coupon.DiscountMethod,
                        IsUsed = r.Coupon.IsUsed,
                        CouponId = r.CouponId,
                        MemberId = r.MemberId,
                        Qty = r.Qty
                    }).ToList();

                if (query.Count == 0)
                {
                    return NotFound("查無資料");
                }

                return Ok(query);

            }
            catch (Exception e)
            {
                return StatusCode(500, "查無資料");
            }
        }


        // GET api/<OrderPaymentsController>/GetPaymentList/5
        [HttpGet("GetPaymentList/{combinedOrderId}")]
        public IActionResult GetPaymentList(int combinedOrderId)
        {
            try
            {
                var orderPayment = _context.OrderPayments
                    .AsNoTracking()
                    .Where(o => o.CombinedOrderId == combinedOrderId).Single();

                if (orderPayment == null)
                {
                    return NotFound("查無資料");
                }

                var orderDatas = _context.Orders
                    .AsNoTracking()
                    .Where(o => o.CombinedOrderId == combinedOrderId).Select(o => new PackageDto
                    {
                        Id = o.Id.ToString(),
                        Name = o.Seat.Name,
                    }).ToList();

                var result = GetPaymentList(orderPayment, orderDatas);
                return Ok(result);

            }
            catch (Exception e)
            {
                return StatusCode(500, "查無資料");
            }
        }

        private OrderListDto GetPaymentList(OrderPayment orderPayment, List<PackageDto> orderDatas)
        {
            var result = new OrderListDto();

            result.Amount = orderPayment.NetAmount;
            result.Currency = "TWD";
            result.OrderId = orderPayment.CombinedOrderId.ToString();


            foreach (var item in orderDatas)
            {
                var list = new PackageDto();

                list.Id = item.Id;
                list.Name = item.Name;
                list.Products = _context.OrderDetails
                    .AsNoTracking()
                    .Where(o => o.OrderId.ToString() == item.Id)
                    .Select(o => new ProductItem
                    {
                        Name = o.Product.Name,
                        ImageUrl = o.Product.ImageUrl,
                        Price = o.UnitPrice,
                        Quantity = o.Qty,

                    }).ToList();

                result.Packages.Add(list);
            }

            return result;
        }


        // POST api/<OrderPaymentsController>
        [HttpPost]
        public string Post([FromBody] OrderPaymentDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                _context.OrderPayments.Add(entity);
                _context.SaveChanges();

                return $"訂單ID:{entity.Id},新增成功";
            }
            catch (ArgumentException e)
            {
                return $"訂單新增失敗,{e.Message}";
            }
        }

        // PUT api/<OrderPaymentsController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] OrderPaymentDto dto)
        {

            if (id != dto.CombinedOrderId)
            {
                return "修改失敗";
            }

            OrderPayment entity = _context.OrderPayments
                .AsNoTracking()
                .Where(o => o.CombinedOrderId == dto.CombinedOrderId)
                .SingleOrDefault();




            if (entity != null)
            {
                try
                {
                    var data = dto.ToEntity();
                    entity.PaymentStatusId = data.PaymentStatusId;
                    entity.Discount = entity.TotalAmount - data.NetAmount;
                    entity.NetAmount = data.NetAmount;
                    entity.PaymentTime = data.PaymentTime;

                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();

                    return "修改訂單成功";
                }
                catch (ArgumentException e)
                {
                    return $"修改訂單失敗,{e.Message}";
                }
            }
            else
            {
                return "修改訂單失敗";
            }
        }

        //刪除票夾
        // DELETE api/<OrderPaymentsController>/5
        [HttpDelete("DeleteReward/{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.Rewards.Find(id);

            if (entity == null)
            {
                return NotFound("找不到指定的票夾。");
            }

            if (entity.Qty <= 0)
            {
                return BadRequest("票夾數量無效。");
            }

            try
            {
                if (entity.Qty == 1)
                {
                    _context.Rewards.Remove(entity);
                }
                else
                {
                    entity.Qty -= 1;
                }
                _context.SaveChanges();
                return Ok("刪除票夾成功!");
            }
            catch (Exception)
            {
                return StatusCode(500, "刪除票夾過程中發生錯誤。");
            }
        }
    }
}
