using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MiniHawraman.Core.Components
{
    public static class Util
    {
        public static DateTime GetDateInLocalTimeZone(DateTime date)
        {
            DateTime convertedDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTime(convertedDate, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
        }

        public static string RemoveSpaceSequences(string text)
        {
            return Regex.Replace(text, @"( )+", " ");
        }

        public static bool IsValidLink(string text)
        {
            string pattern = @"(?<http>(http:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(text))
                return true;
            else
                return false;
        }

        public static bool IsValidEmail(string text)
        {
            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(text))
                return true;
            else
                return false;
        }

        public static string GetPlainTextFromHtml(string html)
        {
            Regex regEx = new Regex(@"<(.|\n)*?>");
            string result = regEx.Replace(html, string.Empty);
            result = result.Replace("&nbsp;", "");
            result = result.Replace("&amp;", "&");
            result = result.Replace("&gt;", ">");
            result = result.Replace("&lt;", "<");
            return result;
        }

        public static string HashString(string text)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static int GetFileLength(string fileUrl)
        {
            WebRequest request = HttpWebRequest.Create(fileUrl);
            request.Method = "HEAD";
            System.Net.WebResponse response = request.GetResponse();

            int contentLength;
            if (int.TryParse(response.Headers.Get("Content-Length"), out contentLength))
            {
                return contentLength;
            }
            return 0;
        }

        public static void LoginNotification(string emailAddress, string ip, string username)
        {
            SmtpClient objSmtp = new SmtpClient(ConfigurationManager.AppSettings["EmailSmtp"]);

            objSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            NetworkCredential objCredential = new NetworkCredential
                (ConfigurationManager.AppSettings["EmailUsername"], ConfigurationManager.AppSettings["EmailPassword"]);
            objSmtp.UseDefaultCredentials = true;
            objSmtp.Credentials = objCredential;
            objSmtp.EnableSsl = true;
            objSmtp.Port = 25;

            MailMessage email = new MailMessage(emailAddress, emailAddress);
            email.Subject = "Login Attempt to Keyvan.FM";
            email.Body = string.Format("An attempt to log into Keyvan.FM from client {0} with username {1}.", ip, username);

            objSmtp.Send(email);
        }
    }
}
