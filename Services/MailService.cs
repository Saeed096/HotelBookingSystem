using HotelBookingSystem.Helpers;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Settings;
using HotelBookingSystem.TemplateMails;
using HotelBookingSystem.ViewModels;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mime;

namespace HotelBookingSystem.Services
{
    public class MailService : IMailService 
    {
        private readonly MailSettings _mailsettings;

        public MailService(IOptions<MailSettings> mailsettings)
        {
            _mailsettings = mailsettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailrequest, mailTemplate mailTemplate,
            MailAdditionalParamsViewModel? additionalParams = null)
        {
            MimeMessage mail = new MimeMessage();
            mail.Sender = MailboxAddress.Parse(_mailsettings.Mail);
            mail.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));    
            mail.Subject = mailrequest.Subject;


            BodyBuilder builder = new BodyBuilder();

            if (mailrequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailrequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())                          {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, MimeKit.ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailTemplate.htmlTags(additionalParams);

            mail.Body = builder.ToMessageBody();
        

            using (MailKit.Net.Smtp.SmtpClient smtp = new MailKit.Net.Smtp.SmtpClient()) 
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; 
                smtp.CheckCertificateRevocation = false; 

                smtp.Connect(_mailsettings.Host, _mailsettings.Port, MailKit.Security.SecureSocketOptions.Auto);
                smtp.Authenticate(_mailsettings.Mail, _mailsettings.Password);

                await smtp.SendAsync(mail);
                smtp.Disconnect(true);
            }
        }
    }
}
