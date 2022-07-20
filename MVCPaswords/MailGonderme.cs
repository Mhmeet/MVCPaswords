using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MVCPaswords
{
    public class MailGonderme
    {
        public void Gonder(string konu, string icerik)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.IsBodyHtml = false;
                    mail.From = new MailAddress("mehmetmeralll111@gmail.com");
                    mail.To.Add("mehmetmerall018@gmail.com");
                    mail.To.Add("mbkaygusuzz@gmail.com");
                    mail.Subject = konu;
                    mail.Body = icerik;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("mehmetmeralll111@gmail.com", "192837465+M");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        

    }
}