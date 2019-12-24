/*
    EmailSender.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Services
{
    /// <summary>
    /// Klasse voor verzenden van mail ten behoeve an accountbevestiging, password reset etc.
    /// Implementeert IEmailSender
    /// Op basis van Microsoft template, zie https://go.microsoft.com/fwlink/?LinkID=532713
    /// </summary>
    public class EmailSender : IEmailSender
    {
        // properties
        public AuthMessageSenderOptions Options { get; }
        private IConfiguration _configuration { get; set; }

        // constructor
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IConfiguration configuration)
        {
            Options = optionsAccessor.Value;
            _configuration = configuration;
        }


        /// <summary>
        /// Implementatie van SendEmailAsync, roept execute methode aan
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return SendWithSendGrid(Options.SendGridKey, subject, message, email);
        }


        /// <summary>
        /// Methode voor verzenden van mail via SendGrid API
        /// </summary>
        public Task SendWithSendGrid(string apiKey, string subject, string message, string email)
        {
            // maak nieuwe client
            var client = new SendGridClient(apiKey);

            // maak nieuw bericht
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["MailSenderFrom"]),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            // zet ontvanger
            msg.AddTo(new EmailAddress(email));

            // geen tracking gewenst
            msg.SetClickTracking(false, false);

            // verstuur de mail
            return client.SendEmailAsync(msg);
        }
    }
}
