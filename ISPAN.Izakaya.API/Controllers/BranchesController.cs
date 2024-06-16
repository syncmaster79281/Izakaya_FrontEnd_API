using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public BranchesController(IzakayaContext context)
        {
            _context = context;
        }

        // GET: api/Branches
        [HttpGet]
        public IEnumerable<BranchDto> Get()
        {
            return _context.Branches
                .Select(b => b.ToDto())
                .ToList();
        }

        // GET: api/Branches/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var branch = _context.Branches.Find(id);
                if (branch == null)
                {
                    return NotFound("查無資料");
                }
                return Ok(branch.ToDto());
            }
            catch (Exception e)
            {
                return StatusCode(500, $"查無資料,{e.Message}");
            }
        }

        // PUT: api/Branches/5
        [HttpPut("{id}")]
        public string Put(int id, [FromForm] BranchDto dto)
        {
            //網址列id要和物件內一樣
            if (id != dto.Id)
            {
                return "修改店家資訊失敗";
            }

            Branch entity = _context.Branches.Find(dto.Id);


            try
            {
                var data = dto.ToEntity();
                if (entity != null)
                {
                    entity.Name = data.Name;
                    entity.Address = data.Address;
                    entity.Tel = data.Tel;
                    entity.SeatingCapacity = data.SeatingCapacity;
                    entity.OpeningTime = data.OpeningTime;
                    entity.ClosingTime = data.ClosingTime;
                    entity.RestDay = data.RestDay;

                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();

                    return "修改店家資訊成功!";

                }
                else
                {
                    return "修改店家資訊失敗";
                }
            }
            catch (ArgumentException e)
            {
                return $"修改店家資訊失敗,{e.Message}";
            }
        }

        // POST: api/Branches
        [HttpPost]
        public string Post([FromBody] BranchDto dto)
        {
            try
            {
                var entity = dto.ToEntity();
                _context.Branches.Add(entity);
                _context.SaveChanges();

                return $"店家ID:{entity.Id} 新增成功";
            }
            catch (ArgumentException e)
            {
                return $"店家資訊新增失敗,{e.Message}";
            }
        }

        // DELETE: api/Branches/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var branch = _context.Branches.Find(id);

            //Id不存在
            if (branch == null)
            {
                return "刪除店家資訊失敗!";
            }

            try
            {
                _context.Branches.Remove(branch);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return "刪除店家資訊關聯紀錄失敗!";
            }

            return "刪除店家資訊成功!";
        }
    }
}
