using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Tags;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Tags.Request;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Tags
{
    /// <summary>
    /// Update info tag request
    /// </summary>
    public class UpdateTagCommand : TagModelRequest, IRequest<MethodResult<TagModelResponse>>
    {
        /// <summary>
        /// Id of tag
        /// </summary>
        [JsonProperty("id")]
        public int IdTag { get; set; }
    }
    /// <summary>
    /// Handler update tag
    /// </summary>
    public class UpdateTagCommandCommandHandler : IRequestHandler<UpdateTagCommand, MethodResult<TagModelResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTagCommandCommandHandler> _logger;
        private readonly ITagQueries _tagQueries;
        private readonly ITagRepository _tagRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tagQueries"></param>
        /// <param name="tagRepository"></param>
        /// <param name="mapper"></param>
        public UpdateTagCommandCommandHandler(ILoggerFactory logger, ITagQueries tagQueries, ITagRepository tagRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<UpdateTagCommandCommandHandler>();
            _tagQueries = tagQueries;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handler update tag
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<TagModelResponse>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            MethodResult<TagModelResponse> methodResult = new();
            try
            {
                #region Validation
                Tag tagUpdate = _mapper.Map<Tag>(request);
                if (!tagUpdate.IsValid())
                {
                    throw new CustomException(tagUpdate.ErrorMessages);
                }
                #endregion

                #region Check exist tag by id
                Tag existTag = await _tagQueries.GetByIdAsync(request.IdTag);
                if (existTag == null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumTagsErrorCode.TT08),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumTagsErrorCode.TT08), nameof(EnumTagsErrorCode.TT08)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Update tag
                existTag = _mapper.Map<Tag>(request);
                existTag.Id = request.IdTag;
                _tagRepository.Update(existTag);
                int checkStatus = await _tagRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                if (checkStatus < 1)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumUserErrorCodes.USRC50C),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumUserErrorCodes.USRC50C), nameof(EnumUserErrorCodes.USRC50C)) }
                    );
                    return methodResult;
                }
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update tag command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update tag) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Update tag) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            Tag tagUpdated = await _tagQueries.GetByIdAsync(request.IdTag);
            methodResult.Result = _mapper.Map<TagModelResponse>(tagUpdated);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
