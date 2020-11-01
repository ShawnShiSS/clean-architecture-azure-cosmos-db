using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string message);
    }
}
