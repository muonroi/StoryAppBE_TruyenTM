using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.JWT;
using BaseConfig.MethodResult;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Application.Commands.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Common.Settings.RefreshTokenSettings;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.Extentions.Mail;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using MuonRoiSocialNetwork.Infrastructure.Services;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Email
{
    /// <summary>
    /// Request resend mail
    /// </summary>
    public class ResendMailVeritificationCommand : IRequest<MethodResult<bool>>
    {
    }
    /// <summary>
    /// Class handle
    /// </summary>
    public class ResendMailVeritificationCommandHandler : BaseUserCommandHandler, IRequestHandler<ResendMailVeritificationCommand, MethodResult<bool>>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ResendMailVeritificationCommandHandler> _logger;
        private readonly IDistributedCache _cache;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userQueries"></param>
        /// <param name="userRepository"></param>
        /// <param name="emailService"></param>
        /// <param name="logger"></param>
        /// <param name="cache"></param>
        /// <param name="authContext"></param>
        public ResendMailVeritificationCommandHandler(IMapper mapper, IConfiguration configuration, IUserQueries userQueries, IUserRepository userRepository,
            IEmailService emailService,
            ILoggerFactory logger, IDistributedCache cache, AuthContext authContext) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _emailService = emailService;
            _logger = logger.CreateLogger<ResendMailVeritificationCommandHandler>();
            _cache = cache;
        }
        /// <summary>
        /// Func handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(ResendMailVeritificationCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                #region Get cache Get user by user id
                UserModelResponse? userGetLogin = await _cache?.GetRecordAsync<UserModelResponse>($"{RefreshTokenDefault.Instance.keyUserModelResponseLogin}_{Guid.Parse(_authContext.CurrentUserId)}");
                if (userGetLogin is null || request is null)
                {
                    MethodResult<BaseUserResponse> resultBaseUserLogin = await _userQueries.GetUserModelByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
                    if (resultBaseUserLogin.StatusCode == 400 || resultBaseUserLogin.Result == null || !resultBaseUserLogin.IsOK)
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumUserErrorCodes.USRC47C),
                            new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                        );
                        methodResult.Result = false;
                        return methodResult;
                    }
                    userGetLogin = _mapper.Map<UserModelResponse>(resultBaseUserLogin.Result);
                    if (userGetLogin is null)
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumUserErrorCodes.USRC47C),
                            new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                        );
                        methodResult.Result = false;
                        return methodResult;
                    }
                }
                #endregion

                #region Get user by user id
                var appUser = await _userQueries.GetByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
                if (appUser.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Check lock time
                DateTime timeNow = DateTime.UtcNow.AddHours(SettingUserDefault.Instance.hourAsia);
                TimeSpan? checkTimeLock = timeNow - appUser.Result.LockoutEnd;
                if (appUser.Result.LockoutEnabled && appUser.Result.LockoutEnd != null && checkTimeLock > TimeSpan.Zero)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC48C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Reset lock time
                if (!appUser.Result.LockoutEnabled || !(checkTimeLock > TimeSpan.Zero))
                {
                    appUser.Result.CountRequestSendMail = 0;
                    appUser.Result.LockoutEnd = null;
                    appUser.Result.LockoutEnabled = false;
                }
                #endregion

                #region Check count request
                if (appUser.Result.CountRequestSendMail >= SettingUserDefault.Instance.maxNumberRequestSendMail)
                {
                    appUser.Result.LockoutEnabled = true;
                    DateTimeOffset lockTime = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(SettingUserDefault.Instance.hourAsia));
                    appUser.Result.LockoutEnd = lockTime.AddHours(2);
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC48C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                    );
                    await _userRepository.UpdateUserAsync(appUser.Result);
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region update count request
                appUser.Result.CountRequestSendMail++;
                await _userRepository.UpdateUserAsync(appUser.Result);
                #endregion

                #region  Send mail comfirm
                string jobId = BackgroundJob.Enqueue<CreateUserCommandHandler>(x => x.GenerateEmailConfirmationTokenAsync(appUser.Result));
                RecurringJob.TriggerJob(jobId);
                #endregion

                methodResult.StatusCode = StatusCodes.Status200OK;
                methodResult.Result = true;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(ResendMailVeritification) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(ResendMailVeritification) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }

        }
        /// <summary>
        /// Genarate mail and token confirm user register
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        public async Task GenerateEmailConfirmationTokenAsync(AppUser identityUser)
        {
            GenarateJwtToken genarateJwtToken = new(_configuration);
            UserModelResponse userModel = _mapper.Map<UserModelResponse>(identityUser);
            string token = genarateJwtToken.GenarateJwt(userModel, SettingUserDefault.Instance.minuteExpitryConfirmEmail);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmail(identityUser, token);
            }
        }
        private async Task SendEmailConfirmationEmail(AppUser user, string token)
        {
            string appDomain = _configuration.GetSection(ConstAppSettings.Instance.APPLICATIONAPPDOMAIN).Value;
            string confirmationLink = _configuration.GetSection(ConstAppSettings.Instance.APPLICATIONEMAILCONFIRMED).Value;

            UserEmailOptions options = new()
            {
                ToEmails = new List<string>() { user.Email ?? "" },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.UserName ?? ""),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };
            await _emailService.SendEmailForEmailConfirmation(options);
        }
    }
}
