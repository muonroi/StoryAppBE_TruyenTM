using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Enums.Tags;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Request;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Tags
{
    /// <summary>
    /// Create tag in story request
    /// </summary>
    public class UpdateTagInStoryCommand : TagInStoriesModelRequest, IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id tag in story
        /// </summary>
        [JsonProperty("id")]
        public int IdTagInStory { get; set; }
    }
    /// <summary>
    /// Handler update TagInStory
    /// </summary>
    public class UpdateTagInStoryCommandHandler : IRequestHandler<UpdateTagInStoryCommand, MethodResult<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTagInStoryCommandHandler> _logger;
        private readonly ITagInStoriesQueries _tagInStoriesQueries;
        private readonly ITagInStoryRepository _tagInStoryRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tagInStoriesQueries"></param>
        /// <param name="tagInStoryRepository"></param>
        /// <param name="mapper"></param>
        public UpdateTagInStoryCommandHandler(ILoggerFactory logger, ITagInStoriesQueries tagInStoriesQueries, ITagInStoryRepository tagInStoryRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<UpdateTagInStoryCommandHandler>();
            _tagInStoriesQueries = tagInStoriesQueries;
            _tagInStoryRepository = tagInStoryRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handler update TagInStory
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(UpdateTagInStoryCommand request, CancellationToken cancellationToken)
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
                existTagInStory.Id = request.IdTagInStory;
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

                #region Update TagInStory
                existTagInStory = _mapper.Map<TagInStory>(request);
                _tagInStoryRepository.Update(existTagInStory);
                int checkStatus = await _tagInStoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (checkStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC50C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC50C), nameof(EnumUserErrorCodes.USRC50C)) }
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
                _logger.LogError($" -->(Update TagInStory command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update TagInStory) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Update TagInStory) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
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
