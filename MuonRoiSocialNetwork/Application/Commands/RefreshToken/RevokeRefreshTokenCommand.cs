using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Token;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Auth;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.RefreshToken
{
    /// <summary>
    /// Request revoke refresh token
    /// </summary>
    public class RevokeRefreshTokenCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// UserId create refresh token
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Especial for re-new refresh token
        /// </summary>
        public bool IsUpdateAccountStatus { get; set; }
    }
    /// <summary>
    /// Class handler
    /// </summary>
    public class RevokeRefreshTokenCommandHandler : BaseUserCommandHandler, IRequestHandler<RevokeRefreshTokenCommand, MethodResult<bool>>
    {
        private readonly IRefreshtokenRepository _refreshtokenRepository;
        private readonly IRefreshTokenQueries _refreshtokenQueries;
        private readonly ILogger<RevokeRefreshTokenCommandHandler> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userQueries"></param>
        /// <param name="userRepository"></param>
        /// <param name="refreshtokenRepository"></param>
        /// <param name="logger"></param>
        /// <param name="authContext"></param>
        /// <param name="refreshTokenQueries"></param>
        public RevokeRefreshTokenCommandHandler(IMapper mapper, IConfiguration configuration, IUserQueries userQueries, IUserRepository userRepository, IRefreshtokenRepository refreshtokenRepository,
            ILoggerFactory logger, AuthContext authContext,
            IRefreshTokenQueries refreshTokenQueries) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _logger = logger.CreateLogger<RevokeRefreshTokenCommandHandler>();
            _refreshtokenRepository = refreshtokenRepository;
            _refreshtokenQueries = refreshTokenQueries;
        }

        /// <summary>
        /// Func handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), EnumUserErrorCodes.USR02C) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #region Check is exist User
                _authContext.CurrentUserId = string.IsNullOrEmpty(_authContext.CurrentUserId) ? request.UserId.ToString() : _authContext.CurrentUserId;
                var checkUser = await _userQueries.GetByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
                if (checkUser.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status205ResetContent;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), Guid.Parse(_authContext.CurrentUserId)) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Revoke refresh token
                var allRefreshToken = await _refreshtokenQueries.GetAllAsync();
                var refreshTokenInfo = allRefreshToken.FirstOrDefault(x => x.UserId == Guid.Parse(_authContext.CurrentUserId));
                if (refreshTokenInfo != null)
                {
                    await _refreshtokenRepository.ExecuteTransactionAsync(async () =>
                    {
                        refreshTokenInfo.RefreshTokenExpiryTimeTS = DateTime.Now.GetTimeStamp();
                        _refreshtokenRepository.Update(refreshTokenInfo);
                        await _refreshtokenRepository.UnitOfWork.SaveEntitiesAsync();
                        #region Set status user is off
                        if (request.IsUpdateAccountStatus)
                        {
                            checkUser.Result.AccountStatus = EnumAccountStatus.IsOf;
                            await _userRepository.UpdateUserAsync(checkUser.Result);
                        }
                        #endregion
                        methodResult.Result = true;
                        return methodResult;
                    });
                }
                #endregion
                methodResult.Result = true;
                return methodResult;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(RevokeRefreshToken) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(RevokeRefreshToken) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
        }
    }
}
