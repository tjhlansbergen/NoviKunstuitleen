using System.Threading.Tasks;

namespace NoviKunstuitleen.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}