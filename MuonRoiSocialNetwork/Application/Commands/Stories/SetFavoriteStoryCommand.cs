using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MuonRoi.Social_Network.Categories;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Notifications;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName;
using MuonRoiSocialNetwork.Domains.DomainObjects.Storys;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Infrastructure.HubCentral;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Favorite of story request
    /// </summary>
    public class SetFavoriteStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Guid story update
        /// </summary>
        public int StoryId { get; set; }
    }
    /// <summary>
    /// Set favorite story handler
    /// </summary>
    public class SetFavoriteStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<SetFavoriteStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<SetFavoriteStoryCommandHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IStoriesFavoriteRepository _storiesFavoriteRepository;
        private readonly IStoriesFavoriteQueries _storiesFavoriteQueries;
        private readonly AuthContext _authContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="hubContext"></param>
        /// <param name="storiesFavoriteRepository"></param>
        /// <param name="context"></param>
        /// <param name="storiesFavoriteQueries"></param>
        public SetFavoriteStoryCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, ILoggerFactory logger, IHubContext<NotificationHub> hubContext, IStoriesFavoriteRepository storiesFavoriteRepository, AuthContext context, IStoriesFavoriteQueries storiesFavoriteQueries) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<SetFavoriteStoryCommandHandler>();
            _hubContext = hubContext;
            _storiesFavoriteRepository = storiesFavoriteRepository;
            _authContext = context;
            _storiesFavoriteQueries = storiesFavoriteQueries;
        }
        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(SetFavoriteStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), EnumStoryErrorCode.ST10) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #region Check story is exist
                Story existStory = await _storiesQuerie.GetByIdAsync(request.StoryId);
                if (existStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryId), request.StoryId.ToString() ?? "") }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Check user was like story or not?
                var isWasLikeStory = await _storiesFavoriteQueries.IsUserWasLikeStoryAsync(Guid.Parse(_authContext.CurrentUserId), existStory.Id);
                if (isWasLikeStory.Result == true)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST14),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryId), request.StoryId.ToString() ?? "") }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Set favorite of story up
                await _storiesRepository.UpdateSingleEntry(existStory, nameof(existStory.TotalFavorite), existStory.TotalFavorite += 1);
                _storiesFavoriteRepository.Add(new StoryFavorite
                {
                    StoryId = request.StoryId,
                    UserGuid = Guid.Parse(_authContext.CurrentUserId),
                });
                await _storiesFavoriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                #endregion

                #region Send notification to user favorite
                await _hubContext.Clients.Group(string.Format(GroupHelperConst.Instance.VoteHeartToAuthor, existStory.Guid)).SendAsync("ReceiveSingle", new NotificationModels
                {
                    NotificationContent = $"Your story {existStory.StoryTitle} have new likes!",
                    TimeCreated = DateTime.Now.ToString("MM/dd")
                }, existStory.CreatedUserGuid, cancellationToken: cancellationToken);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Set favorite of story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Set favorite of story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
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
