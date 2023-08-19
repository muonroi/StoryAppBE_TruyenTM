using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Extentions.Image;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Stories.Request;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Command request
    /// </summary>
    public class UpdateStoryCommand : StoryModelRequest, IRequest<MethodResult<StoryModelResponse>>
    {
        /// <summary>
        /// Guid story update
        /// </summary>
        public Guid StoryGuid { get; set; }
    }
    /// <summary>
    /// Handle update story
    /// </summary>
    public class UpdateStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<UpdateStoryCommand, MethodResult<StoryModelResponse>>
    {
        private readonly ILogger<UpdateStoryCommandHandler> _logger;
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
        public UpdateStoryCommandHandler(ILoggerFactory logger, IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, AuthContext authContext) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<UpdateStoryCommandHandler>();
            _authContext = authContext;
        }
        /// <summary>
        /// Function handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<StoryModelResponse>> Handle(UpdateStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<StoryModelResponse> methodResult = new();
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
                Story isExistStory = await _storiesQuerie.GetByGuidAsync(request.StoryGuid);
                if (isExistStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryGuid), request.StoryGuid.ToString() ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                #region Validation
                Story updateStory = _mapper.Map<Story>(request);
                string normalizedInput = NormalizeString(request.StoryTitle);
                updateStory.Slug = _slugHelper.GenerateSlug(normalizedInput);
                #endregion

                #region upload avatar
                if (request.AvatarTemp is not null)
                {
                    Dictionary<string, string> result = await HandlerImages.UploadImageToAwsAsync(_configuration, request.AvatarTemp);
                    if (!result.Any() || !result.Keys.Any())
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumUserErrorCodes.USRC41C),
                            new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                        );
                        return methodResult;
                    }
                    updateStory.ImgUrl = result.Keys.FirstOrDefault() ?? "";
                    if (result.Values.Equals("OK"))
                    {
                        methodResult.StatusCode = StatusCodes.Status400BadRequest;
                        methodResult.AddApiErrorMessage(
                            nameof(EnumUserErrorCodes.USRC41C),
                            new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                        );
                        return methodResult;
                    }
                }
                if (!updateStory.IsValid())
                {
                    throw new CustomException(updateStory.ErrorMessages);
                }
                #endregion

                #region Update info story
                _storiesRepository.Update(updateStory);
                int resultUpdate = await _storiesRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (resultUpdate < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC50C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                methodResult.Result = _mapper.Map<StoryModelResponse>(await _storiesQuerie.GetByGuidAsync(updateStory.Guid));
                methodResult.Result.ImgUrl = HandlerImages.TakeLinkImage(_configuration, updateStory.ImgUrl);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Update story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
