using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.EntityFrameworkCore;
using ISPAN.Izakaya.EFModels.Dtos;
using Utilities;

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersUpdateController : ControllerBase
    {
        private readonly IzakayaContext _context;

        public MembersUpdateController(IzakayaContext context)
        {
            _context = context;
        }

        // PUT api/membersupdate/updatephone
        [HttpPut("updatephone")]
        public async Task<IActionResult> UpdatePhone([FromBody] UpdateMembersDto request)
        {
            // 查找会员
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == request.Email);
            if (member == null)
            {
                return NotFound("找不到該會員。");
            }

            // 更新电话号码
            member.Phone = request.Phone;
            await _context.SaveChangesAsync();

            return Ok("會員電話更新成功。");
        }
        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto request)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == request.Email);
            if (member == null)
            {
                return NotFound("找不到該會員。");
            }

            // 为新密码生成盐值并加密
            string newSalt = HashHalper.GetSalt();
            member.Salt = newSalt; // 更新盐值
            member.Password = HashHalper.ToSHA256(request.NewPassword, newSalt); // 使用HashHalper将新密码加密

            await _context.SaveChangesAsync();

            return Ok("會員密碼更新成功。");
        }

    }
}
