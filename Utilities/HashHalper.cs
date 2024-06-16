using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public static class HashHalper
    {
        public static string ToSHA256(string plainText, string salt)
        {
            using (var mySHA256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(string.Concat(salt, plainText));
                var hash = mySHA256.ComputeHash(passwordBytes);
                var sb = new StringBuilder();
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string GetSalt(int saltLenght = 20)
        {
            saltLenght = saltLenght >= 10 ? saltLenght : 10;
            saltLenght = saltLenght < 30 ? saltLenght : 30;
            var random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_=-<>?";
            string salt = "";
            for (int i = 0; i < saltLenght; i++)
            {
                salt += characters[random.Next(characters.Length)];
            }
            return salt;
        }
    }
}