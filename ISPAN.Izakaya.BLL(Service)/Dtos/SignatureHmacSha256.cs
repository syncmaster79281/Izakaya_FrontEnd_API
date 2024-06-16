using System.Security.Cryptography;
using System.Text;

namespace ISPAN.Izakaya.BLL_Service_.Dtos
{
    public static class SignatureHMACSHA256
    {

        public static string GenerateSignature(string channelSecret, string uri, string requestBody, string nonce)
        {
            // 拼接用於 HMAC-SHA256 的訊息
            string message = channelSecret + uri + requestBody + nonce;

            // 將通道密鑰轉換為 byte 陣列
            byte[] keyByte = Encoding.UTF8.GetBytes(channelSecret);

            // 將訊息轉換為 byte 陣列
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // 創建一個 HMACSHA256 實例
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                // 生成簽名的 byte 陣列
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

                // 將簽名的 byte 陣列轉換為 Base64 字串
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
