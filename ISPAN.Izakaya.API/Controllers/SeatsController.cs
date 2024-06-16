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
    public class SeatsController : ControllerBase
    {
        private readonly IzakayaContext _context;
        public SeatsController(IzakayaContext context)
        {
            _context = context;
        }

        // GET: api/<SeatsController>  取得全部座位
        [HttpGet]
        public IEnumerable<SeatDto> Get()
        {
            var query = _context.Seats.AsNoTracking()
                .Select(s => new SeatDto
                {
                    Id = s.Id,
                    BranchId = s.BranchId,
                    Name = s.Name,
                    Status = s.Status,
                })
                .ToList();

            return query;
        }

        // GET api/<SeatsController>/2 依店家取得座位(id=branchId)
        [HttpGet("{branchId}")]
        public IEnumerable<SeatDto> Get(int branchId)
        {
            var query = _context.Seats.AsNoTracking()
            .Select(s => new SeatDto
            {
                Id = s.Id,
                BranchId = s.BranchId,
                Name = s.Name,
                Status = s.Status,
            })
             .InBranchId(branchId)
             .ToList();
            return query;
        }

        // GET api/<SeatsController>/OTP/B1 依店家取得座位(id=branchId)
        [HttpGet("OTP/{seatName}")]
        public string Get(string seatName)
        {
            var query = _context.Seats.Where(s => s.Name == seatName).First();
            return query.QrcodeLink;
        }


        //// POST api/<SeatsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<SeatsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<SeatsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
