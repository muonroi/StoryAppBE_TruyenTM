using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Extentions.Datetime;
using BaseConfig.JWT;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Users;
using MuonRoiSocialNetwork.Application.Commands.RefreshToken;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Settings.RefreshTokenSettings;
using MuonRoiSocialNetwork.Common.Settings.UserSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Users;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using Newtonsoft.Json;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using MuonRoiSocialNetwork.Common.Models.Users;
using Hangfire;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Users;
using Serilog.Core;
using Serilog;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;
using BaseConfig.Infrashtructure;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Auth;
using BaseConfig.Extentions.String;
using BaseConfig.Extentions.Image;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;

namespace MuonRoiSocialNetwork.Application.Commands.Users
{
    /// <summary>
    /// Login auth user command
    /// </summary>
    public class AuthUserCommand : IRequest<MethodResult<UserModelResponse>>
    {
        #region Property
        /// <summary>
        /// Username login
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Password Login
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
        #endregion
    }
    /// <summary>
    /// Handler login user
    /// </summary>
    public class AuthUserCommandHandler : BaseUserCommandHandler, IRequestHandler<AuthUserCommand, MethodResult<UserModelResponse>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthUserCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;
        private readonly IRoleQueries _roleQueries;
        private readonly IGroupQueries _groupQueries;
        private readonly IRefreshTokenQueries _refreshTokenQueries;
        private readonly IStoryNotificationRepository _storyNotificationsRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="userQueries"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="cache"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="roleQueries"></param>
        /// <param name="groupQueries"></param>
        /// <param name="authContext"></param>
        /// <param name="refreshTokenQueries"></param>
        /// <param name="storyNotificationsRepository"></param>

        public AuthUserCommandHandler(IMapper mapper,
            IUserRepository userRepository,
            IUserQueries userQueries,
            IConfiguration configuration,
            ILoggerFactory logger,
            IMediator mediator, IDistributedCache cache, IHttpContextAccessor httpContextAccessor, IRoleQueries roleQueries, IGroupQueries groupQueries, AuthContext authContext,
            IRefreshTokenQueries refreshTokenQueries, IStoryNotificationRepository storyNotificationsRepository) : base(mapper, configuration, userQueries, userRepository, authContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger.CreateLogger<AuthUserCommandHandler>();
            _mediator = mediator;
            _cache = cache;
            _roleQueries = roleQueries;
            _groupQueries = groupQueries;
            _refreshTokenQueries = refreshTokenQueries;
            _storyNotificationsRepository = storyNotificationsRepository;
        }
        /// <summary>
        /// Handler function
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<UserModelResponse>> Handle(AuthUserCommand request, CancellationToken cancellationToken)
        {
            MethodResult<UserModelResponse> methodResult = new();
            GenarateJwtToken genarateJwtToken = new(_configuration);
            MethodResult<string> refreshToken = new();
            try
            {

                #region Check valid username and password
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(nameof(EnumUserErrorCodes.USRC49C),
                       new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC49C), EnumUserErrorCodes.USRC49C) }
                   );
                    return methodResult;
                }
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    _logger.LogError($" --> STEP CHECK {"Check valid username and password".ToUpper()} --> USERNAME: {request.Username} | PASSWORD: {request.Password} -->");
                    methodResult.AddApiErrorMessage(
                        string.IsNullOrEmpty(request.Username) ? nameof(EnumUserErrorCodes.USR05C) : nameof(EnumUserErrorCodes.USR06C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.Username), request.Username ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Check user is exsit ?
                var isUserExistResult = await _userQueries.GetByUsernameAsync(request.Username);
                if (!isUserExistResult.IsOK || isUserExistResult.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR02C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.Username), request.Username ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Check user have been locked
                if (isUserExistResult.Result.Status == EnumAccountStatus.Locked)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR28C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.Username), request.Username ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Check password user
                string password = HashPassword(request.Password, isUserExistResult.Result.Salt ?? "");
                if (password != isUserExistResult.Result.PasswordHash)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC39C),
                        new[] { Helpers.GenerateErrorResult(nameof(SettingUserDefault.Instance.loginAttempDefault), SettingUserDefault.Instance.loginAttempDefault - isUserExistResult.Result.AccessFailedCount) }
                    );
                    isUserExistResult.Result.AccessFailedCount++;
                    if (isUserExistResult.Result.AccessFailedCount >= SettingUserDefault.Instance.loginAttempDefault)
                    {
                        isUserExistResult.Result.Status = EnumAccountStatus.Locked;
                        isUserExistResult.Result.LockReason = $"Login failed {isUserExistResult.Result.AccessFailedCount} times";
                    }
                    await _userRepository.UpdateUserAsync(isUserExistResult.Result);
                    return methodResult;
                }
                #endregion

                #region Check user is renew password
                if (isUserExistResult.Result.AccountStatus == EnumAccountStatus.IsRenew)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC43C),
                        new[] { Helpers.GenerateErrorResult(nameof(request.Username), request.Username ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Update info user when login success
                isUserExistResult.Result.AccessFailedCount = 0;
                isUserExistResult.Result.LastLogin = DateTime.UtcNow;
                isUserExistResult.Result.AccountStatus = EnumAccountStatus.IsOnl;
                isUserExistResult.Result.Status = EnumAccountStatus.Active;
                var jobId = BackgroundJob.Enqueue<UserRepository>(x => x.UpdateUserAsync(isUserExistResult.Result, null, null));
                RecurringJob.TriggerJob(jobId);
                #endregion

                #region Get all role of user
                var roleByGroupId = await _groupQueries.GetByIdAsync(isUserExistResult.Result.GroupId.Value);
                var roleIds = roleByGroupId.Roles.Split(',').ToList();
                var rolesList = new List<string>();
                foreach (var roleId in roleIds)
                {
                    var tempRole = await _roleQueries.GetRoleByIdAsync(int.Parse(roleId));
                    rolesList.Add(tempRole.Result is null ? string.Empty : tempRole.Result.Name);
                }
                #endregion

                #region Return info user was login
                UserModelResponse resultInforLoginUser = _mapper.Map<UserModelResponse>(isUserExistResult.Result);

                resultInforLoginUser.CreateDate = DateTimeExtensions.TimeStampToDateTime(isUserExistResult.Result.CreatedDateTS.GetValueOrDefault()).AddHours(SettingUserDefault.Instance.hourAsia);
                resultInforLoginUser.UpdateDate = DateTimeExtensions.TimeStampToDateTime(isUserExistResult.Result.UpdatedDateTS.GetValueOrDefault()).AddHours(SettingUserDefault.Instance.hourAsia);

                MethodResult<BaseUserResponse> userInfo = await _userQueries.GetUserModelByGuidAsync(resultInforLoginUser.Id);
                resultInforLoginUser.RoleName = string.Join(",", rolesList);
                resultInforLoginUser.GroupName = userInfo.Result?.GroupName;
                resultInforLoginUser.JwtToken = genarateJwtToken.GenarateJwt(resultInforLoginUser, SettingUserDefault.Instance.minuteExpitryLogin, rolesList);
                #endregion

                #region Check -> genarate refresh token and set cache
                UserModelResponse? userGet = await _cache.GetRecordAsync<UserModelResponse>($"{RefreshTokenDefault.Instance.keyUserModelResponseLogin}_{resultInforLoginUser.Id}");
                if (userGet is null)
                {
                    TimeSpan expirationTime = RefreshTokenDefault.Instance.expirationTimeLogin;
                    TimeSpan slidingExpirationTime = RefreshTokenDefault.Instance.slidingExpirationLogin;
                    GennerateRefreshTokenCommand gennerateRefreshToken = new()
                    {
                        UserId = resultInforLoginUser.Id,
                    };
                    refreshToken = await _mediator.Send(gennerateRefreshToken, cancellationToken).ConfigureAwait(false);
                    if (!refreshToken.IsOK)
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumUserErrorCodes.USR29C),
                            new[] { Helpers.GenerateErrorResult(nameof(request.Username), request.Username ?? "") }
                        );
                        return methodResult;
                    }
                    resultInforLoginUser.RefreshToken = refreshToken.Result;
                    await _cache.SetRecordAsync($"{RefreshTokenDefault.Instance.keyUserModelResponseLogin}_{resultInforLoginUser.Id}", resultInforLoginUser, expirationTime, slidingExpirationTime);
                }
                #endregion

                #region Get and set location user login
                string dataLocationOfUser = GetLocationUser();
                if (!string.IsNullOrEmpty(dataLocationOfUser))
                {
                    isUserExistResult.Result.LastLoginLocation = JsonConvert.SerializeObject(dataLocationOfUser);
                    await _userRepository.UpdateUserAsync(isUserExistResult.Result);
                }
                #endregion
                var allRefreshToken = await _refreshTokenQueries.GetAllAsync();
                var refreshTokenFormat = refreshToken.Result ?? StringManagers.EncodeTo64(allRefreshToken.FirstOrDefault(x => x.UserId == resultInforLoginUser.Id)?.RefreshToken ?? string.Empty);
                resultInforLoginUser.RefreshToken = userGet?.RefreshToken ?? refreshTokenFormat;
                resultInforLoginUser.LocationUserLogin = dataLocationOfUser != null ? JsonConvert.DeserializeObject<LocationUserLogin>(isUserExistResult.Result.LastLoginLocation ?? string.Empty) : null;
                resultInforLoginUser.Avatar = HandlerImages.TakeLinkImage(_configuration, resultInforLoginUser.Avatar ?? "");
                var notificationNumber = await _storyNotificationsRepository.GetWhereAsync(x => x.UserGuid == resultInforLoginUser.Id);
                resultInforLoginUser.NotificationNumber = notificationNumber != null ? notificationNumber.Count() : 0;
                methodResult.Result = resultInforLoginUser;
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
            }
            catch (Exception ex)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(AUTH) STEP {"Exception".ToUpper()} --> EXEPTION: {ex} ---->");
            }
            return methodResult;
        }
        private string GetLocationUser()
        {
            try
            {
                string? locationData = null;
                using var reader = new DatabaseReader("certificates/muonroi_user.mmdb");
                if (!(_httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() == "::1"))
                {
                    CityResponse response = reader.City(_httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1");
                    if (response != null)
                    {

                        {
                            LocationUserLogin infoUser = new()
                            {
                                CountryName = response.Country?.Name ?? string.Empty,
                                CityName = response.City?.Name ?? string.Empty,
                                Latitude = response.Location?.Latitude ?? 0,
                                Longitude = response.Location?.Longitude ?? 0,
                                TimeZone = response.Location?.TimeZone ?? string.Empty
                            };
                            locationData = JsonConvert.SerializeObject(infoUser);
                        };
                    }
                }
                return locationData;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return null;
            }

        }
    }
}
