using Book_Store_App.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store_App.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly SMTPConfigModel _smtp;

        public EmailServices(IOptions<SMTPConfigModel> smtp)
        {
            _smtp = smtp.Value;
        }

        public async Task SendingEmail(MailRequestModel model)
        {
            model.Body = UpdatePlaceHolders(ReadTemplate("EmailTemplate.html"), model.PlaceHolders);

            await SendEmail(model);
        }

        public async Task SendingEmailConfirmationToken(MailRequestModel model)
        {
            model.Subject = "Bookly Confirmation Email";
            model.Body = UpdatePlaceHolders(ReadTemplate("EmailConfirmation.html"), model.PlaceHolders);

            await SendEmail(model);
        }

        public async Task SendingResetPasswordToken(MailRequestModel model)
        {
            model.Body = UpdatePlaceHolders(ReadTemplate("ResetPasswordEmail.html"), model.PlaceHolders);

            await SendEmail(model);
        }
        private async Task SendEmail(MailRequestModel model)
        {
            // step one -- MailAddress instance to fill Email Data
            MailMessage mail = new MailMessage
            {
                Subject = model.Subject,
                Body = model.Body,
                From = new MailAddress(_smtp.SenderAddress, _smtp.SenderDisplayName),
                IsBodyHtml = _smtp.IsBodyHTML
            };

            foreach (var email in model.ToEmails)
            {
                mail.To.Add(email);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtp.UserName, _smtp.Password);


            // step two -- SmtpClient instance to fill SMTP request Data
            SmtpClient smtpClient = new SmtpClient()
            {
                Host = _smtp.Host,
                Port = _smtp.Port,
                EnableSsl = _smtp.EnableSSL,
                UseDefaultCredentials = _smtp.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mail);

        }

        private string ReadTemplate(string templateName)
        {
            return File.ReadAllText(Directory.GetCurrentDirectory() + "\\Templates\\" + templateName);
        }

        private string UpdatePlaceHolders(string template, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if(!string.IsNullOrEmpty(template) && keyValuePairs != null)
            {
                foreach(var placeholder in keyValuePairs)
                {
                    template = template.Replace(placeholder.Key, placeholder.Value);
                }
            }
            return template;
        }
    }
}
