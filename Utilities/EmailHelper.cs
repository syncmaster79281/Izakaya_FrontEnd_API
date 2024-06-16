using System;
using System.Net;
using System.Net.Mail;


namespace Utilities
{
    public class EmailHelper
    {
        private string senderEmail = "ispanizakaya@gmail.com"; // 寄件者
        private string senderPassword = "mtae amis dwsh myzu";
        //private string senderPassword = "Fuen31jack";
        private string smtpAddress = "smtp.gmail.com";
        private int portNumber = 587;
        private bool enableSSL = true;
        public bool SendConfirmRegisterEmail(string url, string name, string email)
        {
            CheckUrlEncoded(url);
            string subject = "[新會員確認信]";
            string body = $@"
            <html>
            <head>
                <style>
                    .button {{
                        background-color: #0095FF;
                        border: none;
                        color: white;
                        padding: 10px 15px;
                        text-align: center;
                        text-decoration: none;
                        display: inline-block;
                        font-size: 16px;
                        margin: 4px 2px;
                        cursor: pointer;
                        border-radius: 4px;
                    }}
                </style>
            </head>
            <body>
                <h2>Hi {name}</h2>
                <p>感謝你加入我們會員！</p>
                <p>請點擊以下按鈕以驗證你的電子信箱！</p>
                <a href='{url}' class='button'>驗證我的帳號</a>
                <p>請注意! 此驗證信在7天內會失效,請在時限內驗證帳號</p>
                <p>如果您沒有提出申請,請忽略本信,謝謝!</p>
            </body>
            </html>";
            var from = senderEmail;
            var to = email;
            var isSend = SendViaGoogle(from, to, subject, body);
            return isSend;
        }

        private void CheckUrlEncoded(string url)
        {
            var decodeUrl = new Uri(url).ToString();
        }

        public bool SendForgetPasswordEmail(string url, string name, string email, string token)
        {
            string subject = "[重設密碼通知]";

            string body = $@"
    <html>
    <head>
        <style>
            .button {{
                background-color: #0095FF;
                border: none;
                color: white;
                padding: 10px 15px;
                text-align: center;
                text-decoration: none;
                display: inline-block;
                font-size: 16px;
                margin: 4px 2px;
                cursor: pointer;
                border-radius: 4px;
            }}
        </style>
    </head>
    <body>
        <h2>Hi {name}</h2>
        <p>您的重設密碼驗證碼為: <strong>{token}</strong></p>
        <p>請點擊此連結 <a href='{url}' class='button'>我要重設密碼</a>, 以進行重設密碼</p>
        <p>如果您沒有提出申請, 請忽略本信, 謝謝</p>
    </body>
    </html>";


            var from = senderEmail;
            var to = email;
            var isSend = SendViaGoogle(from, to, subject, body);
            return isSend;
        }

        public bool SendReservationInformationEmail(string url, string name, string email, string branchName, string message)
        {
            string subject = "[訂位資訊]";
            string body = $@"
            <html>
            <head>
                <style>
                    
                </style>
            </head>
            <body>
                
            </body>
            </html>";
            var from = senderEmail;
            var to = email;
            var isSend = SendViaGoogle(from, to, subject, body);
            return isSend;
        }
        // 實現多載目的
        //  寄信內容
        public bool SendReservationInformationEmail(string url, string name, string email, string branchName, string message, ReservationInfo info)
        {
            string subject = "[訂位資訊]";
            string body = $@"
            <html>
            <head>
                <style>
                    .button {{
                        background-color: #0095FF;
                        border: none;
                        color: white;
                        padding: 10px 15px;
                        text-align: center;
                        text-decoration: none;
                        display: inline-block;
                        font-size: 16px;
                        margin: 4px 2px;
                        cursor: pointer;
                        border-radius: 4px;
                    }}
                </style> 
            </head>
            <body>
                <h2>Hi {name} 你好</h2>
                <p>您已預約 Izakaya {branchName}</p>
                <p>您的訂位資訊如下</p>
                <p>日期:{info.ReservationTime.ToString("yyyy/MM/dd HH:mm")}</p>
                <p>訂位人數:{info.Adult}位大人,{info.Child}位小孩</p>
                <p>訂位電話:{info.Phone}</p>
                <p>備註: {message}</p>
                <p>若需更改您的訂位資訊請點選下方按鈕</p>
                <p><a href='{url}' class='button'>IZAKAYA</a></p>
                <p>祝您今日順心</p>
            </body>
            </html>";
            var from = senderEmail;
            var to = email;
            var isSend = SendViaGoogle(from, to, subject, body);
            return isSend;
        }

        public bool SendChangeEmail(string url, string name, string email, string branchName, string message, ReservationInfo info)
        {
            string subject = "[訂位資訊]";
            string body = $@"
            <html>
            <head>
                <style>
                    .button {{
                        background-color: #0095FF;
                        border: none;
                        color: white;
                        padding: 10px 15px;
                        text-align: center;
                        text-decoration: none;
                        display: inline-block;
                        font-size: 16px;
                        margin: 4px 2px;
                        cursor: pointer;
                        border-radius: 4px;
                    }}
                </style> 
            </head>
            <body>
                <h2>Hi {name} 你好</h2>
                <p>您已更改 Izakaya {branchName}的訂位內容</p>                
                <p>您更改後的訂位資訊如下</p>
                <p>日期:{info.ReservationTime.ToString("yyyy/MM/dd HH:mm")}</p>
                <p>訂位人數:{info.Adult}位大人,{info.Child}位小孩</p>
                <p>訂位電話:{info.Phone}</p>
                <p>備註: {message}</p>
                <p>若您還需要更改訂位資訊請點選下方按鈕</p>
                <p><a href='{url}' class='button'>IZAKAYA</a></p>
                <p>祝您今日順心</p>
            </body>
            </html>";
            var from = senderEmail;
            var to = email;
            var isSend = SendViaGoogle(from, to, subject, body);
            return isSend;
        }


        public bool SendViaGoogle(string from, string to, string subject, string body)
        {
            // todo 以下是開發時,測試之用, 只是建立text file, 不真的寄出信
            //var path = HttpContent.Current.Server.MapPath("~/files/");
            //CreateTextFile(path, from, to, subject, body);
            //return;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                //設定為純文字檔還是要上傳東西
                mail.IsBodyHtml = true;
                //mail.IsBodyHtml = false;
                //mail.Attachments.Add(new Attachment("C:\\Users\\ispan\\Desktop\\TestGoogleDriveApi\\TestGoogleDriveApi\\bin\\Debug\\net8.0\\易大師.jpg"));
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(from, senderPassword);
                    smtp.EnableSsl = enableSSL;
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                }
            }
        }
        //        private void CreateTextFile(string path, string from, string to, string subject, string body)
        //        {
        //            var fileName = $"{to.Replace("@", "_")} {DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
        //            // 文字檔案的完整路徑
        //            var fullPath = Path.Combine(path, fileName);
        //            // 檔案內容
        //            var contents = $@"from:{from}
        //to:{to}
        //subject:{subject}
        //{body}";
        //            // 建立文字檔案, 採用UTF8編碼
        //            File.WriteAllText(fullPath, contents, Encoding.UTF8);
        //        }
    }
    public class ReservationInfo
    {
        public int Adult { get; set; }
        public int Child { get; set; }
        public DateTime ReservationTime { get; set; }
        public string Phone { get; set; }

        public string Random { get; set; }

    }

}
