using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Tags;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using Newtonsoft.Json;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Tags
{
    /// <summary>
    /// Remove command tag request
    /// </summary>
    public class RemoveTagCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id of tag
        /// </summary>
        [JsonProperty("id")]
        public int Idtag { get; set; }
    }
    /// <summary>
    /// Handler update tag
    /// </summary>
    public class RemoveTagCommandCommandHandler : IRequestHandler<RemoveTagCommand, MethodResult<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveTagCommandCommandHandler> _logger;
        private readonly ITagQueries _tagQueries;
        private readonly ITagRepository _tagRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tagQueries"></param>
        /// <param name="tagRepository"></param>
        /// <param name="mapper"></param>
        public RemoveTagCommandCommandHandler(ILoggerFactory logger, ITagQueries tagQueries, ITagRepository tagRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<RemoveTagCommandCommandHandler>();
            _tagQueries = tagQueries;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handle remove tag
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> Handle(RemoveTagCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumTagsErrorCode.TT08),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumTagsErrorCode.TT08), EnumTagsErrorCode.TT08) }
                    );
                    return methodResult;
                }
                #region Check exist tag by id
                Tag existTag = await _tagQueries.GetByIdAsync(request.Idtag);
                if (existTag == null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumTagsErrorCode.TT08),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumTagsErrorCode.TT08), nameof(EnumTagsErrorCode.TT08)) }
                    );
                    return methodResult;
                }
                existTag.Id = request.Idtag;
                #endregion

                #region Remove tag
                await _tagRepository.DeleteAsync(existTag);
                await _tagRepository.UnitOfWork
                      .SaveEntitiesAsync(cancellationToken)
                      .ConfigureAwait(false);
                #endregion
            }
            catch (CustomException ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Remove tag command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Remove tag) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Remove tag) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
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
