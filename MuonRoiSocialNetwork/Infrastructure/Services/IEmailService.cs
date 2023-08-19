using MuonRoiSocialNetwork.Infrastructure.Extentions.Mail;

namespace MuonRoiSocialNetwork.Infrastructure.Services
{
    /// <summary>
    /// Declare interface mail services
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send mail confirm
        /// </summary>
        /// <param name="userEmailOptions"></param>
        /// <returns></returns>

        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);
        /// <summary>
        /// Send mail reset password
        /// </summary>
        /// <param name="userEmailOptions"></param>
        /// <returns></returns>

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}
