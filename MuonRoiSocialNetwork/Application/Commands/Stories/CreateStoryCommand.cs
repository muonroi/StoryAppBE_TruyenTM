using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.Extentions.Image;
using BaseConfig.Extentions.String;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Categories;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Common.Models.Stories.Request;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Serilog;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Create story command
    /// </summary>
    public class CreateStoryCommand : StoryModelRequest, IRequest<MethodResult<StoryModelResponse>>
    { }
    /// <summary>
    /// Handle create story
    /// </summary>
    public class CreateStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<CreateStoryCommand, MethodResult<StoryModelResponse>>
    {
        private readonly ILogger<CreateStoryCommandHandler> _logger;
        private readonly AuthContext _authContext;
        private readonly ICategoryQueries _categoryQueries;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="authContext"></param>
        /// <param name="categoryQueries"></param>
        public CreateStoryCommandHandler(ILoggerFactory logger, IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, AuthContext authContext, ICategoryQueries categoryQueries) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<CreateStoryCommandHandler>();
            _authContext = authContext;
            _categoryQueries = categoryQueries;
        }
        /// <summary>
        /// Function handle
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<StoryModelResponse>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<StoryModelResponse> methodResult = new();
            try
            {
                #region Validation
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST00),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername) }
                    );
                    return methodResult;
                }
                Story newStory = _mapper.Map<Story>(request);
                newStory.AuthorName = request.AuthorName;
                newStory.Slug = StringManagers.GenerateSlug(request.StoryTitle);
                newStory.ImgUrl = "img";
                if (!newStory.IsValid())
                {
                    throw new CustomException(newStory.ErrorMessages);
                }
                #endregion
                #region Check category
                CategoryEntities category = await _categoryQueries.GetByIdAsync(request.CategoryId);
                if (category is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumCategoriesErrorCode.CTS02),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumCategoriesErrorCode.CTS02), request.CategoryId) }
                    );
                    return methodResult;
                }
                #endregion

                #region upload avatar
                if (request.AvatarTemp != null)
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
                    newStory.ImgUrl = result.Keys.FirstOrDefault() ?? "";
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
                #endregion

                #region Create new story
                _storiesRepository.Add(newStory);
                int resultSave = await _storiesRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);//save
                if (resultSave < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC51C),
                        new[] { Helpers.GenerateErrorResult(nameof(_authContext.CurrentUsername), _authContext.CurrentUsername ?? "") }
                    );
                    return methodResult;
                }
                #endregion

                methodResult.Result = _mapper.Map<StoryModelResponse>(await _storiesQuerie.GetByGuidAsync(newStory.Guid));
                methodResult.Result.ImgUrl = HandlerImages.TakeLinkImage(_configuration, newStory.ImgUrl);

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Create story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
