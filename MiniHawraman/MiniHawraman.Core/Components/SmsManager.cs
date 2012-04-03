using Twilio;
using System.Configuration;

namespace MiniHawraman.Components
{
    public static class SmsManager
    {
        public static void SendTextMessage(string message)
        {
            TwilioRestClient twilioClient = new TwilioRestClient
                (ConfigurationManager.AppSettings["TwilioAcccountSid"], ConfigurationManager.AppSettings["TwilioAuthToken"]);
            twilioClient.SendSmsMessage(ConfigurationManager.AppSettings["TwilioSenderPhone"],
                ConfigurationManager.AppSettings["TwilioReceiverPhone"], message);
        }
    }
}