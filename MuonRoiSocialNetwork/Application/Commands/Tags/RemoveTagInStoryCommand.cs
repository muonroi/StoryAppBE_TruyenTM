using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Enums.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Tags
{
    /// <summary>
    /// Remove tag in story request
    /// </summary>
    public class RemoveTagInStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id tag in story
        /// </summary>
        [JsonProperty("id")]
        public int IdTagInStory { get; set; }
    }
    /// <summary>
    /// Handler remove TagInStory
    /// </summary>
    public class RemoveTagInStoryCommandHandler : IRequestHandler<RemoveTagInStoryCommand, MethodResult<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveTagInStoryCommandHandler> _logger;
        private readonly ITagInStoriesQueries _tagInStoriesQueries;
        private readonly ITagInStoryRepository _tagInStoryRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tagInStoriesQueries"></param>
        /// <param name="tagInStoryRepository"></param>
        /// <param name="mapper"></param>
        public RemoveTagInStoryCommandHandler(ILoggerFactory logger, ITagInStoriesQueries tagInStoriesQueries, ITagInStoryRepository tagInStoryRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<RemoveTagInStoryCommandHandler>();
            _tagInStoriesQueries = tagInStoriesQueries;
            _tagInStoryRepository = tagInStoryRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handler remove TagInStory
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(RemoveTagInStoryCommand request, CancellationToken cancellationToken)
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

                #region Check exist TagInStory by id
                TagInStory existTagInStory = await _tagInStoriesQueries.GetByIdAsync(request.IdTagInStory);
                if (existTagInStory == null)
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

                #region remove TagInStory
                await _tagInStoryRepository.DeleteAsync(existTagInStory);
                await _tagInStoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Remove TagInStory command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Remove TagInStory) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Remove TagInStory) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
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
