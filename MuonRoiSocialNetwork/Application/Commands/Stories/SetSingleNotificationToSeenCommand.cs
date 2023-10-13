using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Request
    /// </summary>
    public class SetSingleNotificationToSeenCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id notification
        /// </summary>
        public int NotificationId { get; set; }
    }
    /// <summary>
    /// Handler
    /// </summary>
    public class UpdateNotificationCommandHandler : BaseStoriesCommandHandler, IRequestHandler<SetSingleNotificationToSeenCommand, MethodResult<bool>>
    {
        private readonly IStoryNotificationRepository _storyNotificationRepository;
        private readonly ILogger<UpdateNotificationCommandHandler> _logger;
        private readonly AuthContext _authContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="storyNotificationRepository"></param>
        /// <param name="logger"></param>
        /// <param name="authContext"></param>
        public UpdateNotificationCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, IStoryNotificationRepository storyNotificationRepository, ILoggerFactory logger, AuthContext authContext) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _storyNotificationRepository = storyNotificationRepository;
            _logger = logger.CreateLogger<UpdateNotificationCommandHandler>();
            _authContext = authContext;
        }
        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(SetSingleNotificationToSeenCommand request, CancellationToken cancellationToken)
        {
            var methodResult = new MethodResult<bool>();
            try
            {
                #region Get notification
                var notificationInfo = await _storyNotificationRepository.GetWhereAsync(x => x.Id == request.NotificationId && x.UserGuid == Guid.Parse(_authContext.CurrentUserId));
                if (notificationInfo is null || !notificationInfo.Any())
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumNotificationStoryErrorCodes.NT06),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumNotificationStoryErrorCodes.NT06), EnumNotificationStoryErrorCodes.NT06) }
                    );
                    return methodResult;
                }
                #endregion

                #region Update notification to seen
                notificationInfo.First().NotificationSate = EnumStateNotification.SEEN;
                _storyNotificationRepository.Update(notificationInfo.First());
                await _storyNotificationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                #endregion

                methodResult.Result = true;
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(set single notification to seen of user) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(set single notification to seen of user) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(set single notification to seen of user) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
        }
    }
}
