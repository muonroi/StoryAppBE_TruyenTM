using BaseConfig.EntityObject.Entity;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MuonRoiSocialNetwork.Application.Commands.Chapter;
using MuonRoiSocialNetwork.Common.Models.Chapter.Request;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using System.Net;
using AutoMapper;
using BaseConfig.BaseDbContext.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using BaseConfig.Infrashtructure;
using System.Diagnostics;
using MuonRoiSocialNetwork.Common.Models.Logs;
using Serilog;
using Newtonsoft.Json;
using MuonRoiSocialNetwork.Common.Settings.RoleSettings;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;

namespace MuonRoiSocialNetwork.Controllers.Chapter
{
    /// <summary>
    /// Auth: PhiLe 20230531
    /// </summary>
    [ApiVersion(MainSettings.APIVersion)]
    [Route("api/v{version:apiVersion}/chapters")]
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
                    Request = JsonConvert.SerializeObject(cmd),
                    Response = JsonConvert.SerializeObject(methodResult),
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
                    Request = JsonConvert.SerializeObject(cmd),
                    Response = JsonConvert.SerializeObject(methodResult),
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
                    Request = JsonConvert.SerializeObject(cmd),
                    Response = JsonConvert.SerializeObject(methodResult),
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
                MethodResult<ChapterModelResponse> methodResult = await _chapterQueries.GetDetailChapterById(chapterId).ConfigureAwait(false);
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
        [ProducesResponseType(typeof(MethodResult<IEnumerable<ChapterModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGroupChapter([FromQuery] int storyId, long fromChapterId = 0, long toChapterId = 0, bool isSetCache = false)
        {
            try
            {
                MethodResult<IEnumerable<ChapterModelResponse>> methodResult = await _chapterQueries.GetGroupChapterAsync(storyId, fromChapterId, toChapterId, isSetCache).ConfigureAwait(false);
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
        /// <summary>
        /// Next chapter with story id and chapter id API
        /// </summary>
        /// <returns></returns>
        [HttpGet("next")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<ChapterModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> NextChapterByStoryId([FromQuery] int storyId, long chapterId)
        {
            try
            {
                MethodResult<ChapterModelResponse> methodResult = await _chapterQueries.NextChapterByStoryId(storyId, chapterId).ConfigureAwait(false);
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
        /// Previous chapter with story id and chapter id API
        /// </summary>
        /// <returns></returns>
        [HttpGet("previous")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<ChapterModelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PreviousChapterByStoryId([FromQuery] int storyId, long chapterId)
        {
            try
            {
                MethodResult<ChapterModelResponse> methodResult = await _chapterQueries.PreviousChapterByStoryId(storyId, chapterId).ConfigureAwait(false);
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
        /// Get list chapter and paging according by 100 chapters each chunk API
        /// </summary>
        /// <returns></returns>
        [HttpGet("paging-chapter")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<List<ChapterListPagingResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PagingChapterListByStoryId([FromQuery] int storyId, bool isSetCache = false)
        {
            try
            {
                MethodResult<List<ChapterListPagingResponse>> methodResult = await _chapterQueries.PagingChapterListByStoryId(storyId, isSetCache).ConfigureAwait(false);
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
        /// Get chunk chapter each 250 character API
        /// </summary>
        /// <returns></returns>
        [HttpGet("chunk-chapter")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<ChapterChunkResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChunkChapterListByStoryId([FromQuery] int chapterId, int chunkSize = 250)
        {
            try
            {
                MethodResult<ChapterChunkResponse> methodResult = await _chapterQueries.ChunkChapterListByStoryId(chapterId, chunkSize).ConfigureAwait(false);
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
        /// Get group character API
        /// </summary>
        /// <returns></returns>
        [HttpGet("group-chapter")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MethodResult<PagingItemsDTO<ChapterModelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GroupChapterListByStoryId([FromQuery] int storyId, int pageIndex, int pageSize = 100, bool isSetCache = false)
        {
            try
            {
                MethodResult<PagingItemsDTO<ChapterModelResponse>> methodResult = await _chapterQueries.GroupChapterListByStoryId(storyId, pageIndex, pageSize, isSetCache).ConfigureAwait(false);
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
