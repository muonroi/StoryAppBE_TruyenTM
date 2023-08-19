using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Enums.Storys;
using MuonRoiSocialNetwork.Common.Models.Stories.Request;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Update story review request
    /// </summary>
    public class UpdateCommentStoryCommand : IRequest<MethodResult<StoryReviewModelResponse>>
    {
        /// <summary>
        /// request story comment update
        /// </summary>
        public StoryReviewModelRequest? StoryReviewModelRequest { get; set; }
        /// <summary>
        /// Guid comment update
        /// </summary>
        public int IdComment { get; set; }
    }
    /// <summary>
    /// Define class update comment
    /// </summary>
    public class UpdateCommentStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<UpdateCommentStoryCommand, MethodResult<StoryReviewModelResponse>>
    {
        private readonly ILogger<UpdateCommentStoryCommandHandler> _logger;
        private readonly IReviewStoryQueries _reviewStoryQueries;
        private readonly IReviewStoryRepository _reviewStoryRepository;
        private readonly IUserQueries _userQueries;
        private readonly AuthContext _authContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="reviewStoryQueries"></param>
        /// <param name="reviewStoryRepository"></param>
        /// <param name="logger"></param>
        /// <param name="userQueries"></param>
        /// <param name="authContext"></param>
        public UpdateCommentStoryCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, IReviewStoryQueries reviewStoryQueries, IReviewStoryRepository reviewStoryRepository, ILoggerFactory logger, IUserQueries userQueries, AuthContext authContext) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _reviewStoryQueries = reviewStoryQueries;
            _reviewStoryRepository = reviewStoryRepository;
            _logger = logger.CreateLogger<UpdateCommentStoryCommandHandler>();
            _userQueries = userQueries;
            _authContext = authContext;
        }
        /// <summary>
        /// Function define
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<StoryReviewModelResponse>> Handle(UpdateCommentStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<StoryReviewModelResponse> methodResult = new();
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
                #region Valid request
                if (request.StoryReviewModelRequest is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), EnumStoryErrorCode.ST10) }
                    );
                    return methodResult;
                }
                StoryReview storyReview = _mapper.Map<StoryReview>(request.StoryReviewModelRequest);
                storyReview.Id = request.IdComment;
                if (!storyReview.IsValid())
                {
                    throw new CustomException(storyReview.ErrorMessages);
                }
                #endregion

                #region Check story is exist
                Story existStory = await _storiesQuerie.GetByIdAsync(request.StoryReviewModelRequest.StoryId);
                if (existStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryReviewModelRequest.StoryId), request.StoryReviewModelRequest.StoryId.ToString() ?? "") }
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

                #region Check user update is match with user create
                if (!(storyReview.CreatedUserGuid == new Guid(_authContext.CurrentUserId)))
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumReviewStorys.RVST05),
                        new[] { Helpers.GenerateErrorResult(nameof(baseUserResponse.Result.Username), baseUserResponse.Result.Username ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Comment to story
                _reviewStoryRepository.Update(storyReview);
                int resultStatus = await _reviewStoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                if (resultStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC51C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC51C), nameof(EnumUserErrorCodes.USRC51C)) }
                    );
                    return methodResult;
                }
                #endregion
                StoryReview reviewAdded = await _reviewStoryQueries.GetByIdAsync(storyReview.Id).ConfigureAwait(false);
                methodResult.Result = new StoryReviewModelResponse
                {
                    DisplayNameUser = string.Format($"{0} {1}", baseUserResponse.Result.Surname, baseUserResponse.Result.Name),
                    Content = reviewAdded.Content,
                    Rating = reviewAdded.Rating,
                    CreatetedDate = reviewAdded.CreatedDateTS ?? 0
                };
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update comments story) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update comments story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Update comments story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
