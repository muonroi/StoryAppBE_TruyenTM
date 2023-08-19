using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Extentions.String;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.DomainObjects.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Token;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Auth;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.RefreshToken
{
    /// <summary>
    /// Request create refresh token
    /// </summary>
    public class GennerateRefreshTokenCommand : IRequest<MethodResult<string>>
    {
        /// <summary>
        /// UserId create refresh token
        /// </summary>
        public Guid UserId { get; set; }
    }
    /// <summary>
    /// Class handler
    /// </summary>
    public class GennerateRefreshTokenCommandHandler : BaseUserCommandHandler, IRequestHandler<GennerateRefreshTokenCommand, MethodResult<string>>
    {
        private readonly IRefreshtokenRepository _refreshtokenRepository;
        private readonly IRefreshTokenQueries _refreshTokenQueries;
        private readonly ILogger<GennerateRefreshTokenCommandHandler> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userQueries"></param>
        /// <param name="userRepository"></param>
        /// <param name="refreshtokenRepository"></param>
        /// <param name="logger"></param>
        /// <param name="refreshTokenQueries"></param>
        /// <param name="authContext"></param>
        public GennerateRefreshTokenCommandHandler(IMapper mapper, IConfiguration configuration, IUserQueries userQueries, IUserRepository userRepository, IRefreshtokenRepository refreshtokenRepository,
            ILoggerFactory logger, AuthContext authContext, IRefreshTokenQueries refreshTokenQueries) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _refreshtokenRepository = refreshtokenRepository;
            _refreshTokenQueries = refreshTokenQueries;
            _logger = logger.CreateLogger<GennerateRefreshTokenCommandHandler>();
        }

        /// <summary>
        /// Func handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<string>> Handle(GennerateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            MethodResult<string> methodResult = new();
            string genareRefreshToken = string.Empty;
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.UserId), EnumUserErrorCodes.USR02C) }
                    );
                    return methodResult;
                }
                #region Check is exist User
                var checkUser = await _userQueries.GetByGuidAsync(request.UserId);
                if (checkUser.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.UserId), request.UserId) }
                    );
                    return methodResult;
                }
                #endregion

                #region Get info refresh token
                Dictionary<string, string[]> keyValuePairs = await _refreshtokenRepository.GetInfoRefreshTokenAsync(request.UserId);
                if (keyValuePairs.Any(x => x.Key == "false"))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC44C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.UserId), request.UserId) }
                    );
                    return methodResult;
                }
                if (keyValuePairs.Any(x => x.Key == "FirstLogin"))
                {
                    #region genarate refresh token
                    genareRefreshToken = await GenarateRefreshTokenAysnc(request.UserId);
                    #endregion
                    methodResult.StatusCode = StatusCodes.Status200OK;
                    methodResult.Result = genareRefreshToken;
                    return methodResult;
                }
                string[] valueRefreshToken = keyValuePairs.FirstOrDefault(x => x.Key == request.UserId.ToString()).Value;
                string refreshToken = valueRefreshToken.Length <= 0 ? "" : valueRefreshToken[1];
                double timeExpired = valueRefreshToken.Length <= 0 ? 0 : Convert.ToDouble(string.IsNullOrEmpty(valueRefreshToken[2]) ? 0 : valueRefreshToken[2]);
                if (DateTime.UtcNow >= DateTimeExtensions.TimeStampToDateTime(timeExpired))
                {
                    #region genarate refresh token
                    var allRefreshToken = await _refreshTokenQueries.GetAllAsync();
                    var refreshTokenInfo = allRefreshToken.FirstOrDefault(x => x.UserId == request.UserId);
                    if (refreshTokenInfo != null)
                    {
                        await _refreshtokenRepository.ExecuteTransactionAsync(async () =>
                        {
                            refreshTokenInfo.RefreshTokenExpiryTimeTS = DateTime.UtcNow.AddDays(7).GetTimeStamp();
                            _refreshtokenRepository.Update(refreshTokenInfo);
                            await _refreshtokenRepository.UnitOfWork.SaveEntitiesAsync();
                            return methodResult;
                        });
                    }
                    #endregion
                }
                #endregion

                #region Check refresh token is exist
                if (refreshToken is not null)
                {
                    methodResult.StatusCode = StatusCodes.Status200OK;
                    methodResult.Result = null;
                    return methodResult;
                }
                #endregion

                #region genarate refresh token
                genareRefreshToken = await GenarateRefreshTokenAysnc(request.UserId);
                #endregion

                methodResult.StatusCode = StatusCodes.Status200OK;
                methodResult.Result = genareRefreshToken;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(GennerateRefreshToken) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(GennerateRefreshToken) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
        }
        private async Task<string> GenarateRefreshTokenAysnc(Guid userId)
        {
            UserLogin user = new();
            var genareRefreshToken = RandomString(SettingUserDefault.Instance.genareRefreshToken);
            user.KeySalt = GenarateSalt();
            user.RefreshToken = genareRefreshToken;
            user.UserId = userId;
            user.RefreshTokenExpiryTimeTS = DateTime.UtcNow.AddDays(7).GetTimeStamp();
            await _refreshtokenRepository.ExecuteTransactionAsync(async () =>
                {
                    _refreshtokenRepository.Add(user);
                    await _refreshtokenRepository.UnitOfWork.SaveChangesAsync();
                    return new MethodResult<string>();
                });
            return StringManagers.EncodeTo64(genareRefreshToken);
        }
    }
}
