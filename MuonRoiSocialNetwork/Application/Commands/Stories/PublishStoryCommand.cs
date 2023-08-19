using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Notifications;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Infrastructure.HubCentral;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Publish story request
    /// </summary>
    public class PublishStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Guid story update
        /// </summary>
        public int StoryId { get; set; }
    }
    /// <summary>
    /// Handle update story
    /// </summary>
    public class PublishStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<PublishStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<PublishStoryCommandHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="hubContext"></param>
        public PublishStoryCommandHandler(ILoggerFactory logger, IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, IHubContext<NotificationHub> hubContext) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<PublishStoryCommandHandler>();
            _hubContext = hubContext;
        }
        /// <summary>
        /// Handle function
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(PublishStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST00),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST00), EnumStoryErrorCode.ST00) }
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

                #region Pushlish
                await _storiesRepository.UpdateSingleEntry(existStory, nameof(existStory.IsShow), true);
                #endregion

                #region Send notification to user favorite
                await _hubContext.Clients.Group(string.Format(GroupHelperConst.Instance.GroupNameFavorite, existStory.Guid)).SendAsync("ReceiveNotification", new NotificationModels
                {
                    NotificationContent = $"Your story {existStory.StoryTitle} was published",
                    TimeCreated = DateTime.Now.ToString("MM/dd")
                }, cancellationToken: cancellationToken);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Publish story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Publish story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
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
