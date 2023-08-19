using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Users;
using MediatR;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Infrastructure.Extentions.Mail;
using BaseConfig.JWT;
using MuonRoiSocialNetwork.Infrastructure.Services;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Request;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using Hangfire;
using Serilog;
using BaseConfig.Infrashtructure;

namespace MuonRoiSocialNetwork.Application.Commands.Users
{
    /// <summary>
    /// Command for user
    /// </summary>
    public class CreateUserCommand : UserModelRequest, IRequest<MethodResult<UserModelResponse>>
    {
        /// <summary>
        /// Password register
        /// </summary>
        public string? PasswordHash { get; set; }

    }
    /// <summary>
    /// Handler create user
    /// </summary>
    public class CreateUserCommandHandler : BaseUserCommandHandler, IRequestHandler<CreateUserCommand, MethodResult<UserModelResponse>>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IMediator _mediator;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="userQueries"></param>
        /// <param name="configuration"></param>
        /// <param name="emailService"></param>
        /// <param name="logger"></param>
        /// <param name="authContext"></param>
        /// <param name="mediator"></param>
        public CreateUserCommandHandler(IMapper mapper,
            IUserRepository userRepository,
            IUserQueries userQueries,
            IConfiguration configuration,
            IEmailService emailService,
            ILoggerFactory logger, AuthContext authContext, IMediator mediator) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _emailService = emailService;
            _logger = logger.CreateLogger<CreateUserCommandHandler>();
            _mediator = mediator;
        }
        /// <summary>
        /// Handle register
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<UserModelResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            MethodResult<UserModelResponse> methodResult = new();
            try
            {
                #region Validation
                AppUser newUser = _mapper.Map<AppUser>(request);
                newUser.LastLogin = DateTime.UtcNow;
                if (!newUser.IsValid())
                {
                    throw new CustomException(newUser.ErrorMessages);
                }
                #endregion

                #region Check is exist user
                var userExist = await _userRepository.ExistUserByUsernameAsync(newUser.UserName ?? "");
                bool appUser = userExist.Result;
                if (appUser)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR13C),
                        new[] { Helpers.GenerateErrorResult(nameof(newUser.UserName), newUser.UserName ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Genarate salt and password
                newUser.Salt = GenarateSalt();
                newUser.PasswordHash = HashPassword(request.PasswordHash ?? "", newUser.Salt);
                #endregion

                #region Create new user
                newUser.GroupId = SettingUserDefault.Instance.groupDefault;
                newUser.PhoneNumberConfirmed = true;
                var resultCreateUser = await _userRepository.CreateNewUserAsync(newUser);
                if (resultCreateUser.Result <= 0)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC51C),
                        new[] { Helpers.GenerateErrorResult(nameof(newUser.UserName), newUser.UserName ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region  Send mail comfirm
                string jobId = BackgroundJob.Enqueue<CreateUserCommandHandler>(x => x.GenerateEmailConfirmationTokenAsync(newUser));
                RecurringJob.TriggerJob(jobId);
                #endregion

                #region return info new user registed
                AuthUserCommand authUser = new()
                {
                    Username = request.UserName ?? string.Empty,
                    Password = request.PasswordHash ?? string.Empty
                };
                var resultLogin = await _mediator.Send(authUser).ConfigureAwait(false);
                if (!resultLogin.IsOK && resultLogin.StatusCode == StatusCodes.Status400BadRequest)
                {
                    throw new Exception("Error server! Please re-login!");
                }
                UserModelResponse responseUserRegister = resultLogin.Result ?? new UserModelResponse();
                string? name = responseUserRegister.Name;
                responseUserRegister.Name = string.Concat(responseUserRegister.Surname + " ", name);
                methodResult.Result = responseUserRegister;
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(REGISTER) STEP CUSTOMEXCEPTION --> ID USER {ex} ---->");
#pragma warning disable CS8604
                methodResult.AddResultFromErrorList(ex?.ErrorMessages);
#pragma warning restore CS8604
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(REGISTER) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(REGISTER) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
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
            if (_configuration is null)
                return;
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
