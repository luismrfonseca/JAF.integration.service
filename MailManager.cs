using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JAF.integration.service
{
    class MailManager
    {
        private string smtpServers;
        private bool smtpSSL;
        private int smptPort;
        private bool smtpDefaultCredentials;
        private string smtpUsername;
        private string smtpPassword;

        public MailManager(string smtpServers, bool smtpSSL, int smptPort, bool smtpDefaultCredentials, string smtpUsername, string smtpPassword)
        {
            this.smtpServers = smtpServers;
            this.smtpSSL = smtpSSL;
            this.smptPort = smptPort;
            this.smtpDefaultCredentials = smtpDefaultCredentials;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }

        public void sendLogByMail(string mailFrom, string mailTo, string mailSubject, string mailBody, string pathLog)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailFrom);
            string[] mailsTo = mailTo.Split(';');

            foreach (var mTo in mailsTo)
            {
                mail.To.Add(mTo);
            }
            mail.Subject = mailSubject;
            mail.Body = mailBody;

            mail.Attachments.Add(new Attachment(pathLog));

            using (var smtp = new SmtpClient(smtpServers))
            {
                smtp.EnableSsl = this.smtpSSL;
                smtp.Port = this.smptPort;

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = this.smtpDefaultCredentials;

                if (!this.smtpDefaultCredentials)
                {
                    smtp.Credentials = new NetworkCredential(this.smtpUsername, this.smtpPassword);
                }

                smtp.Send(mail);
            }
        }
    }
}
