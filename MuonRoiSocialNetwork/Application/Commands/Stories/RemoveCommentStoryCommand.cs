using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Roles;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Enums.Storys;
using MuonRoiSocialNetwork.Common.Models.Users.Base.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.GroupAndRoles;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Users;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Request comment story
    /// </summary>
    public class RemoveCommentStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id of story
        /// </summary>
        public int IdCommentStory { get; set; }
    }
    /// <summary>
    /// Class remove comment story
    /// </summary>
    public class RemoveCommentStoryCommandHandler : IRequestHandler<RemoveCommentStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<RemoveCommentStoryCommandHandler> _logger;
        private readonly IReviewStoryQueries _reviewStoryQueries;
        private readonly IReviewStoryRepository _reviewStoryRepository;
        private readonly IUserQueries _userQueries;
        private readonly IGroupQueries _groupQueries;
        private readonly AuthContext _authContext;
        /// <summary>
        /// Construcor
        /// </summary>
        /// <param name="reviewStoryQueries"></param>
        /// <param name="reviewStoryRepository"></param>
        /// <param name="logger"></param>
        /// <param name="userQueries"></param>
        /// <param name="groupQueries"></param>
        /// <param name="authContext"></param>
        public RemoveCommentStoryCommandHandler(IReviewStoryQueries reviewStoryQueries, IReviewStoryRepository reviewStoryRepository, ILoggerFactory logger, IUserQueries userQueries, IGroupQueries groupQueries, AuthContext authContext)
        {
            _reviewStoryQueries = reviewStoryQueries;
            _reviewStoryRepository = reviewStoryRepository;
            _logger = logger.CreateLogger<RemoveCommentStoryCommandHandler>();
            _userQueries = userQueries;
            _groupQueries = groupQueries;
            _authContext = authContext;
        }
        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(RemoveCommentStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumReviewStorys.RVST04),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #region Check comment is exist
                StoryReview storyReview = await _reviewStoryQueries.GetByIdAsync(request.IdCommentStory).ConfigureAwait(false);
                storyReview.Id = request.IdCommentStory;
                if (storyReview is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumReviewStorys.RVST04),
                        new[] { Helpers.GenerateErrorResult(nameof(request.IdCommentStory), request.IdCommentStory) }
                    );
                    return methodResult;
                }
                #endregion

                #region Check user is user create comment ?
                MethodResult<BaseUserResponse> baseUserResponse = await _userQueries.GetUserModelByGuidAsync(new Guid(_authContext.CurrentUserId));
                if (baseUserResponse.Result is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USR13C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USR13C), EnumUserErrorCodes.USR13C) }
                    );
                    return methodResult;
                }
                #endregion

                #region Check user remove is match with user create or user remove is admin
                GroupUserMember groupUser = await _groupQueries.GetByIdAsync(_authContext.GroupId).ConfigureAwait(false);
                if (groupUser is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumGroupErrorCodes.GRP04C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.GroupId), _authContext.GroupId) }
                    );
                    return methodResult;
                }
                if (!(storyReview.CreatedUserGuid == new Guid(_authContext.CurrentUserId)) || !Enum.TryParse(groupUser.GroupName, out EnumManage managementName))
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
                await _reviewStoryRepository.DeleteAsync(storyReview);
                await _reviewStoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Delete comments story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Delete comments story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
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
