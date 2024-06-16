using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace Utilities
{
    public static class GoogleDriveHelper
    {
        private static DriveService service;
        //private static string nextPageToken;
        private static void GetDriveService()
        {
            GoogleCredential credential;
            using (FileStream fs = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(fs).CreateScoped(new[] { DriveService.Scope.Drive });
            }
            service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "_applicationName"
            });
            //return service;
        }
        public static List<Google.Apis.Drive.v3.Data.File> GetAllFiles()
        {
            if (service == null) GetDriveService();
            var filesList = new List<Google.Apis.Drive.v3.Data.File>();
            FilesResource.ListRequest request = service.Files.List();
            request.PageSize = 100;
            request.Fields = "nextPageToken, files(id, name,parents,mimeType,size,capabilities,modifiedTime,webViewLink,webContentLink)";
            //listRequest.PageToken = nextPageToken;
            FileList result = request.Execute();
            //nextPageToken = fileFeedList.NextPageToken;
            foreach (var file in result.Files)
            {
                if (!file.MimeType.Contains("folder"))
                {
                    filesList.Add(file);
                }
            }
            return filesList;
        }

        public static bool UploadFile(string fileName)
        {
            try
            {
                if (service == null) GetDriveService();
                var uploadFile = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(fileName),
                };
                FilesResource.CreateMediaUpload request;
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    request = service.Files.Create(uploadFile, stream, GetMimeType(fileName));
                    request.Upload();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static bool DownloadFile(string fileName, string path)
        {
            try
            {
                CheckPath(path);
                if (service == null) GetDriveService();
                var fileIdList = GetFileId(fileName);
                string fileType = Path.GetExtension(fileName).ToLower();
                if (fileIdList.Count > 1)
                {
                    foreach (var fileId in fileIdList)
                    {
                        Download(fileId, fileType, path);
                    }
                }
                else
                {
                    Download(fileIdList[0], fileType, path);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool DeleteFile(string fileId)
        {
            try
            {
                if (service == null) GetDriveService();
                service.Files.Delete(fileId).Execute();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static void CheckPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private static void Download(string fileId, string fileType, string path)
        {
            var request = service.Files.Get(fileId);
            string fileName = Path.GetRandomFileName();
            string newfileName = fileName + fileType;
            string fullfileName = Path.Combine(path, newfileName);
            using (var stream = new FileStream(fullfileName, FileMode.Create))
            {
                request.Download(stream);
            }
        }
        public static List<string> GetFileId()
        {
            var fileList = GetAllFiles();
            var fileIdList = new List<string>();
            foreach (var file in fileList)
            {
                fileIdList.Add(file.Id);
            }
            return fileIdList;
        }
        public static List<string> GetFileId(string fileName)
        {
            if (service == null) GetDriveService();
            var request = service.Files.List();
            request.Q = $"name='{fileName}'";
            var result = request.Execute();
            var fileIdList = new List<string>();
            foreach (var file in result.Files)
            {
                fileIdList.Add(file.Id);
            }
            return fileIdList;
        }
        public static List<string> GetFileUrl()
        {
            var urlList = new List<string>();
            var filesList = GetAllFiles();
            foreach (var file in filesList)
            {
                //urlList.Add(file.WebViewLink);
                var url = GetFileUrl(file.Id);
                urlList.Add(url);
            }
            return urlList;
        }
        public static string GetFileUrl(string fileId)
        {
            return $"https://lh3.googleusercontent.com/d/{fileId}";
        }
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".mp4":
                    return "video/mp4";
                case ".mp3":
                    return "audio/mp3";
                case ".txt":
                    return "text/plain";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                default:
                    return mimeType;
            }
        }
        //public static List<string> GetFiles()
        //{
        //    GoogleCredential credential;
        //    using (FileStream fs = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
        //    {
        //        credential = GoogleCredential.FromStream(fs).CreateScoped(new[] { DriveService.Scope.Drive });
        //    }
        //    DriveService _driveService = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "_applicationName"
        //    });
        //    List<Google.Apis.Drive.v3.Data.File> ListOfFiles = new List<Google.Apis.Drive.v3.Data.File>();
        //    List<string>fileUrl= new List<string>();
        //    FilesResource.ListRequest listRequest = _driveService.Files.List();
        //    listRequest.PageSize = 1000;
        //    listRequest.Fields = "nextPageToken, files(id, name,parents,mimeType,size,capabilities,modifiedTime,webViewLink,webContentLink)";
        //    FileList fileFeedList = listRequest.Execute();
        //    int count = 0;
        //    while (fileFeedList != null)
        //    {
        //        foreach (Google.Apis.Drive.v3.Data.File file in fileFeedList.Files)
        //        {
        //            Console.WriteLine(count);
        //            Console.WriteLine($"https://drive.google.com/uc?export=view&id={file.Id}");
        //            Console.WriteLine("=======");
        //            Console.WriteLine(file.Name);//檔名
        //            Console.WriteLine("=======");
        //            Console.WriteLine(file.Size);
        //            Console.WriteLine("=======");
        //            Console.WriteLine(file.WebViewLink);//url
        //            Console.WriteLine("=======");
        //            ListOfFiles.Add(file);
        //            count++;
        //            fileUrl.Add(file.WebViewLink);
        //        }
        //        if (fileFeedList.NextPageToken == null)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            listRequest.PageToken = fileFeedList.NextPageToken;
        //            fileFeedList = listRequest.Execute();
        //        }
        //    }
        //    return fileUrl;
        //}
    }
}
