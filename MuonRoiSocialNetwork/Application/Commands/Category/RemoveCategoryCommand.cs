using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Categories;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using Newtonsoft.Json;
using MuonRoi.Social_Network.Users;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Category
{
    /// <summary>
    /// Update info story request
    /// </summary>
    public class RemoveCategoryCommand : IRequest<MethodResult<bool>>
    {
        /// <summary>
        /// Id of category
        /// </summary>
        [JsonProperty("id")]
        public int IdCategory { get; set; }
    }
    /// <summary>
    /// Handler update category
    /// </summary>
    public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand, MethodResult<bool>>
    {
        private readonly ILogger<RemoveCategoryCommandHandler> _logger;
        private readonly ICategoryQueries _categoryQueries;
        private readonly ICategoryRepository _categoryRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryQueries"></param>
        /// <param name="categoryRepository"></param>
        public RemoveCategoryCommandHandler(ILoggerFactory logger, ICategoryQueries categoryQueries, ICategoryRepository categoryRepository)
        {
            _logger = logger.CreateLogger<RemoveCategoryCommandHandler>();
            _categoryQueries = categoryQueries;
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// Function handle remove category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<bool> methodResult = new();
            try
            {
                #region Check exist category by id
                if (request is null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumCategoriesErrorCode.CTS02),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumCategoriesErrorCode.CTS02), nameof(EnumCategoriesErrorCode.CTS02)) }
                    );
                    return methodResult;
                }
                CategoryEntities existCategory = await _categoryQueries.GetByIdAsync(request.IdCategory);
                if (existCategory == null)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumCategoriesErrorCode.CTS02),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumCategoriesErrorCode.CTS02), nameof(EnumCategoriesErrorCode.CTS02)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Remove category
                await _categoryRepository.DeleteAsync(existCategory);
                await _categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

                #endregion
            }
            catch (CustomException ex)
            {

                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Remove category command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                methodResult.Result = false;
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Remove category) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Remove category) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
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
