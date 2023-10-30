using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using ChapterEntites = MuonRoi.Social_Network.Chapters.Chapter;
using MuonRoiSocialNetwork.Common.Models.Chapter.Request;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoi.Social_Network.Storys;
using Serilog;
using BaseConfig.Extentions.String;

namespace MuonRoiSocialNetwork.Application.Commands.Chapter
{
    /// <summary>
    /// Create new chapter request
    /// </summary>
    public class CreateChapterCommand : ChapterModelRequest, IRequest<MethodResult<ChapterModelResponse>>
    {
    }
    /// <summary>
    /// Handler create new chapter
    /// </summary>
    public class CreateChapterCommandHandler : BaseStoriesCommandHandler, IRequestHandler<CreateChapterCommand, MethodResult<ChapterModelResponse>>
    {
        private readonly ILogger<CreateChapterCommandHandler> _logger;
        private readonly IChapterQueries _chapterQueries;
        private readonly IChapterRepository _chapterRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="chapterQueries"></param>
        /// <param name="chapterRepository"></param>
        /// <param name="logger"></param>
        public CreateChapterCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, IChapterQueries chapterQueries, IChapterRepository chapterRepository, ILoggerFactory logger) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<CreateChapterCommandHandler>();
            _chapterQueries = chapterQueries;
            _chapterRepository = chapterRepository;
        }

        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<ChapterModelResponse>> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            MethodResult<ChapterModelResponse> methodResult = new();
            ChapterEntites newChapter;
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC49C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC49C), nameof(EnumUserErrorCodes.USRC49C)) }
                    );
                    return methodResult;
                }
                #region Validation

                newChapter = _mapper.Map<ChapterEntites>(request);
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                newChapter.Slug = StringManagers.GenerateSlug(request.ChapterTitle);
                newChapter.NumberOfWord = newChapter.Body.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                if (!newChapter.IsValid())
                {
                    throw new CustomException(newChapter.ErrorMessages);
                }
                #endregion

                #region Check exist story
                Story existStory = await _storiesQuerie.GetByIdAsync(request.StoryId);
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

                #region Create new chapter
                newChapter.Body = StringManagers.CompressHtml(newChapter.Body);
                _chapterRepository.Add(newChapter);
                int checkStatus = await _chapterRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (checkStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC51C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC51C), nameof(EnumUserErrorCodes.USRC51C)) }
                    );
                    return methodResult;
                }
                #endregion
                #region Update total chapter
                existStory.TotalChapter += 1;
                _storiesRepository.Update(existStory);
                await _storiesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create chapter command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create chapter) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Create chapter) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            ChapterEntites chapter = await _chapterQueries.GetByGuidAsync(newChapter.Guid);
            methodResult.Result = _mapper.Map<ChapterModelResponse>(chapter);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
