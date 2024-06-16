using Hangfire;
using Humanizer;
using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public RewardsController(IzakayaContext context)
        {
            _context = context;
        }


        // GET api/<RewardsController>/5
        [HttpGet(template: "{memberId}")]
        public IEnumerable<RewardDto> Get(int memberId)
        {
            var query = _context.Rewards.AsNoTracking()
                .Include(r => r.Member)
                .Include(r => r.Coupon)
                .Where(r => r.MemberId == memberId)
                .Select(r => new RewardDto
                {
                    Id = r.Id,
                    CouponId = r.CouponId,
                    MemberId = r.MemberId,
                    Qty = r.Qty,
                    CouponName = r.Coupon.Name,
                    Condition = r.Coupon.Condition,
                    DiscountMethod = r.Coupon.DiscountMethod,
                    StartTime = r.Coupon.StartTime,
                    EndTime = r.Coupon.EndTime,
                    IsUsed = r.Coupon.IsUsed,
                    Description = r.Coupon.Description,
                }).ToList();

            return query;
        }

        // POST api/<RewardsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RewardsController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] RewardDto dto)
        {
            //網址列id要和物件內一樣
            if (id != dto.Id)
            {
                return "轉讓優惠券失敗";
            }

            Reward entity = _context.Rewards.Find(dto.Id);

            if (entity != null)
            {
                try
                {
                    entity.MemberId = dto.MemberId;
                    entity.Qty = dto.Qty;


                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();

                    return "轉讓優惠券成功";
                }
                catch (ArgumentException e)
                {
                    return $"轉讓優惠券失敗,{e.Message}";
                }
            }
            else
            {
                return "轉讓優惠券失敗";
            }
        }


        // DELETE api/<RewardsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }

    
}
