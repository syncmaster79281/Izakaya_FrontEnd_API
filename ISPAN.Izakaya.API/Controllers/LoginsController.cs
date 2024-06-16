using ISPAN.Izakaya.EFModels.Models;
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
    public class LoginsController : ControllerBase
    {
        private readonly IzakayaContext _dbContext;
        private readonly string _jwtSecret;
        private readonly IConfiguration _config;

        public LoginsController(IzakayaContext dbContext)
        {
            _dbContext = dbContext;


            _jwtSecret = GetRandomKey(32); // 生成一个长度为 32 的随机密钥
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginRequest request)
        {
            var member = await _dbContext.Members.SingleOrDefaultAsync(m => m.Email == request.Email);

            if (member == null || !VerifyPassword(member.Password, request.Password, member.Salt))
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateJwtToken(member);
            return Ok(new
            {
                Name = member.Name,
                Phone=member.Phone,
                Point = member.Points,
                Email = member.Email,
                Birthday=member.Birthday,
                UserId = member.Id,
                Token = token
            });
        }

        private bool VerifyPassword(string hashedDbPassword, string plainPassword, string salt)
        {

            var hashedPassword = HashHalper.ToSHA256(plainPassword, salt);
            return hashedPassword == hashedDbPassword;
        }

        private string GenerateJwtToken(Member member)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, member.Id.ToString()),
             new Claim(JwtRegisteredClaimNames.Email, member.Email),
              // 根據需要添加其他聲明
            };

            var token = new JwtSecurityToken(
                //issuer: _config["Jwt:Issuer"],
                //audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // 令牌到期時間
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        private string GetRandomKey(int length)
        {
            byte[] bytes = new byte[length / 2];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return BitConverter.ToString(bytes).Replace("-", "");
        }
        

    }
}
