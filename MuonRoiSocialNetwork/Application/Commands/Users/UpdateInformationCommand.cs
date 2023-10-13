using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Extentions.Image;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Request;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Users
{
    /// <summary>
    /// Update user command
    /// </summary>
    public class UpdateInformationCommand : BaseUserRequest, IRequest<MethodResult<BaseUserResponse>>
    {

        [JsonProperty("newSalf")]
        public string? NewSalf { get; set; } = null;

        [JsonProperty("newPassword")]
        public string? NewPassword { get; set; } = null;

        [JsonProperty("accountStatus")]
        public EnumAccountStatus AccountStatus { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }
    }
    /// <summary>
    /// Handler update infor user
    /// </summary>
    public class UpdateInformationCommandHandler : BaseUserCommandHandler, IRequestHandler<UpdateInformationCommand, MethodResult<BaseUserResponse>>
    {
        private readonly ILogger<UpdateInformationCommandHandler> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userRepository"></param>
        /// <param name="userQueries"></param>
        /// <param name="logger"></param>
        /// <param name="authContext"></param>
        public UpdateInformationCommandHandler(IMapper mapper,
            IConfiguration configuration, IUserRepository userRepository, IUserQueries userQueries, ILoggerFactory logger, AuthContext authContext) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _logger = logger.CreateLogger<UpdateInformationCommandHandler>();
        }
        /// <summary>
        /// Function Handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<BaseUserResponse>> Handle(UpdateInformationCommand request, CancellationToken cancellationToken)
        {
            MethodResult<BaseUserResponse> methodResult = new();
            try
            {
                #region Check is exist user
                var userIsExist = await _userQueries.GetByUsernameAsync(_authContext.CurrentUsername ?? "");
                if (!userIsExist.IsOK || userIsExist.Result is null || request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Check is update email
                if (_authContext.Email != userIsExist.Result.Email)
                {
                    var existingUser = await _userQueries
                    .GetUserByEmailAsync(request.Email ?? "")
                    .ConfigureAwait(false);
                    if (existingUser.Result is not null)
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumUserErrorCodes.USRC34C),
                            new[] { Helpers.GenerateErrorResult(nameof(_authContext.Email), _authContext.Email ?? "") }
                        );
                        return methodResult;
                    }
                }
                #endregion

                #region Update info user
                var userInfoUpdate = userIsExist.Result;
                _mapper.Map(request, userInfoUpdate);
                userInfoUpdate.Status = request.AccountStatus == EnumAccountStatus.None ? request.AccountStatus : userInfoUpdate.AccountStatus;
                userInfoUpdate.LockReason = request.Reason ?? userInfoUpdate.LockReason;
                await _userRepository.UpdateUserAsync(userInfoUpdate, request.NewSalf, request.NewPassword);
                #endregion

                #region Return info user updated
                userInfoUpdate.Avatar = HandlerImages.TakeLinkImage(_configuration, userInfoUpdate.Avatar ?? "");
                BaseUserResponse resultInforLoginUser = _mapper.Map<BaseUserResponse>(userInfoUpdate);
                methodResult.Result = resultInforLoginUser;
                methodResult.StatusCode = StatusCodes.Status200OK;
                #endregion

            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(UPDATE INFO) STEP CHECK {"CustomException".ToUpper()} --> EXEPTION: {ex}");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(UPDATE INFO) STEP CHECK {"Exception".ToUpper()} --> EXEPTION: {ex}");
                _logger.LogError($" -->(UPDATE INFO) STEP CHECK {"Exception".ToUpper()} --> EXEPTION{" StackTrace".ToUpper()}: {ex.StackTrace}");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
            }
            return methodResult;
        }

    }
}
