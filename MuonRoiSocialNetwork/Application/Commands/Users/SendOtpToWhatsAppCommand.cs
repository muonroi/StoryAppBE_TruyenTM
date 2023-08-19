using BaseConfig.MethodResult;
using MediatR;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using Flurl.Http;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Common.Models.Users.Request.Whatsapp;
using BaseConfig.Extentions.String;
using BaseConfig.EntityObject.Entity;
using MuonRoi.Social_Network.Users;
using PhoneNumbers;
using BaseConfig.Exeptions;
using BaseConfig.EntityObject.EntityObject;
using Serilog;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;

namespace MuonRoiSocialNetwork.Application.Commands.Users
{
    /// <summary>
    /// Send Otp then register new user
    /// </summary>
    public class SendOtpToWhatsAppCommand : IRequest<MethodResult<long>>
    {
        /// <summary>
        /// Phone number give otp
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
    /// <summary>
    /// Handle
    /// </summary>
    public class SendOtpToWhatsAppCommandHandler : IRequestHandler<SendOtpToWhatsAppCommand, MethodResult<long>>
    {
        private readonly string _hostUri = $"https://graph.facebook.com/v17.0/108335998999395/messages";
        private readonly IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        public SendOtpToWhatsAppCommandHandler(IConfiguration configuration, ILoggerFactory logger, IDistributedCache cache)
        {
            _configuration = configuration;
            _logger = logger.CreateLogger<SendOtpToWhatsAppCommandHandler>();
            _cache = cache;
        }
        /// <summary>
        /// Handle send otp code
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<long>> Handle(SendOtpToWhatsAppCommand request, CancellationToken cancellationToken)
        {
            MethodResult<long> methodResult = new();
            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                #region Valid data then phone number
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC49C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC49C), nameof(EnumUserErrorCodes.USRC49C) ?? "") }
                    );
                    methodResult.Result = -1;
                    return methodResult;
                }
                if (string.IsNullOrEmpty(request.PhoneNumber))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR21C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USR21C), nameof(EnumUserErrorCodes.USR21C) ?? "") }
                    );
                    methodResult.Result = -1;
                    return methodResult;
                }
                PhoneNumber phoneNumber = phoneNumberUtil.Parse(request.PhoneNumber, "VN");
                phoneNumberUtil.IsValidNumber(phoneNumber);
                #endregion
                #region Send Otp with random code
                string otpCode = StringManagers.GenerateOtp().ToString();
                if (otpCode == "-1")
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC53C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC53C), nameof(EnumUserErrorCodes.USRC53C) ?? "") }
                    );
                    methodResult.Result = -1;
                    Log.Error($"Send Otp failure! {otpCode}");
                    return methodResult;
                }
                IFlurlResponse sendOtpResponse = await _hostUri.WithHeaders(new { Content_Type = "application/json", Authorization = $"Bearer {_configuration[ConstAppSettings.Instance.WHATSAPP_ACESSTOKEN]}" }).PostJsonAsync(new WhatsappMessageOtpModelRequest
                {
                    MessagingProduct = "whatsapp",
                    RecipientType = "individual",
                    To = $"{phoneNumber.CountryCode}{phoneNumber.NationalNumber}",
                    Type = "template",
                    Template = new Template
                    {
                        Name = "send_otp",
                        Language = new Language
                        {
                            Code = "en_US"
                        },
                        Components = new List<Component>
                    {
                        new Component
                        {
                            Type = "body",
                            Parameters = new List<Parameter>
                            {
                                new Parameter
                                {
                                    Type = "text",
                                    Text = otpCode
                                }
                            }
                        },
                        new Component
                        {
                            Type = "button",
                            SubType = "url",
                            Index = 0,
                            Parameters = new List<Parameter>
                            {
                                new Parameter
                                {
                                    Type = "text",
                                    Text = otpCode
                                }
                            }
                        }
                    }
                    },
                });
                if (sendOtpResponse.StatusCode != 200)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC53C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC53C), nameof(EnumUserErrorCodes.USRC53C) ?? "") }
                    );
                    methodResult.Result = -1;
                    Log.Error(sendOtpResponse.ResponseMessage.ReasonPhrase ?? $"Send Otp failure! {JsonConvert.SerializeObject(sendOtpResponse)}");
                    return methodResult;
                }
                #endregion
                methodResult.Result = long.Parse(otpCode);
                await _cache.SetRecordAsync($"otp-{request.PhoneNumber}", otpCode, SettingUserDefault.Instance.minuteExpitryOtpCode);
                return methodResult;
            }
            catch (FlurlHttpException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(SendOtpToWhatsAppCommand) STEP FlurlHttpException --> Message {ex} ---->");
                methodResult.AddResultFromErrorList(new List<ErrorResult>
                {
                    new ErrorResult
                    {
                        ErrorCode = ex.StatusCode.Value.ToString(),
                        ErrorMessage = ex.Message
                    }
                });
                methodResult.Result = -1;
                return methodResult;
            }
            catch (NumberParseException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(SendOtpToWhatsAppCommand) STEP NumberParseException --> Message {ex} ---->");
                methodResult.AddResultFromErrorList(new List<ErrorResult>
                { new ErrorResult
                {
                    ErrorCode ="400",
                    ErrorMessage = ex.Message
                }
                });
                methodResult.Result = -1;
                return methodResult;
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(SendOtpToWhatsAppCommand) STEP CUSTOMEXCEPTION --> Message {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = -1;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(SendOtpToWhatsAppCommand) STEP EXEPTION MESSAGE --> Message {ex} ---->");
                _logger.LogError($" -->(SendOtpToWhatsAppCommand) STEP EXEPTION STACK --> Message {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = -1;
                return methodResult;
            }

        }
    }
}
