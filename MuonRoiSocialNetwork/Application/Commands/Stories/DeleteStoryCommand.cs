using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
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
    /// DeleteCommand request
    /// </summary>
    public class DeleteStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id story update
        /// </summary>
        public int StoryId { get; set; }
    }
    /// <summary>
    /// Handle create story
    /// </summary>
    public class DeleteStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<DeleteStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<DeleteStoryCommandHandler> _logger;
        private readonly AuthContext _authContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="authContext"></param>
        public DeleteStoryCommandHandler(ILoggerFactory logger, IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, AuthContext authContext) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<DeleteStoryCommandHandler>();
            _authContext = authContext;
        }
        /// <summary>
        /// Handle function
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST00),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
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

                #region Delete story
                await _storiesRepository.ExecuteTransactionAsync(async () =>
                {
                    await _storiesRepository.DeleteAsync(existStory);
                    await _storiesRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);
                    methodResult.Result = true;
                    return methodResult;
                });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Delete story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Delete story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
