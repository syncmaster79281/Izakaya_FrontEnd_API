using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // 用於模型驗證

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IzakayaContext _DbContext;

        public UserInfoController(IzakayaContext dbContext)
        {
            _DbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MembersDto memberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 转换 DTO 为实体模型
                var member = new Member
                {
                    Name = memberDto.Name,
                    Phone = memberDto.Phone,
                    Email = memberDto.Email,
                    Birthday = memberDto.Birthday
                };

                _DbContext.Members.Add(member);
                await _DbContext.SaveChangesAsync();

                return Ok(new { message = "用户信息已成功保存" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "保存用户信息失败", error = ex.Message });
            }
        }

        public class MembersDto
        {
            
            public string? Name { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public DateTime? Birthday { get; set; }
        }
    }
}
