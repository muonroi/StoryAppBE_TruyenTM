using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Application.Commands.Category;
using MuonRoiSocialNetwork.Common.Models.Tags.Request;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Tags
{
    /// <summary>
    /// Request create new tag
    /// </summary>
    public class CreateTagCommand : TagModelRequest, IRequest<MethodResult<List<TagModelResponse>>>
    { }
    /// <summary>
    /// Handler create new tag
    /// </summary>
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, MethodResult<List<TagModelResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly ITagQueries _tagQueries;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tagQueries"></param>
        /// <param name="tagRepository"></param>
        /// <param name="mapper"></param>
        public CreateTagCommandHandler(ILoggerFactory logger, ITagQueries tagQueries, ITagRepository tagRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<CreateCategoryCommandHandler>();
            _tagQueries = tagQueries;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handle create new tag
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<List<TagModelResponse>>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            MethodResult<List<TagModelResponse>> methodResult = new();
            try
            {
                #region Validation
                Tag newTag = _mapper.Map<Tag>(request);
                if (!newTag.IsValid())
                {
                    throw new CustomException(newTag.ErrorMessages);
                }
                #endregion

                #region Check exist tag by name
                var isExistTag = await _tagQueries.GetTagByName(request.TagName ?? string.Empty);
                if (isExistTag.Result)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumTagsErrorCode.TT05),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumTagsErrorCode.TT05), nameof(EnumTagsErrorCode.TT05)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Create new tag
                _tagRepository.Add(newTag);
                int checkStatus = await _tagRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (checkStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC51C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC51C), nameof(EnumUserErrorCodes.USRC51C)) }
                    );
                    return methodResult;
                }
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create tag command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create tag) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Create tag) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            List<Tag> listTags = await _tagQueries.GetAllAsync();
            methodResult.Result = _mapper.Map<List<TagModelResponse>>(listTags);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
