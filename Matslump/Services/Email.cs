﻿using Matslump.Models;
using Matslump.Tools;
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
            test = "<table style=\"margin:0px; padding:5px;text-align:left;background-color:#f6f6f7;width:100%;\">";
            foreach (var item in lista)
            {
                test += "<tr> <th width=\"81\"><img src =\"https://matslumpiator.se/content/pic/" + item.Url_pic+"\" alt=\"Bild\"" +
                   " width=\"80\" height=\"80\"></th><th>" + item.Date.ToShortDateString() + " " + Timetool.TranslateDayOfWeek(item.Date.DayOfWeek.ToString()) + "<br>"+ item.Name + "</th> </tr>";
            }
            test += "</table>";
            string start = "<body><div style=\"width:300px;border-radius:5px;background-color:#e3eeee;padding:5px;\">" +
                "<div style=\"color:#e3eeee;background-color:#3d7ee1;width:100%;border-radius:5px;padding-top:2px;padding-bottom:2px;\">" +
                "<a style= \"text-decoration: none;\" href =\"https://matslumpiator.se/\" target=\"_blank\">" +
                "<h1 style= \"font-family: Arial, Helvetica, sans-serif;color:#e3eeee; text-align:center;\"> Matslumpiatorn </h1>  </a></div>" +
                "<div style=\"Padding:5px;\"> <b> Hej " + name + " </b> " +
                "<p> Message</p> </div>";
                     string link = "<a href=\"https://matslumpiator.se/\" target = \"_blank\" >Matslumpiatorn </ a > ";
            string body = string.Format(@"<html>

                      {0}
                      {1}
            

                    <p>Ha en trevlig dag</p>
                    <p>Med vänlig hälsning {2} </p></body></html>
                    ", start,test,link);
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