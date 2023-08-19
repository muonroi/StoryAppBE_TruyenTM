using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Extentions.String;
using BaseConfig.Infrashtructure;
using BaseConfig.JWT;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Settings.RefreshTokenSettings;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Token;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.RefreshToken
{
    /// <summary>
    /// Request renew access token
    /// </summary>
    public class RenewAccessTokenCommand : IRequest<MethodResult<string>>
    {
        /// <summary>
        /// RefreshToken request
        /// </summary>
        [JsonProperty("refresh-token")]
        public string? RefreshToken { get; set; }
    }
    /// <summary>
    /// Class handler
    /// </summary>
    public class RenewAccessTokenCommandHandler : BaseUserCommandHandler, IRequestHandler<RenewAccessTokenCommand, MethodResult<string>>
    {
        private readonly IRefreshtokenRepository _refreshtokenRepository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<RenewAccessTokenCommandHandler> _logger;
        private readonly IRoleQueries _roleQueries;
        private readonly IGroupQueries _groupQueries;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="userQueries"></param>
        /// <param name="userRepository"></param>
        /// <param name="refreshtokenRepository"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        /// <param name="authContext"></param>
        /// <param name="roleQueries"></param>
        /// <param name="groupQueries"></param>
        public RenewAccessTokenCommandHandler(IMapper mapper, IConfiguration configuration, IUserQueries userQueries, IUserRepository userRepository, IRefreshtokenRepository refreshtokenRepository, IDistributedCache cache,
            ILoggerFactory logger, AuthContext authContext, IRoleQueries roleQueries, IGroupQueries groupQueries) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _refreshtokenRepository = refreshtokenRepository;
            _cache = cache;
            _logger = logger.CreateLogger<RenewAccessTokenCommandHandler>();
            _roleQueries = roleQueries;
            _groupQueries = groupQueries;
        }

        /// <summary>
        /// Func handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<string>> Handle(RenewAccessTokenCommand request, CancellationToken cancellationToken)
        {
            MethodResult<string> methodResult = new();
            GenarateJwtToken genarateJwtToken = new(_configuration);
            string accessToken = string.Empty;
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), EnumUserErrorCodes.USR02C) }
                    );
                    return methodResult;
                }
                #region Check is exist User
                var checkUser = await _userQueries.GetByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
                if (checkUser.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), Guid.Parse(_authContext.CurrentUserId)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Get info refresh token

                Dictionary<string, string[]> keyValuePairs = await _refreshtokenRepository.GetInfoRefreshTokenAsync(Guid.Parse(_authContext.CurrentUserId));
                if (keyValuePairs.Any(x => x.Key == "false"))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC44C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), Guid.Parse(_authContext.CurrentUserId)) }
                    );
                    return methodResult;
                }
                string[] valueRefreshToken = keyValuePairs.FirstOrDefault(x => x.Key == _authContext.CurrentUserId).Value;
                if (valueRefreshToken is null || valueRefreshToken.Length == 0)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC44C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), Guid.Parse(_authContext.CurrentUserId)) }
                    );
                    return methodResult;
                }
                string salt = valueRefreshToken.Length <= 0 ? "" : valueRefreshToken[0];
                string refreshToken = valueRefreshToken.Length <= 0 ? "" : valueRefreshToken[1];
                double timeExpired = valueRefreshToken.Length <= 0 ? 0 : Convert.ToDouble(valueRefreshToken[2]);
                if (DateTime.UtcNow >= DateTimeExtensions.TimeStampToDateTime(timeExpired))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC36C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUserId), Guid.Parse(_authContext.CurrentUserId)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Check valid refresh token
#pragma warning disable CS8604
                if (refreshToken != StringManagers.DecodeFrom64(request.RefreshToken))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC45C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.RefreshToken), request.RefreshToken) }
                    );
                    return methodResult;
                }
#pragma warning restore CS8604
                #endregion

                #region Get cache
                UserModelResponse? resultInforLoginUser = await _cache.GetRecordAsync<UserModelResponse>($"{RefreshTokenDefault.Instance.keyUserModelResponseRegister}_{_authContext.CurrentUserId}");
                MethodResult<BaseUserResponse> userResponse = new();
                if (resultInforLoginUser is null)
                {
                    userResponse = await _userQueries.GetUserModelByGuidAsync(Guid.Parse(_authContext.CurrentUserId));
                }
                resultInforLoginUser = _mapper.Map<UserModelResponse>(userResponse.Result);
                #endregion

                #region Renew access token
                var roleByGroupId = await _groupQueries.GetByIdAsync(_authContext.GroupId);
                var roleIds = roleByGroupId.Roles.Split(',').ToList();
                var rolesList = new List<string>();
                foreach (var roleId in roleIds)
                {
                    var tempRole = await _roleQueries.GetRoleByIdAsync(int.Parse(roleId));
                    rolesList.Add(tempRole.Result is null ? string.Empty : tempRole.Result.Name);
                }
                accessToken = genarateJwtToken.GenarateJwt(resultInforLoginUser, SettingUserDefault.Instance.minuteExpitryLogin, rolesList);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(RenewAccessToken) STEP EXEPTION MESSAGE --> ID USER {ex} ---->");
                _logger.LogError($" -->(RenewAccessToken) STEP EXEPTION STACK --> ID USER {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
            }
            methodResult.Result = accessToken;
            return methodResult;
        }
    }
}
