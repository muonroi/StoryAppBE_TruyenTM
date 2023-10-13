using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.HubCentral;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Stories;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Request
    /// </summary>
    public class DeleteBookmarkStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// story id
        /// </summary>
        [JsonProperty("story-guid")]
        public Guid StoryGuid { get; set; }
    }
    /// <summary>
    /// Handler
    /// </summary>
    public class DeleteBookmarkStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<DeleteBookmarkStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<BookmarkStoryCommandHandler> _logger;
        private readonly AuthContext _authContext;
        private readonly IBookmarkStoryRepository _bookmarkStoryRepository;
        private readonly IBookmarkStoryQueries _bookmarkStoryQueries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="authContext"></param>
        /// <param name="bookmarkStoryRepository"></param>
        /// <param name="bookmarkStoryQueries"></param>
        public DeleteBookmarkStoryCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, ILoggerFactory logger, AuthContext authContext, IBookmarkStoryRepository bookmarkStoryRepository, IBookmarkStoryQueries bookmarkStoryQueries) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<BookmarkStoryCommandHandler>();
            _authContext = authContext;
            _bookmarkStoryRepository = bookmarkStoryRepository;
            _bookmarkStoryQueries = bookmarkStoryQueries;
        }
        /// <summary>
        /// Handle funciton
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(DeleteBookmarkStoryCommand request, CancellationToken cancellationToken)
        {
            var methodResult = new MethodResult<bool>();
            try
            {
                #region Valid request
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC49C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC49C), EnumUserErrorCodes.USRC49C) }
                    );
                    return methodResult;
                }
                #endregion

                #region Check story bookmark for user
                var bookmarkResult = await _bookmarkStoryQueries.ExistBookmarkStoryOfUser(request.StoryGuid, new Guid(_authContext.CurrentUserId));
                if (bookmarkResult.Result is not null)
                {
                    await _bookmarkStoryRepository.ExecuteTransactionAsync(async () =>
                    {
                        var bookmark = await _bookmarkStoryQueries.GetByIdAsync(bookmarkResult.Result.Id);
                        await _bookmarkStoryRepository.DeleteAsync(bookmark);
                        await _bookmarkStoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
                        methodResult.Result = true;
                        methodResult.StatusCode = StatusCodes.Status200OK;
                        return methodResult;
                    });
                }
                #endregion

                methodResult.Result = false;
                methodResult.StatusCode = StatusCodes.Status400BadRequest;

                return methodResult;
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Bookmark story) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Bookmark story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Bookmark story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
        }
    }
}
