using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string message);
    }
}
