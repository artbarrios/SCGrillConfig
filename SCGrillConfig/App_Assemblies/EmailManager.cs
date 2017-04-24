using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SCGrillConfig
{
    public class EmailManager
    {
        public static bool Send(MailMessage mailMessage)
        {
            // for gmail smtp server 
            // NOTE be sure to login to your google account, go to Sign in and security, Connected apps & sites,  
            // and turn on Allow less secure apps 

            // send email sample code
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.To.Add(new MailAddress("art.barrios@outlook.com"));
            //mailMessage.From = new MailAddress("email@email.com");
            //mailMessage.Subject = "Test Email Sent " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            //mailMessage.Body = "Test Email Sent " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            //EmailManager.Send(mailMessage);

            // send the email
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential("astrogator1234@gmail.com", "DX354$^64cd%fc");
                // send the email
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                LogManager.Log("Email Send Error: " + e.Message);
                return false;
            }
        } // Send()
    }
}
