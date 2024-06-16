using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public ActivitiesController(IzakayaContext context)
        {
            _context = context;
        }

        // GET: api/<ActivitiesController>
        [HttpGet]
        public IEnumerable<ActivityDto> Get()
        {
            var query = _context.Activities.AsNoTracking()
                .Select(a => a.ToDto())
                .ToList();

            return query;
        }

        // GET api/<ActivitiesController>/5 根據結束時間查詢活動
        [HttpGet("{endtime}")]
        public IEnumerable<ActivityDto> Get(DateTime endtime)
        {
            var query = _context.Activities.AsNoTracking()
                .Where(a => a.EndTime <= endtime)
                .Select(a => a.ToDto())
                .ToList();

            return query;
        }


        // PUT api/<ActivitiesController>/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] ActivityDto dto)
        {
            //網址列id要和物件內一樣
            if (id != dto.Id)
            {
                return "修改活動資訊失敗";
            }

            Activity entity = _context.Activities.Find(dto.Id);

            if (entity != null)
            {
                try
                {
                    var data = dto.ToEntity();

                    entity.BranchId = data.BranchId;
                    entity.Name = data.Name;
                    entity.Type = data.Type;
                    entity.Discount = data.Discount;
                    entity.StartTime = data.StartTime;
                    entity.EndTime = data.EndTime;
                    entity.IsUsed = data.IsUsed;
                    entity.Levels = data.Levels;
                    entity.Description = data.Description;

                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();

                    return "修改活動資訊成功";
                }
                catch (ArgumentException e)
                {
                    return $"修改活動資訊失敗,{e.Message}";
                }
            }
            else
            {
                return "修改活動資訊失敗";
            }
        }


    }
}
