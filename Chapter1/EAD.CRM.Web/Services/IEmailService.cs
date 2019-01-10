using System.Threading.Tasks;

namespace EAD.CRM.Web.Services
{
    public interface IEmailService
    {
        Task SendEmail(string toAddress, string subject, string htmlContent);
    }
}