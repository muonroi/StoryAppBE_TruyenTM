using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Extentions.String;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Chapter.Request;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Serilog;
using ChapterEntites = MuonRoi.Social_Network.Chapters.Chapter;

namespace MuonRoiSocialNetwork.Application.Commands.Chapter
{
    /// <summary>
    /// Update chapter model request
    /// </summary>
    public class UpdateChapterCommand : IRequest<MethodResult<ChapterModelResponse>>
    {
        /// <summary>
        /// id of chapter
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Data update
        /// </summary>
        public ChapterModelRequest? InfoUpdate { get; set; }
    }
    /// <summary>
    /// Update chapter class
    /// </summary>
    public class UpdateChapterCommandHandler : BaseStoriesCommandHandler, IRequestHandler<UpdateChapterCommand, MethodResult<ChapterModelResponse>>
    {
        private readonly ILogger<UpdateChapterCommandHandler> _logger;
        private readonly IChapterQueries _chapterQueries;
        private readonly IChapterRepository _chapterRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="chapterQueries"></param>
        /// <param name="chapterRepository"></param>
        public UpdateChapterCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, ILoggerFactory logger, IChapterQueries chapterQueries, IChapterRepository chapterRepository) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<UpdateChapterCommandHandler>();
            _chapterQueries = chapterQueries;
            _chapterRepository = chapterRepository;
        }
        /// <summary>
        /// Function handler update chapter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<ChapterModelResponse>> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            MethodResult<ChapterModelResponse> methodResult = new();
            ChapterEntites updateChapter;
            try
            {
                #region Validation
                if (request is null || request.InfoUpdate is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), nameof(EnumStoryErrorCode.ST10)) }
                    );
                    return methodResult;
                }
                updateChapter = _mapper.Map<ChapterEntites>(request.InfoUpdate);
                updateChapter.Id = request.Id;
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                updateChapter.Slug = StringManagers.GenerateSlug(request.InfoUpdate.ChapterTitle);
                updateChapter.NumberOfWord = updateChapter.Body.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

                if (!updateChapter.IsValid())
                {
                    throw new CustomException(updateChapter.ErrorMessages);
                }
                #endregion

                #region Check exist story
                Story existStory = await _storiesQuerie.GetByIdAsync(request.InfoUpdate.StoryId);
                if (existStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), nameof(EnumStoryErrorCode.ST10)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Update chapter
                updateChapter.Body = StringManagers.CompressHtml(updateChapter.Body);
                _chapterRepository.Update(updateChapter);
                int checkStatus = await _chapterRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (checkStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC50C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC50C), nameof(EnumUserErrorCodes.USRC50C)) }
                    );
                    return methodResult;
                }
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update chapter command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update chapter) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Update chapter) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            ChapterEntites chapter = await _chapterQueries.GetByGuidAsync(updateChapter.Guid);
            methodResult.Result = _mapper.Map<ChapterModelResponse>(chapter);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
