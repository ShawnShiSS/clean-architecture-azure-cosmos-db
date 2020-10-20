using CleanArchitectureCosmosDB.Core.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Infrastructure.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        /// <summary>
        ///     Settings
        /// </summary>
        private readonly SendGridEmailSettings _sendGridEmailSettings;

        /// <summary>
        ///     Send Grid wrapper
        /// </summary>
        private readonly SendGridClient _sendGridClient;

        /// <summary>
        ///     FromEmail from the settings
        /// </summary>
        private string FromEmail => _sendGridEmailSettings.FromEmail;

        /// <summary>
        ///     FromName from the settings
        /// </summary>
        private string FromName => _sendGridEmailSettings.FromName;

        /// <summary>
        ///     ctor
        /// </summary>
        /// <param name="sendGridEmailSettings"></param>
        public SendGridEmailSender(IOptions<SendGridEmailSettings> sendGridEmailSettings)
        {
            _sendGridEmailSettings = sendGridEmailSettings.Value ?? throw new ArgumentNullException(nameof(sendGridEmailSettings));.
            _sendGridClient = new SendGridClient(_sendGridEmailSettings.SendGridApiKey);

        }

        // TODO : consider adding support for HTML content
        /// <summary>
        ///     Send message
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="toName"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string toEmail, string toName, string subject, string message)
        {
            SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(
                    new EmailAddress(this.FromEmail, this.FromName),
                    new EmailAddress(toEmail, toName),
                    subject,
                    message,
                    message
                );

            await _sendGridClient.SendEmailAsync(sendGridMessage);
        }
    }

    /// <summary>
    ///     SendGrid email settings
    /// </summary>
    public class SendGridEmailSettings
    {
        /// <summary>
        ///     API Key
        /// </summary>
        public string SendGridApiKey { get; set; }
        /// <summary>
        ///     From Email
        /// </summary>
        public string FromEmail { get; set; }
        /// <summary>
        ///     From Name
        /// </summary>
        public string FromName { get; set; }
    }

}
