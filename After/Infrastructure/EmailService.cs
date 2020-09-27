using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string address, string subject, string body)
        {
            throw new System.NotImplementedException();
        }
    }
}