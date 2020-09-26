using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string address, string subject, string body);
    }
}
