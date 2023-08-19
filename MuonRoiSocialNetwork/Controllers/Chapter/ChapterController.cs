using BaseConfig.EntityObject.Entity;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChapterEntites = MuonRoi.Social_Network.Chapters.Chapter;
using MuonRoiSocialNetwork.Application.Commands.Chapter;
using MuonRoiSocialNetwork.Common.Models.Chapter.Request;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using System.Net;
using AutoMapper;
using BaseConfig.BaseDbContext.Common;
using MuonRoi.Social_Network.Chapters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using BaseConfig.Infrashtructure;
using System.Diagnostics;
using MuonRoiSocialNetwork.Common.Models.Logs;
using Serilog;
using Newtonsoft.Json;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;

namespace MuonRoiSocialNetwork.Controllers.Chapter
{
    /// <summary>
    /// Auth: PhiLe 20230531
    /// </summary>
    [Route("api/chapters")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChapterController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthContext _auth;
        private readonly IMediator _mediator;
        private readonly IChapterQueries _chapterQueries;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="chapterQueries"></param>
        /// <param name="mapper"></param>
        /// <param name="auth"></param>
        /// <param name="httpContextAccessor"></param>
        public ChapterController(IMediator mediator, IChapterQueries chapterQueries, IMapper mapper, AuthContext auth, IHttpContextAccessor httpContextAccessor)
        {
            _auth = auth;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _chapterQueries = chapterQueries;
            _mapper = mapper;
        }

        #region Repository
        /// <summary>
        /// Create new chapter of story API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = nameof(RoleSettings.AUTHOR))]
        [ProducesResponseType(typeof(MethodResult<ChapterModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> InitialNewChapter([FromForm] CreateChapterCommand cmd)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                MethodResult<ChapterModelResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(ChapterController),
                    ApiName = nameof(InitialNewChapter),
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
        /// Update chapter of story API
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = nameof(RoleSettings.AUTHOR))]
        [ProducesResponseType(typeof(MethodResult<ChapterModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateChapter([FromQuery] int chapterId, [FromForm] ChapterModelRequest chapterModelRequest)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                UpdateChapterCommand cmd = new()
                {
                    Id = chapterId,
                    InfoUpdate = chapterModelRequest
                };
                MethodResult<ChapterModelResponse> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(ChapterController),
                    ApiName = nameof(UpdateChapter),
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
        /// Remove chapter of story API
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = nameof(RoleSettings.AUTHOR))]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteChapter([FromQuery] int chapterId)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                DeleteChapterCommand cmd = new()
                {
                    Id = chapterId
                };
                MethodResult<bool> methodResult = await _mediator.Send(cmd).ConfigureAwait(false);
                stopwatch.Stop();
                LogsDto logsInfo = new()
                {
                    Username = _auth.CurrentUsername,
                    ServiceName = nameof(ChapterController),
                    ApiName = nameof(DeleteChapter),
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
        /// Get detail chapter API
        /// </summary>
        /// <returns></returns>
        [HttpGet("detail")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<ChapterModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDetailChapter([FromQuery] int chapterId)
        {
            try
            {
                MethodResult<ChapterModelResponse> methodResult = new();
                ChapterEntites chapterDetailResult = await _chapterQueries.GetByIdAsync(chapterId).ConfigureAwait(false);
                if (chapterDetailResult == null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumChapterErrorCode.CT11),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                    );
                    return methodResult.GetActionResult();
                }
                methodResult.StatusCode = StatusCodes.Status200OK;
                methodResult.Result = _mapper.Map<ChapterModelResponse>(chapterDetailResult);
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
        /// Get all chapter by story guid API
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<PagingItemsDTO<ChapterPreviewResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllChapter([FromQuery] int storyId, int pageIndex = 1, int pageSize = 10, bool isLatest = false)
        {
            try
            {
                MethodResult<PagingItemsDTO<ChapterPreviewResponse>> methodResult = await _chapterQueries.GetListChapterOfStory(storyId, pageIndex, pageSize, isLatest).ConfigureAwait(false);
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
        /// Get group chapter API
        /// </summary>
        /// <returns></returns>
        [HttpGet("group")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<PagingItemsDTO<ChapterModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGroupChapter([FromQuery] int storyId, int fromChapterId = 0, int pageIndex = 1, int pageSize = 20, bool isSetCache = false)
        {
            try
            {
                MethodResult<PagingItemsDTO<ChapterModelResponse>> methodResult = await _chapterQueries.GetGroupChapterAsync(storyId, fromChapterId, pageIndex, pageSize, isSetCache).ConfigureAwait(false);
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
        /// Get all chapter API
        /// </summary>
        /// <returns></returns>
        [HttpGet("latest/all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<PagingItemsDTO<ChapterModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllChapter([FromQuery] int pageIndex = 1, int pageSize = 20, bool isSetCache = false)
        {
            try
            {
                MethodResult<PagingItemsDTO<ChapterModelResponse>> methodResult = await _chapterQueries.GetAllChapterAsync(pageIndex, pageSize, isSetCache).ConfigureAwait(false);
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
