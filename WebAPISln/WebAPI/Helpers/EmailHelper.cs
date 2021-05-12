using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public static class EmailHelper
    {
        public static bool Send(SendEmailModel model)
        {
            MailMessage m = new MailMessage();
            m.To.Add(model.To);
            m.Subject = model.Subject;
            m.Body = model.Body;
            m.From = new MailAddress("guvenliesecim@gmail.com");
            m.IsBodyHtml = false;
            // smtp.gmail.com
            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("guvenliesecim@gmail.com", "");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(m);
                return true;
            }
        }
    }
}
