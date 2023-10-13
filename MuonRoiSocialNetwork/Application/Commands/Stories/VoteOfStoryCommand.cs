using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Notifications;
using MuonRoiSocialNetwork.Common.Models.Notifications.Base;
using MuonRoiSocialNetwork.Common.Models.Stories.Response.Dto;
using MuonRoiSocialNetwork.Common.Settings.SignalRSettings.GroupName;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Infrastructure.HubCentral;
using MuonRoiSocialNetwork.Infrastructure.Repositories.Stories;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Vote story request
    /// </summary>
    public class VoteOfStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id story update
        /// </summary>
        public int StoryId { get; set; }
        /// <summary>
        /// Vote value
        /// </summary>
        public double VoteValue { get; set; }
    }
    /// <summary>
    /// Handle vote story
    /// </summary>
    public class VoteOfStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<VoteOfStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<VoteOfStoryCommandHandler> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly AuthContext _authContext;
        private readonly IStoryNotificationRepository _storyNotificationRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="logger"></param>
        /// <param name="hubContext"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="auth"></param>
        /// <param name="storyNotificationRepository"></param>
        public VoteOfStoryCommandHandler(ILoggerFactory logger, IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, IHubContext<NotificationHub> hubContext, AuthContext auth, IStoryNotificationRepository storyNotificationRepository) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<VoteOfStoryCommandHandler>();
            _hubContext = hubContext;
            _authContext = auth;
            _storyNotificationRepository = storyNotificationRepository;
        }
        /// <summary>
        /// Function handle vote of story
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(VoteOfStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            StoryRattings storyRattings = new()
            {
                Data = new List<Rattings>()
            };
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), EnumStoryErrorCode.ST10) }
                    );
                    return methodResult;
                }
                #region Check story is exist
                Story existStory = await _storiesQuerie.GetByIdAsync(request.StoryId);
                if (existStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryId), request.StoryId) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Publish
                storyRattings = JsonConvert.DeserializeObject<StoryRattings>(existStory.ListRattings) ?? new()
                {
                    Data = new List<Rattings>()
                };
                var userVote = storyRattings.Data.FirstOrDefault(x => x.UserId == _authContext.CurrentUserId);
                if (userVote != null)
                    storyRattings.Data.Remove(userVote);
                storyRattings.Data.Add(new Rattings
                {
                    RattingValues = request.VoteValue,
                    UserId = _authContext.CurrentUserId
                });
                existStory.Rating = storyRattings.Data.Average(x => x.RattingValues);
                existStory.ListRattings = JsonConvert.SerializeObject(storyRattings);
                _storiesRepository.Update(existStory);
                await _storiesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                #endregion

                #region Send notification to user favorite
                await _hubContext.Clients.Group(string.Format(GroupHelperConst.Instance.GroupNameVoteHear, existStory.Guid)).SendAsync("ReceiveSingle", new BaseNotificationModels
                {
                    NotificationContent = $"{existStory.StoryTitle}-{_authContext.CurrentNameUser}-{storyRattings.Data.Average(x => x.RattingValues)}",
                    TimeCreated = DateTime.Now.ToString("MM/dd"),
                    Type = Common.Settings.SignalRSettings.Enum.NotificationType.VoteStory
                }, existStory.CreatedUserGuid, cancellationToken: cancellationToken);
                #endregion

                #region Save notification to db
                var storyNotification = new StoryNotifications()
                {
                    Title = existStory.StoryTitle,
                    Message = $"{_authContext.CurrentNameUser}-{existStory.StoryTitle}",
                    ImgUrl = existStory.ImgUrl,
                    NotificationUrl = "notification/user",
                    StoryId = existStory.Id,
                    UserGuid = Guid.Parse(_authContext.CurrentUserId),
                    NotificationSate = EnumStateNotification.SENT,
                    NotificationType = Common.Settings.SignalRSettings.Enum.NotificationType.VoteStory
                };
                _storyNotificationRepository.Add(storyNotification);
                await _storyNotificationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Vote story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Vote story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
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
