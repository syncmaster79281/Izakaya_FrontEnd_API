using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {
        private string _salt;
        [HttpPost("{userId}")]
        public string GetToken(string userId)
        {
            //新資料表儲存userId / salt 
            string salt = Guid.NewGuid().ToString();
            _salt = salt;
            var jwt = JwtHelper.EncryptJwt(userId, salt);
            return jwt;
        }

        [HttpGet("{userId}/{JwtToken}")]
        public bool GetClaims(string userId, string JwtToken)
        {
            //解密時去撈資料表userId的salt
            var isValid = JwtHelper.DecryptJwt(JwtToken, _salt);
            return isValid;
        }
        [HttpGet("{url}/{userName}/{email}/{branchName}/{message}")]
        public string SendEmail(string url, string userName, string email, string branchName, string message)
        {
            try
            {
                var service = new EmailHelper();
                var isSend = service.SendReservationInformationEmail(url, userName, email, branchName, message);
                return isSend.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpGet("GetSalt")]
        public string GetSalt()
        {
            string salt = HashHalper.GetSalt();
            return salt;
        }
        [HttpGet("HashPassword/{password}/{salt}")]
        public string HashPassword(string password, string salt)
        {
            var passwordHash = HashHalper.ToSHA256(password, salt);
            return passwordHash;
        }
        [HttpGet("GetUrl")]
        public List<string> GetUrl()
        {
            var url = GoogleDriveHelper.GetFileUrl();
            return url;
        }
        [HttpGet("GetUrl/{fileName}")]
        public string GetUrl(string fileName)
        {
            var fileId = GoogleDriveHelper.GetFileId(fileName);
            if (fileId.Count == 1)
            {
                var url = GoogleDriveHelper.GetFileUrl(fileId[0]);
                return url;
            }
            else
            {
                return fileId.Aggregate((acc, next) => acc + next);
            }
        }
        [HttpGet("GetImgurUrl/{fileName}")]
        public async Task<string> GetImgurUrl(string fileName)
        {
            var image = await ImgurHelper.GetImageInfo(fileName);
            return image.Url;
        }
    }
}
