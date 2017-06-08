using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Matslump.Models
{
    public class Email
    {
        public static void SendEmail(string epost , string name, string Subject, string Message)
        {

            string Sender = "mat@nppc.se";
            string Password = "dapobuz1";
            string Emailsmtp = "mail.nppc.se";
            

            
            
            SmtpClient client = new SmtpClient(Emailsmtp, 587);
            MailMessage mail = new MailMessage();

                
                    client.EnableSsl = false;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Sender, Password);

                    var inlineLogo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Content/apa.jpg"));
                    inlineLogo.ContentId = Guid.NewGuid().ToString();

                    string body = string.Format(@"
                    <p>{0}</p>
                    <p>{1}</p>
                    <img src=""cid:{2}"" />
                    <p>Ha en trevlig dag</p>
                    <p>Med vänliga hälsningar Matslumpiatorn</p> 
                    ", "Hej " + name , Message, inlineLogo.ContentId);

                    mail = new MailMessage(Sender, epost, Subject, Message);
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    view.LinkedResources.Add(inlineLogo);
                    mail.AlternateViews.Add(view);
               

                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            
        }
    }
}