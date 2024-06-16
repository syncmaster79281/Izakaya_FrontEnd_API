using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;



namespace Utilities
{
    public static class JwtHelper
    {
        private readonly static string JwtKey = "4sxOuVIOC4geb7f";
        public static string EncryptJwt(string userId, string salt)
        {
            JwtHeader header = new JwtHeader("HS256", "JWT");

            DateTimeOffset now = DateTimeOffset.Now;
            long iat = now.ToUnixTimeSeconds();
            long exp = now.AddMinutes(10).ToUnixTimeSeconds();
            Guid jti = Guid.NewGuid();
            JwtPayload payload = new JwtPayload(exp, iat, userId, jti);

            string b64header = ObjectToString(header);

            string b64payload = ObjectToString(payload);

            string encryptSignature = ComputeHs256(b64header + b64payload, salt + payload.jti + JwtKey);
            string token = b64header + "." + b64payload + "." + encryptSignature;

            return token;
        }

        public static string ComputeHs256(string data, string key)
        {

            var keyBytes = Encoding.UTF8.GetBytes(key);
            using (var hmacSHA = new HMACSHA256(keyBytes))
            {
                var dataBytes = Encoding.UTF8.GetBytes(data);
                var hash = hmacSHA.ComputeHash(dataBytes, 0, dataBytes.Length);
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
        public static bool DecryptJwt(string JwtToken, string salt)
        {
            string[] ary = JwtToken.Split('.');
            if (ary.Length != 3) return false;
            else
            {
                string b64_header = ary[0];
                string b64_payload = ary[1];
                string Signature = ary[2];

                string jsonHeader = EncodeString(b64_header);
                JwtHeader header = JsonConvert.DeserializeObject<JwtHeader>(jsonHeader);

                string jsonPayload = EncodeString(b64_payload);
                JwtPayload payload = JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);

                string encryptSignature = ComputeHs256(b64_header + b64_payload, salt + payload.jti + JwtKey);

                if (!Signature.Equals(encryptSignature)) return false;

                long now = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (payload.iat > now) return false;
                if (now > payload.exp) return false;
            }
            return true;
        }
        private static string ObjectToString(Object model)
        {
            string json = JsonConvert.SerializeObject(model);
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(json);
            string result = Convert.ToBase64String(bytes);
            return result;
        }
        private static string EncodeString(string inputString)
        {
            Byte[] bytes = Convert.FromBase64String(inputString);
            string result = System.Text.UTF8Encoding.UTF8.GetString(bytes);
            return result;
        }
    }
    public class JwtHeader
    {
        public JwtHeader(string alg, string typ)
        {
            this.alg = alg;
            this.typ = typ;
        }
        public string alg { get; set; }
        public string typ { get; set; }
    }
    public class JwtPayload
    {
        public JwtPayload(long exp, long iat, string sub, Guid jti)
        {
            this.exp = exp;
            this.iat = iat;
            this.sub = sub;
            this.jti = jti;
        }
        public long exp { get; set; }
        public long iat { get; set; }
        public string sub { get; set; }
        public Guid jti { get; set; }
    }
}
