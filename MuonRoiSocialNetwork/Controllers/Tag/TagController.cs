using BaseConfig.BaseDbContext.Common;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuonRoiSocialNetwork.Application.Commands.Tags;
using MuonRoiSocialNetwork.Common.Models.Logs;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Request;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Response;
using MuonRoiSocialNetwork.Common.Models.Tags.Request;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;

namespace MuonRoiSocialNetwork.Controllers.Tag
{
    /// <summary>
    /// Auth: PhiLe 20230531
    /// </summary>
    [ApiVersion(MainSettings.APIVersion)]
    [Route("api/v{version:apiVersion}/tags")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthContext _auth;
        private readonly IMediator _mediator;
        private readonly ITagQueries _tagQueries;
        private readonly ITagInStoriesQueries _tagInStoriesQueries;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="tagQueries"></param>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="tagInStoriesQueries"></param>
        public TagController(IMediator mediator, ITagQueries tagQueries, AuthContext auth, IHttpContextAccessor httpContextAccessor, ITagInStoriesQueries tagInStoriesQueries)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _tagQueries = tagQueries;
            _tagInStoriesQueries = tagInStoriesQueries;
        }
        #region Repository
        /// <summary>
        /// Create new tag API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<List<TagModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<List<TagModelResponse>> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(TagController),
                    ApiName = nameof(CreateTag),
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
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Update tag API
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<TagModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTag([FromBody] TagModelRequest tagModelRequest, [FromQuery] int idTag)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                UpdateTagCommand cmd = new()
                {
                    IdTag = idTag,
                    TagName = tagModelRequest.TagName,
                    Details = tagModelRequest.Details
                };
                MethodResult<TagModelResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(TagController),
                    ApiName = nameof(UpdateTag),
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
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Remove tag API
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveTag([FromQuery] int idTag)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                RemoveTagCommand cmd = new()
                {
                    Idtag = idTag
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(TagController),
                    ApiName = nameof(RemoveTag),
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
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }

        /// <summary>
        /// Create new tag in story API
        /// </summary>
        /// <returns></returns>
        [HttpPost("stories")]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTagInStory([FromBody] CreateTagInStoryCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(TagController),
                    ApiName = nameof(CreateTagInStory),
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
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Update tag in story API
        /// </summary>
        /// <returns></returns>
        [HttpPut("stories")]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTagInStory([FromBody] TagInStoriesModelRequest tagModelRequest, [FromQuery] int idTagInStory)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                UpdateTagInStoryCommand cmd = new()
                {
                    IdTagInStory = idTagInStory,
                    TagId = tagModelRequest.TagId,
                    StoryId = tagModelRequest.StoryId
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(TagController),
                    ApiName = nameof(UpdateTagInStory),
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
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Remove tag in story API
        /// </summary>
        /// <returns></returns>
        [HttpDelete("stories")]
        [Authorize(Policy = nameof(RoleSettings.MOD))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveTagInStory([FromQuery] int idTagInStory)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                RemoveTagInStoryCommand cmd = new()
                {
                    IdTagInStory = idTagInStory
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(TagController),
                    ApiName = nameof(RemoveTagInStory),
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
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }

        #endregion

        #region Queries
        /// <summary>
        /// Get all tag API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<PagingItemsDTO<TagModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTags([FromQuery] int pageSize = 1, int pageIndex = 10)
        {
            try
            {
                MethodResult<PagingItemsDTO<TagModelResponse>> methodResult = await _tagQueries.GetAllTag(pageSize, pageIndex);
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Get all tag in stories API
        /// </summary>
        /// <returns></returns>
        [HttpGet("stories/all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<List<TagInStoriesModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTagInStories([FromQuery] int pageSize = 1, int pageIndex = 10)
        {
            try
            {
                MethodResult<List<TagInStoriesModelResponse>> methodResult = await _tagInStoriesQueries.GetAllTagInStory(pageIndex, pageSize);
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        /// <summary>
        /// Get special tag in stories API
        /// </summary>
        /// <returns></returns>
        [HttpGet("stories/tag-story")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<TagInStoriesModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTagInStoriesById([FromQuery] int tagId, int storyId)
        {
            try
            {
                MethodResult<TagInStoriesModelResponse> methodResult = await _tagInStoriesQueries.GetTagInStoriesById(tagId, storyId);
                return methodResult.GetActionResult();
            }
            catch (Exception ex)
            {
                var errCommandResult = new VoidMethodResult();
                errCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return errCommandResult.GetActionResult();
            }
        }
        #endregion
    }
}
