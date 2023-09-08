using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuonRoiSocialNetwork.Application.Commands.GroupAndRoles;
using MuonRoiSocialNetwork.Common.Models.GroupAndRoles.Base.Response;
using MuonRoiSocialNetwork.Common.Models.Logs;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;
namespace MuonRoiSocialNetwork.Controllers.Accounts
{
    /// <summary>
    /// Auth: PhiLe 202303028
    /// </summary>
    [ApiVersion(MainSettings.APIVersion)]
    [Route("api/v{version:apiVersion}/groups")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupAndRoleController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthContext _auth;
        private readonly IMediator _mediator;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        public GroupAndRoleController(IMediator mediator, AuthContext auth, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        #region Repository
        /// <summary>
        /// Initial group API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<GroupInitialBaseResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GroupInit([FromBody] InitialGroupCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<GroupInitialBaseResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(GroupAndRoleController),
                    ApiName = nameof(GroupInit),
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
        /// Initial role API
        /// </summary>
        /// <returns></returns>
        [HttpPost("role")]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<RoleInitialBaseResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RoleInit([FromBody] InitialRoleCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<RoleInitialBaseResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(GroupAndRoleController),
                    ApiName = nameof(RoleInit),
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
        /// Assign role to group
        /// </summary>
        /// <returns></returns>
        [HttpPatch("group/role")]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AssignRoleToGroup([FromBody] List<string> roleId, int groupId)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                AssignRoleCommand cmd = new()
                {
                    RoleId = roleId,
                    GroupId = groupId
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(GroupAndRoleController),
                    ApiName = nameof(AssignRoleToGroup),
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
    }
}
