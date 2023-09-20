using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Notifications;
using MuonRoiSocialNetwork.Common.Models.Stories.Request;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.SignleName;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using MuonRoiSocialNetwork.Infrastructure.HubCentral;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Request
    /// </summary>
    public class BookmarkStoryCommand : BookmarkStoryModelRequest, IRequest<MethodResult<bool>>
    { }
    /// <summary>
    /// Handle
    /// </summary>
    public class BookmarkStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<BookmarkStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<BookmarkStoryCommandHandler> _logger;
        private readonly IUserQueries _userQueries;
        private readonly AuthContext _authContext;
        private readonly IBookmarkStoryRepository _bookmarkStoryRepository;
        private readonly IBookmarkStoryQueries _bookmarkStoryQueries;
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="userQueries"></param>
        /// <param name="authContext"></param>
        /// <param name="bookmarkStoryRepository"></param>
        /// <param name="bookmarkStoryQueries"></param>
        /// <param name="hubContext"></param>
        public BookmarkStoryCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, ILoggerFactory logger, IUserQueries userQueries, AuthContext authContext, IBookmarkStoryRepository bookmarkStoryRepository, IBookmarkStoryQueries bookmarkStoryQueries, IHubContext<NotificationHub> hubContext) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<BookmarkStoryCommandHandler>();
            _userQueries = userQueries;
            _authContext = authContext;
            _bookmarkStoryRepository = bookmarkStoryRepository;
            _bookmarkStoryQueries = bookmarkStoryQueries;
            _hubContext = hubContext;
        }
        /// <summary>
        /// Method handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(BookmarkStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
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

                #region Check story is exist
                Story existStory = await _storiesQuerie.GetByIdAsync(request.StoryId);
                if (existStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryId), request.StoryId.ToString() ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Check user is exist
                MethodResult<BaseUserResponse> baseUserResponse = await _userQueries.GetUserModelByGuidAsync(new Guid(_authContext.CurrentUserId));
                if (baseUserResponse.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR13C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Check user was marked story bookmark?
                var bookmarkResult = await _bookmarkStoryQueries.ExistBookmarkStoryOfUser(existStory.Guid, new Guid(_authContext.CurrentUserId));
                if (bookmarkResult.Result is null)
                {
                    _bookmarkStoryRepository.Add(new BookmarkStory
                    {
                        StoryGuid = existStory.Guid,
                        UserGuid = new Guid(_authContext.CurrentUserId),
                        BookmarkDate = DateTime.UtcNow,
                    });
                    await _bookmarkStoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                }
                #endregion

                #region Push notifycation to author
                await _hubContext.Clients.User(existStory.CreatedUserGuid.ToString() ?? Guid.NewGuid().ToString())
                    .SendAsync(SingleHelperConst.Instance.StreamUserSpecial, JsonConvert.SerializeObject(new NotificationModels
                    {
                        NotificationContent = $"{_authContext.CurrentNameUser}-{existStory.StoryTitle}",
                        TimeCreated = DateTime.Now.ToString("MM/dd"),
                        Url = existStory.ImgUrl,
                        Type = Common.Settings.SignalRSettings.Enum.NotificationType.BookmarkStory
                    }), cancellationToken: cancellationToken);

                #endregion
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
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = true;
            return methodResult;
        }
    }
}
