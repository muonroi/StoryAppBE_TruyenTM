using AutoMapper;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Request
    /// </summary>
    public class DeleteSignleNotificationCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id notification
        /// </summary>
        public int Id { get; set; }
    }
    /// <summary>
    /// Handler
    /// </summary>
    public class DeleteSignleNotificationCommandHandler : BaseStoriesCommandHandler, IRequestHandler<DeleteSignleNotificationCommand, MethodResult<bool>>
    {
        private readonly IStoryNotificationRepository _storyNotificationRepository;
        private readonly ILogger<DeleteSignleNotificationCommandHandler> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="storyNotificationRepository"></param>
        /// <param name="logger"></param>
        public DeleteSignleNotificationCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, IStoryNotificationRepository storyNotificationRepository, ILoggerFactory logger) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<DeleteSignleNotificationCommandHandler>();
            _storyNotificationRepository = storyNotificationRepository;
        }
        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(DeleteSignleNotificationCommand request, CancellationToken cancellationToken)
        {
            var methodResult = new MethodResult<bool>();
            try
            {
                #region Valid request
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumNotificationStoryErrorCodes.NT06),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumNotificationStoryErrorCodes.NT06), EnumNotificationStoryErrorCodes.NT06) }
                    );
                    return methodResult;
                }
                #endregion

                #region Get notification by id
                var notificationInfo = await _storyNotificationRepository.GetByIdAsync(request.Id);
                #endregion
                await _storyNotificationRepository.DeleteAsync(notificationInfo);
                methodResult.Result = true;
                methodResult.StatusCode = StatusCodes.Status200OK;
                return methodResult;
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Delete single notification of user) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Delete single notification of user) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Delete single notification of user) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }

        }
    }
}
