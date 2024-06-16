using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utilities;
namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IzakayaContext _dbContext;
        private readonly IConfiguration _config;
        private readonly EmailHelper _emailHelper;

        public ForgotPasswordController(IzakayaContext dbContext, IConfiguration configuration, EmailHelper emailHelper)
        {
            _dbContext = dbContext;
            _config = configuration;
            _emailHelper = emailHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ForgotPasswordRequest request)
        {
            var user = await _dbContext.Members.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return NotFound("找不到使用該郵件的帳戶。");
            }

            // 生成一個安全的隨機令牌
            var token = GenerateToken(6);
            var expirationTime = DateTime.UtcNow.AddHours(1); // 設定令牌1小時後過期

            // 將令牌和過期時間保存到用戶記錄中
            user.AuthenticationCode = token;
            user.AuthenticationCodeGeneratedAt = expirationTime;
            await _dbContext.SaveChangesAsync();

            var resetLink = $"https://localhost:8080/ResetPassword?token={token}&email={user.Email}";

            // 使用 EmailHelper 發送重置密碼郵件
            bool emailSent = _emailHelper.SendForgetPasswordEmail(resetLink, user.Name, user.Email, token);


            if (!emailSent)
            {
                return StatusCode(500, "無法發送密碼重置郵件。");
            }

            return Ok("已發送密碼重置郵件。");
        }


        private string GenerateToken(int length)
        {
            // 創建一個足夠長的陣列來存儲隨機數
            var randomNumber = new byte[length];

            // 使用RandomNumberGenerator填充陣列
            RandomNumberGenerator.Fill(randomNumber);

            // 將隨機數轉換為Base64字串
            var token = Convert.ToBase64String(randomNumber);

            // 因為Base64字串可能包含對URL不友好的字符，所以進行替換或裁剪
            token = token.Replace("+", "")
                         .Replace("/", "")
                         .Replace("=", "")
                         .Substring(0, length); // 確保字串長度符合要求

            return token;
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _dbContext.Members
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.AuthenticationCode == model.Token);

            if (user == null || user.AuthenticationCodeGeneratedAt == null || user.AuthenticationCodeGeneratedAt.Value.AddHours(1) < DateTime.UtcNow)
            {
                return BadRequest("無效或過期的令牌。");
            }

            // 令牌驗證通過，更新密碼
            // 注意：這裡應該對新密碼進行哈希處理後存儲
            user.Password = HashHalper.ToSHA256(model.NewPassword, user.Salt);
            user.AuthenticationCode = null; // 清除令牌
            user.AuthenticationCodeGeneratedAt = null; // 清除令牌生成時間
            await _dbContext.SaveChangesAsync();

            return Ok("密碼已成功重置。");
        }
        public class ResetPasswordModel
        {
            public string Token { get; set; }
            public string Email { get; set; }
            public string NewPassword { get; set; }
        }
    }
}
