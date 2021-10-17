using Book_Store_App.Models;
using System.Threading.Tasks;

namespace Book_Store_App.Services
{
    public interface IEmailServices
    {
        Task SendingEmail(MailRequestModel model);
        Task SendingEmailConfirmationToken(MailRequestModel model);

        Task SendingResetPasswordToken(MailRequestModel model);
    }
}