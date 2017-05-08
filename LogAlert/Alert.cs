using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web.Mail;
using System.Net.Mail;
using System.Net;
using System.Web;
namespace LogAlert
{
   public class Alert
    {
        public string logLocation { get; set; }
        public int sizeToAlert { get; set; }
        public int checkTime { get; set; }
        List<string> logFormatList { get; set; }
        List<string> emailsList { get; set; }
        private string sender { get; set; }
        private string smtpServer { get; set; }

        private string mailPort { get; set; }

      

        public Alert()
        {
            logLocation = ConfigurationManager.AppSettings["LogLocation"];
            sizeToAlert = Convert.ToInt32(ConfigurationManager.AppSettings["SizeToAlert"]);
            checkTime = Convert.ToInt32(ConfigurationManager.AppSettings["CheckTime"]);
            string logsFormat = ConfigurationManager.AppSettings["LogFormat"];
            string emails = ConfigurationManager.AppSettings["Emails"];
            sender = ConfigurationManager.AppSettings["Sender"];
            smtpServer = ConfigurationManager.AppSettings["Smtp Server"];
            mailPort = ConfigurationManager.AppSettings["Mail Port"];
            logFormatList =  logsFormat.Split(';').ToList();
            emailsList = emails.Split(';').ToList();
            logFormatList.RemoveAll(string.IsNullOrWhiteSpace);
            emailsList.RemoveAll(string.IsNullOrWhiteSpace);
           
        

        }
        public void CheckSpace()
        {
           
            List<string> files = Directory.GetFiles(logLocation, "*.*", SearchOption.AllDirectories).Where(x => logFormatList.Contains(Path.GetExtension(x), StringComparer.OrdinalIgnoreCase)).ToList();
            FileInfo filesInfoLenght;
            long size =0;
            double sizeInMb;
            for (int i = 0; i < files.Count; i++)
            {
                filesInfoLenght = new FileInfo(files[i]);
                size += filesInfoLenght.Length;

            }

            sizeInMb = ConvertBytesToMb(size);
            if (sizeInMb >= sizeToAlert)
            {
                SendMessage();

            }


        }
        static double ConvertBytesToMb(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public void SendMessage()
        {
            
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(sender.Split(';')[0]);
            for (int i = 0;  i < emailsList.Count; i++)
            {
                mail.To.Add(emailsList[i]);

            }
            mail.Subject = "Notification about logs size";
            SmtpClient client = new SmtpClient();
            client.Host = smtpServer;
            client.Port = Convert.ToInt32(mailPort);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(sender.Split(';')[0], sender.Split(';')[1]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
            mail.Dispose();

        }


    }
}
