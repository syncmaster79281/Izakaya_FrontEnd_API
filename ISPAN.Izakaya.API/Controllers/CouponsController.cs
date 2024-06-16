using Hangfire;
using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IzakayaContext _context;
        private readonly ICouponService _service;
        private readonly IBackgroundJobClient _backgroundJobClient;


        public CouponsController(IzakayaContext context, ICouponService service)
        {
            _context = context;
            _service = service;
            //_backgroundJobClient = backgroundJobClient;
        }

        // GET: api/<CouponsController>
        [HttpGet]
        public CouponDto Get(int id)
        {
            return _service.Get(id);
        }

        // GET api/<CouponsController>/5
        [HttpGet("{endtime}")]
        public IEnumerable<CouponDto> Get(DateTime endtime)
        {
            var query = _context.Coupons.AsNoTracking()
                .Where(c => c.EndTime <= endtime)
                .Select(c => c.ToDto())
                .ToList();

            return query;
        }



        [HttpGet("hangfire")]
        public OkObjectResult ScheduleBirthdayCouponJob()
        {

            try
            {
                RecurringJob.AddOrUpdate("CouponService", () => _service.Execute(), Cron.Daily);

                return Ok("Birthday coupon job scheduled successfully!");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            // 每天凌晨执行生日礼卷任务

        }

        [HttpGet("hangfireDelete")]
        public OkObjectResult BirthdayCouponDeleteJob()
        {
            //var birthdayCouponDeleteJob = new BirthdayCouponJob(_context, _service);

            // 每天凌晨执行刪除生日礼卷任務
            RecurringJob.AddOrUpdate("CouponService", () => _service.DeleteExpire(), Cron.Daily); ;
            return Ok("Birthday coupon Delete job scheduled successfully!");
        }


    }
}
