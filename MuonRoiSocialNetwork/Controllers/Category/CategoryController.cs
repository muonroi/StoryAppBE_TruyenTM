using BaseConfig.BaseDbContext.Common;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuonRoiSocialNetwork.Application.Commands.Category;
using MuonRoiSocialNetwork.Common.Models.Category.Request;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using MuonRoiSocialNetwork.Common.Models.Logs;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;

namespace MuonRoiSocialNetwork.Controllers.Category
{
    /// <summary>
    /// Auth: PhiLe 20230531
    /// </summary>
    [ApiVersion(MainSettings.APIVersion)]
    [Route("api/v{version:apiVersion}/categories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthContext _auth;
        private readonly IMediator _mediator;
        private readonly ICategoryQueries _categoryQueries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="categoryQueries"></param>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        public CategoryController(IMediator mediator, ICategoryQueries categoryQueries, AuthContext auth, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _categoryQueries = categoryQueries;
        }
        #region Repository
        /// <summary>
        /// Create new category API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<List<CategoryResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<List<CategoryResponse>> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(CategoryController),
                    ApiName = nameof(CreateCategory),
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
        /// Update info category API
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<CategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest categoryRequest, [FromQuery] int idCategory)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                UpdateCategoryCommand cmd = new()
                {
                    IdCategory = idCategory,
                    IsActive = categoryRequest.IsActive,
                    IconName = categoryRequest.IconName,
                    NameCategory = categoryRequest.NameCategory
                };
                MethodResult<CategoryResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(CategoryController),
                    ApiName = nameof(UpdateCategory),
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
        /// Delete category API
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveCategory([FromBody] RemoveCategoryCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(CategoryController),
                    ApiName = nameof(RemoveCategory),
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
        /// Get all category API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<PagingItemsDTO<CategoryResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCategories([FromQuery] int pageSize = 1, int pageIndex = 10)
        {
            try
            {
                MethodResult<PagingItemsDTO<CategoryResponse>> methodResult = await _categoryQueries.GetAllCategory(pageSize, pageIndex);
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
