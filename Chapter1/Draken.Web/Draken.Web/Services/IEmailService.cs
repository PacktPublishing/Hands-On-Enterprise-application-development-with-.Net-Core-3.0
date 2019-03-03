using System.Threading.Tasks;

namespace Draken.Web.Services
{
    public interface IEmailService
    {
        Task SendEmail(string toAddress, string subject, string htmlContent);
    }
}