using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    /// accessToken 有時效約一個月 過期必須使用refreshToken clientId client secret 重新取得accessToken
    /// Client secret:7f3ecbf265991e18cb6525373c1f201d8e9cfcae
    /// refresh Token:cd50172b54c152b756447dc09d296fbc6da28c20
    /// 使用時,只有上傳跟刪除,沒有下載,取得imageUrl後在瀏覽器開啟便可下載
    /// 上傳後會取得image的所有資訊,目前只使用imageId,imageUrl,DeleteId 
    /// 使用accessToken 去刪除檔案只需要imageId,使用clientId刪除檔案要使用DeleteId才有權限刪除
    /// </summary>
    public static class ImgurHelper
    {
        private static readonly string albumHash = "lVhWn6g";
        private static readonly string accessToken = "29c7ed1605f60c62a38c61fb542f650116d54b09";
        private static readonly string clientId = "53309291b88f61d";
        public static async Task<string> GetAlbum()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.imgur.com/3/album/{albumHash}");
                request.Headers.Add("Authorization", $"Client-ID {clientId}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }

        }
        public static async Task<ImgurImage> GetImageInfo(string imageId)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.imgur.com/3/image/{imageId}");
                request.Headers.Add("Authorization", $"Client-ID {clientId}");
                var response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                var image = ImgurImage.GetImage(await response.Content.ReadAsStringAsync(), false);
                return image;
            }
        }

        public static async Task<ImgurImage> UploadImageAsync(Stream imageStream)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.imgur.com/3/image");
                request.Headers.Add("Authorization", $"Bearer {accessToken}");

                var content = new MultipartFormDataContent();

                string fileName = "my_image.jpg"; // 或從 imageStream 獲取原始名稱
                string extension = Path.GetExtension(fileName).TrimStart('.');

                var memoryStream = new MemoryStream();

                await imageStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                content.Add(new StreamContent(memoryStream), "image", $"image.{extension}");

                content.Add(new StringContent("image"), "type");
                content.Add(new StringContent(albumHash), "album");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var image = ImgurImage.GetImage(await response.Content.ReadAsStringAsync());
                return image;
            }
        }
        public static async Task<ImgurImage> UploadImageAsync(string imagePath)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.imgur.com/3/image");
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
                var content = new MultipartFormDataContent();
                string fileName = Path.GetFileName(imagePath);
                // 獲取副檔名
                string extension = Path.GetExtension(fileName).TrimStart('.');

                var memoryStream = new MemoryStream();
                using (var fileStream = File.OpenRead(imagePath))
                {
                    memoryStream.SetLength(fileStream.Length);
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    content.Add(new StreamContent(memoryStream), "image", $"image.{extension}");
                }

                content.Add(new StringContent("image"), "type");
                content.Add(new StringContent(albumHash), "album");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var image = ImgurImage.GetImage(await response.Content.ReadAsStringAsync());
                return image;
            }
        }
        public static async Task<string> DeleteImageAsync(string imageId)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://api.imgur.com/3/image/{imageId}");
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return "Delete success";
            }
        }
        public static async Task<string> DeleteImageHashAsync(string deleteHash)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", clientId);

                var deleteUrl = $"https://api.imgur.com/3/image/{deleteHash}";
                var response = await client.DeleteAsync(deleteUrl);
                response.EnsureSuccessStatusCode();
                return "Delete success";
            }
        }
    }
    public class ImgurImage
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string DeleteId { get; set; }
        public static ImgurImage GetImage(string jsonString, bool haveDeleteId = true)
        {
            JObject json = JObject.Parse(jsonString);
            var image = new ImgurImage();
            // 提取 id
            image.Id = json["data"]["id"].ToString();

            // 提取 link
            image.Url = json["data"]["link"].ToString();

            string[] allowExts = { ".jpg", ".jpeg", ".png" };
            string ext = Path.GetExtension(image.Url).ToLower();
            if (!allowExts.Contains(ext))
            {
                string imageType = json["data"]["type"].ToString();
                int index = imageType.IndexOf("/");
                string type = imageType.Substring(index + 1);
                image.Url += type;
            }

            if (haveDeleteId)
            {
                image.DeleteId = json["data"]["deletehash"].ToString();
            }
            return image;
        }
    }
}

