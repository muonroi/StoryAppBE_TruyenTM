using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoiSocialNetwork.Common.Models.Logs;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;

namespace MuonRoiSocialNetwork.Controllers.Cache
{
    /// <summary>
    /// Auth: PhiLe 20230609
    /// </summary>
    [ApiVersion(MainSettings.APIVersion)]
    [Route("api/v{version:apiVersion}/caches")]
    [Authorize(Policy = nameof(RoleSettings.SU))]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthContext _auth;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        public CacheController(IDistributedCache cache, AuthContext auth, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }
        /// <summary>
        /// Clear all cache
        /// </summary>
        [HttpPost]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ClearCache()
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await RedisCachingManager.ClearApplicationCache();
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(CacheController),
                    ApiName = nameof(ClearCache),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = 0,
                    ErrorMessages = string.Empty,
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
        /// Get all cache
        /// </summary>
        [HttpGet("all")]
        [Authorize(Policy = nameof(RoleSettings.SU))]
        [ProducesResponseType(typeof(MethodResult<List<string>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCache()
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<List<string>> methodResult = await RedisCachingManager.GetApplicationCache();
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(CacheController),
                    ApiName = nameof(GetCache),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
                    DurationTime = stopwatch.ElapsedMilliseconds,
                    Browser = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                    StatusCode = 0,
                    ErrorMessages = string.Empty,
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
    }
}
