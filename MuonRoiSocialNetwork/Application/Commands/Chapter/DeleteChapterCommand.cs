using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Storys;
using ChapterEntites = MuonRoi.Social_Network.Chapters.Chapter;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using MuonRoi.Social_Network.Chapters;
using MuonRoi.Social_Network.Users;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Chapter
{
    /// <summary>
    /// Delete chapter request
    /// </summary>
    public class DeleteChapterCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// id of chapter
        /// </summary>
        public int Id { get; set; }
    }
    /// <summary>
    /// Delete chapter class
    /// </summary>
    public class DeleteChapterCommandHandler : IRequestHandler<DeleteChapterCommand, MethodResult<bool>>
    {
        private readonly ILogger<DeleteChapterCommandHandler> _logger;
        private readonly IChapterQueries _chapterQueries;
        private readonly IChapterRepository _chapterRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="chapterQueries"></param>
        /// <param name="chapterRepository"></param>
        /// <param name="logger"></param>
        public DeleteChapterCommandHandler(IChapterQueries chapterQueries, IChapterRepository chapterRepository, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<DeleteChapterCommandHandler>();
            _chapterQueries = chapterQueries;
            _chapterRepository = chapterRepository;
        }
        /// <summary>
        /// Function handle delete chapter
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
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
                #region Check exist chapter
                ChapterEntites existChapter = await _chapterQueries.GetByIdAsync(request.Id);
                if (existChapter is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumChapterErrorCode.CT11),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Delete chapter
                await _chapterRepository.DeleteAsync(existChapter);
                await _chapterRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create chapter command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create chapter) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Create chapter) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
            methodResult.Result = true;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
