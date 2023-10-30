using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Categories;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Application.Commands.Base.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Serilog;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
namespace MuonRoiSocialNetwork.Application.Commands.Stories
{
    /// <summary>
    /// Set category request
    /// </summary>
    public class SetCategoryOfStoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id story update
        /// </summary>
        public int StoryId { get; set; }
        /// <summary>
        /// Id of category
        /// </summary>
        public int CategoryId { get; set; }
    }
    /// <summary>
    /// Set favorite story handler
    /// </summary>
    public class SetCategoryOfStoryCommandHandler : BaseStoriesCommandHandler, IRequestHandler<SetCategoryOfStoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<SetCategoryOfStoryCommandHandler> _logger;
        private readonly IStoriesFavoriteRepository _storiesFavoriteRepository;
        private readonly ICategoryQueries _categoryQueries;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        /// <param name="logger"></param>
        /// <param name="storiesFavoriteRepository"></param>
        /// <param name="categoryQueries"></param>
        public SetCategoryOfStoryCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository, ILoggerFactory logger, IStoriesFavoriteRepository storiesFavoriteRepository, ICategoryQueries categoryQueries) : base(mapper, configuration, storiesQuerie, storiesRepository)
        {
            _logger = logger.CreateLogger<SetCategoryOfStoryCommandHandler>();
            _storiesFavoriteRepository = storiesFavoriteRepository;
            _categoryQueries = categoryQueries;
        }
        /// <summary>
        /// Function handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<bool>> Handle(SetCategoryOfStoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                    nameof(EnumCategoriesErrorCode.CTS01),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumCategoriesErrorCode.CTS01), EnumCategoriesErrorCode.CTS01) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #region Check story & category is exist
                Story existStory = await _storiesQuerie.GetByIdAsync(request.StoryId);
                if (existStory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumStoryErrorCode.ST10),
                        new[] { Helpers.GenerateErrorResult(nameof(request.StoryId), request.StoryId) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                CategoryEntities existCategory = await _categoryQueries.GetByIdAsync(request.CategoryId);
                if (existCategory is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumCategoriesErrorCode.CTS02),
                        new[] { Helpers.GenerateErrorResult(nameof(request.CategoryId), request.CategoryId) }
                    );
                    methodResult.Result = false;
                    return methodResult;
                }
                #endregion

                #region Set category to story
                await _storiesRepository.UpdateSingleEntry(existStory, nameof(existStory.CategoryId), request.CategoryId);
                await _storiesFavoriteRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Set category of story) STEP EXEPTION MESSAGE -->{ex} ---->");
                _logger.LogError($" -->(Set category of story) STEP EXEPTION STACK -->{ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                methodResult.Result = false;
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = true;
            return methodResult;
        }
    }
}
