using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IzakayaContext _dbContext;

        public RegisterController(IzakayaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] MembersDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var existingUser = await _dbContext.Members.FirstOrDefaultAsync(m => m.Email == model.Email);
            if (existingUser != null)
            {
                return BadRequest("該電子郵件已被註冊。");
            }

            string salt = HashHalper.GetSalt();
            string hashedPassword = HashHalper.ToSHA256(model.Password, salt);
            int points = model.Point ?? 0;

            Member newMember = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashedPassword,
                Salt = salt,
                Phone = model.Phone,
                Points = points,
                Birthday = model.Birthday ?? DateTime.Now
            };

            try
            {
                _dbContext.Members.Add(newMember);
                await _dbContext.SaveChangesAsync();
                return Ok("用户註冊成功。");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"註冊過程中發現異常: {ex.Message}");
                return StatusCode(500, "服務器內部錯誤，請稍後在試。");
            }
        }
    }
}
