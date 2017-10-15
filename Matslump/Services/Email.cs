using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Matslump.Services
{
    public class Email
    {
        public static void SendEmail(string epost , string name, string Subject, string body)
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

                    //var inlineLogo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Content/apa.jpg"));
                    //inlineLogo.ContentId = Guid.NewGuid().ToString();
                    //            <img src=""cid:{2}"" />
                    //string body = string.Format(@"
                    //<b>{0}</b>
                    //<p>{1}</p>

                    //<p>Ha en trevlig dag</p>
                    //<p>Med vänliga hälsningar Matslumpiatorn</p> 
                    //", "Hej " + name , Message /*,inlineLogo.ContentId*/);

                    mail = new MailMessage(Sender, epost, Subject, body);
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    //view.LinkedResources.Add(inlineLogo);
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
        public static string Emailslumplist(string name , string Message, List<Receptmodels> lista)
        {
            string test;
            test = "<style>table {width:420px;}table, th, td { border: 1px solid black;border-collapse: collapse;}th, td {padding: 5px;text-align: left;}</style><table>";
            foreach (var item in lista)
            {
                test +="<tr> <th>" + item.Date.ToShortDateString()+ " "+item.Date.DayOfWeek + "</th><th>" + item.Name + "</th> </tr>";
            }
            test += "</table>";    
        
            string link = "<a href=\"https://matslumpiator.se/\" target = \"_blank\" >Matslumpiatorn </ a > ";
            string body = string.Format(@"
                    <b>{0}</b>
                    <p>{1}</p>
                    <p>{2}</p>

                    <p>Ha en trevlig dag</p>
                    <p>Med vänlig hälsning {3} </p>
                    ", "Hej " + name, Message ,test,link);
            return body;
        }
        public static string EmailOther(string name , string Message)
        {
            string link = "<a href=\"https://matslumpiator.se/\" target = \"_blank\" >Matslumpiatorn </ a > ";
            string body = string.Format(@"
            <b>{0}</b>
            <p>{1}</p>

            <p>Ha en trevlig dag</p>
            <p>Med vänlig hälsning {2}</p>
            ", "Hej " + name, Message , link); /*,inlineLogo.ContentId*/
            return body;
        }
    }
}