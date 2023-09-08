using BaseConfig.EntityObject.Entity;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MuonRoiSocialNetwork.Application.Commands.Users;
using MuonRoiSocialNetwork.Application.Commands.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MuonRoiSocialNetwork.Common.Models.Users.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using BaseConfig.Infrashtructure;
using System.Diagnostics;
using Newtonsoft.Json;
using Serilog;
using MuonRoiSocialNetwork.Common.Models.Logs;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;

namespace MuonRoiSocialNetwork.Controllers.Accounts
{
    /// <summary>
    /// Auth: PhiLe 20230305
    /// </summary>
    [ApiVersion(MainSettings.APIVersion)]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthContext _auth;
        private readonly IMediator _mediator;
        private readonly IUserQueries _userQueries;

        /// <summary>
        /// Constructure
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userQueries"></param>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        public UserController(IMediator mediator, IUserQueries userQueries, AuthContext auth, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _userQueries = userQueries;
        }
        #region Repository
        /// <summary>
        /// Register new user API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<UserModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<UserModelResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(RegisterUser),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Login API
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<UserModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginAuth([FromBody] AuthUserCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<UserModelResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(LoginAuth),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Verification email API
        /// </summary>
        /// <returns></returns>
        [HttpPatch("active")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> VerificationEmail([FromBody] VerificationEmailCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(VerificationEmail),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Update informations for user API
        /// </summary>
        /// <returns></returns>
        [HttpPut("profile")]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<BaseUserResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateInformation([FromBody] UpdateInformationCommand userChange)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<BaseUserResponse> methodResult = await _mediator.Send(userChange).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(UpdateInformation),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary> 
        /// Delete user by Guid API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpDelete]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUserByGuid([FromQuery] Guid userGuid)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                DeleteUserCommand cmd = new()
                {
                    UserGuid = userGuid
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(DeleteUserByGuid),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// User forgot password API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FotgotPassword([FromBody] ForgotPasswordUserCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(FotgotPassword),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// User change password API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpPatch("change-passoword")]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePasswordForgot([FromBody] ChangePasswordCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(ChangePasswordForgot),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Change status account ( lock | active ) API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpPatch("status")]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeStatusAccount([FromBody] ChangeStatusCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(ChangeStatusAccount),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Resend veritifycation mail API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpPost("resend-mail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResendMailVeritification([FromBody] ResendMailVeritificationCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(ResendMailVeritification),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Assign user to group
        /// </summary>
        /// <returns></returns>
        [HttpPatch("group")]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignUserToGroup([FromQuery] int groupId)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                AssignUserToGroupCommand cmd = new()
                {
                    GroupId = groupId
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(AssignUserToGroup),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Send otp to whatsapp
        /// </summary>
        /// <returns></returns>
        [HttpPost("send-otp")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<long>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendOtpToWhatsApp([FromBody] SendOtpToWhatsAppCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<long> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(SendOtpToWhatsApp),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Valid otp to whatsapp
        /// </summary>
        /// <returns></returns>
        [HttpPost("valid-otp")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<UserModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ValidOtpToWhatsApp([FromBody] ValidOtpToWhatsAppCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<UserModelResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(UserController),
                    ApiName = nameof(ValidOtpToWhatsApp),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = methodResult.StatusCode ?? 0,
                    ErrorMessages = methodResult.ErrorMessages.FirstOrDefault()?.ErrorMessage ?? string.Empty,
                    CreatedDate = DateTime.UtcNow
                };
                Log.Information($"{JsonConvert.SerializeObject(logsInfo)}");
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        #endregion

        #region Queries
        /// <summary> 
        /// Get user by username API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpGet("single/username")]
        [Authorize(Policy = nameof(RoleSettings.VIEWER))]
        [ProducesResponseType(typeof(MethodResult<BaseUserResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserByUserName([FromQuery] string username)
        {
            try
            {
                MethodResult<BaseUserResponse> methodResult = await _userQueries.GetUserModelBynameAsync(username).ConfigureAwait(false);
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Get user by Guid API
        /// </summary>
        /// <returns>UserModel</returns>
        [HttpGet("single/id")]
        [Authorize(Policy = nameof(RoleSettings.VIEWER))]
        [ProducesResponseType(typeof(MethodResult<BaseUserResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserByGuid([FromQuery] Guid uid)
        {
            try
            {
                MethodResult<BaseUserResponse> methodResult = await _userQueries.GetUserModelByGuidAsync(uid).ConfigureAwait(false);
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                LogsError logsError = new()
                {
                    FullInfo = ex.ToString(),
                    MessageShort = ex.Message
                };
                Log.Error($"{JsonConvert.SerializeObject(logsError)}");
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        #endregion
    }
}