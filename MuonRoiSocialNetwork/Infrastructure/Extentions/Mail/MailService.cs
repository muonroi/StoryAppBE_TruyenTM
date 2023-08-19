using Microsoft.Extensions.Options;
using MuonRoiSocialNetwork.Common.Models;
using System.Net.Mail;
using System.Net;
using System.Text;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Infrastructure.Services;
using MuonRoiSocialNetwork.Common.Models.Images;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
namespace MuonRoiSocialNetwork.Infrastructure.Extentions.Mail
{
    /// <summary>
    /// Handle mail service
    /// </summary>
    public class MailService : IEmailService
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Send email confirm
        /// </summary>
        /// <param name="userEmailOptions"></param>
        /// <returns></returns>
        public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Xin chào! {{UserName}}, Vui lòng xác nhận email của bạn", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(await GetEmailBodyAsync("EmailConfirm", _configuration), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="smtpConfig"></param>
        /// <param name="configuration"></param>
        public MailService(IOptions<SMTPConfigModel> smtpConfig, IConfiguration configuration)
        {
            _smtpConfig = smtpConfig.Value;
            _configuration = configuration;
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new()
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new()
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }
        private static async Task<string> GetEmailBodyAsync(string templateName, IConfiguration config)
        {
            string accessKey = config.GetSection(ConstAppSettings.Instance.ENV_ACCESSKEY).Value;
            string secretKey = config.GetSection(ConstAppSettings.Instance.ENV_SERECT).Value;
            AmazonS3Client client = new(accessKey, secretKey, RegionEndpoint.APNortheast1);
            GetObjectRequest request = new()
            {
                BucketName = FolderSetting.BUCKET_USER,
                Key = string.Format(templatePath, templateName),
            };
            string body = "";
            using (var response = await client.GetObjectAsync(request))
            {
                using var streamReader = new StreamReader(response.ResponseStream);
                body = streamReader.ReadToEnd();
            }
            return body;
        }
        private static string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (KeyValuePair<string, string> placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }
        /// <summary>
        /// Handle send mail reset password
        /// </summary>
        /// <param name="userEmailOptions"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Xin chào! {{UserName}}, Vui lòng xác nhận email của bạn", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(await GetEmailBodyAsync("ForgotPassword", _configuration), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }
    }
}
