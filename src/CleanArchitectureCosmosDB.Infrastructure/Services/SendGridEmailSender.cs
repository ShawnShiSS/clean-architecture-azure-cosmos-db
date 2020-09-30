using CleanArchitectureCosmosDB.Core.Interfaces;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Infrastructure.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string toName, string subject, string message)
        {
            // TODO : implement using SendGrid
            return Task.CompletedTask;
        }
    }
}
