using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Enums.Tags;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Request;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Tags
{
    /// <summary>
    /// Create tag in story request
    /// </summary>
    public class CreateTagInStoryCommand : TagInStoriesModelRequest, IRequest<MethodResult<bool>>
    { }
    /// <summary>
    /// Handler create new tag
    /// </summary>
    public class CreateTagInStoryCommandHandler : IRequestHandler<CreateTagInStoryCommand, MethodResult<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTagInStoryCommandHandler> _logger;
        private readonly ITagInStoriesQueries _tagInStoriesQueries;
        private readonly ITagInStoryRepository _tagInStoryRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tagInStoriesQueries"></param>
        /// <param name="tagInStoryRepository"></param>
        /// <param name="mapper"></param>
        public CreateTagInStoryCommandHandler(ILoggerFactory logger, ITagInStoriesQueries tagInStoriesQueries, ITagInStoryRepository tagInStoryRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<CreateTagInStoryCommandHandler>();
            _tagInStoriesQueries = tagInStoriesQueries;
            _tagInStoryRepository = tagInStoryRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handle create new tag in story
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> Handle(CreateTagInStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                #region Validation
                TagInStory newTagInStory = _mapper.Map<TagInStory>(request);
                if (!newTagInStory.IsValid())
                {
                    throw new CustomException(newTagInStory.ErrorMessages);
                }
                #endregion

                #region Check exist tag in story by name
                MethodResult<TagInStoriesModelResponse> existTagInStory = await _tagInStoriesQueries.GetTagById(request.TagId, request.StoryId);
                if (existTagInStory.Result == null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumTagInStoryErrorCode.TIS01),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumTagInStoryErrorCode.TIS01), nameof(EnumTagInStoryErrorCode.TIS01)) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Create new tag in story
                _tagInStoryRepository.Add(newTagInStory);
                int checkStatus = await _tagInStoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (checkStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC51C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC51C), nameof(EnumUserErrorCodes.USRC51C)) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create tag in story command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create tag in story) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Create tag in story) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
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
