using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.EntityObject.EntityObject;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Request;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using PhoneNumbers;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Users
{
    /// <summary>
    /// Valid otp
    /// </summary>
    public class ValidOtpToWhatsAppCommand : IRequest<MethodResult<UserModelResponse>>
    {
        /// <summary>
        /// Otp code sended to phone number
        /// </summary>
        public long OtpCode { get; set; }
        /// <summary>
        /// Info of user register
        /// </summary>
        public UserModelRequest? UserInfoRegister { get; set; }
        /// <summary>
        /// Password register
        /// </summary>
        public string? PasswordHash { get; set; }
    }
    /// <summary>
    /// Handle valid otp
    /// </summary>
    public class ValidOtpToWhatsAppCommandHandler : IRequestHandler<ValidOtpToWhatsAppCommand, MethodResult<UserModelResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ValidOtpToWhatsAppCommandHandler(IMediator mediator, IDistributedCache cache, ILoggerFactory logger, IMapper mapper)
        {
            _mediator = mediator;
            _cache = cache;
            _logger = logger.CreateLogger<ValidOtpToWhatsAppCommandHandler>();
            _mapper = mapper;
        }
        /// <summary>
        /// Function handle valid otp code
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<UserModelResponse>> Handle(ValidOtpToWhatsAppCommand request, CancellationToken cancellationToken)
        {
            MethodResult<UserModelResponse> methodResult = new();
            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                #region Check request
                if (request is null || request.OtpCode == 0 || request.UserInfoRegister is null || string.IsNullOrEmpty(request.UserInfoRegister.PhoneNumber))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC49C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC49C), nameof(EnumUserErrorCodes.USRC49C) ?? "") }
                    );
                    methodResult.Result = null;
                    return methodResult;
                }
                #endregion

                #region Valid phone number
                PhoneNumber phoneNumber = phoneNumberUtil.Parse(request.UserInfoRegister.PhoneNumber, "VN");
                phoneNumberUtil.IsValidNumber(phoneNumber);
                #endregion

                #region Valid Otp
                var otpCodeFromCache = await _cache.GetRecordAsync<string>($"otp-{request.UserInfoRegister.PhoneNumber}");
                if (string.IsNullOrEmpty(otpCodeFromCache) || long.Parse(otpCodeFromCache) != request.OtpCode)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC54C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC54C), nameof(EnumUserErrorCodes.USRC54C) ?? "") }
                    );
                    methodResult.Result = null;
                    return methodResult;
                }
                #endregion

                #region Register account
                CreateUserCommand createUserInfo = new();
                createUserInfo = _mapper.Map<CreateUserCommand>(request.UserInfoRegister);
                createUserInfo.PasswordHash = request.PasswordHash;
                var registerAccountResult = await _mediator.Send(createUserInfo).ConfigureAwait(false);
                if (createUserInfo is null || !registerAccountResult.IsOK)
                    throw new Exception("Error server! Please re-register!");
                #endregion

                #region Login account
                AuthUserCommand allowLogin = new()
                {
                    Username = createUserInfo.UserName ?? string.Empty,
                    Password = createUserInfo.PasswordHash ?? string.Empty
                };
                var loginInfo = await _mediator.Send(allowLogin).ConfigureAwait(false);
                if (loginInfo is null || !loginInfo.IsOK)
                    throw new Exception("Register success! Please login again");
                #endregion
                methodResult.Result = loginInfo.Result;
                return methodResult;
            }
            catch (NumberParseException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(ValidOtpToWhatsAppCommand) STEP NumberParseException --> Message {ex} ---->");
                methodResult.AddResultFromErrorList(new List<ErrorResult>
                { new ErrorResult
                    {
                        ErrorCode ="400",
                        ErrorMessage = ex.Message
                    }
                });
                methodResult.Result = null;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(ValidOtpToWhatsAppCommand) STEP EXEPTION MESSAGE --> Message {ex} ---->");
                _logger.LogError($" -->(ValidOtpToWhatsAppCommand) STEP EXEPTION STACK --> Message {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = null;
                return methodResult;
            }
        }
    }
}
