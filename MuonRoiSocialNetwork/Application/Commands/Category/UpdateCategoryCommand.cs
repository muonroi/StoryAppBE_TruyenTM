using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoi.Social_Network.Categories;
using MuonRoi.Social_Network.Users;
using MuonRoiSocialNetwork.Common.Models.Category.Request;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using Newtonsoft.Json;
using Serilog;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;

namespace MuonRoiSocialNetwork.Application.Commands.Category
{
    /// <summary>
    /// Update info story request
    /// </summary>
    public class UpdateCategoryCommand : CategoryRequest, IRequest<MethodResult<CategoryResponse>>
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
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, MethodResult<CategoryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;
        private readonly ICategoryQueries _categoryQueries;
        private readonly ICategoryRepository _categoryRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryQueries"></param>
        /// <param name="categoryRepository"></param>
        /// <param name="mapper"></param>
        public UpdateCategoryCommandHandler(ILoggerFactory logger, ICategoryQueries categoryQueries, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<UpdateCategoryCommandHandler>();
            _categoryQueries = categoryQueries;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handler update category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<CategoryResponse> methodResult = new();
            try
            {
                #region Validation
                CategoryEntities newCategory = _mapper.Map<CategoryEntities>(request);
                if (!newCategory.IsValid())
                {
                    throw new CustomException(newCategory.ErrorMessages);
                }
                newCategory.Id = request.IdCategory;
                #endregion

                #region Check exist category by id
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

                #region Update category
                _categoryRepository.Update(newCategory);
                int checkStatus = await _categoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
                _logger.LogError($" -->(Update category command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Update category) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Update category) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            CategoryEntities categoryUpdated = await _categoryQueries.GetByIdAsync(request.IdCategory);
            methodResult.Result = _mapper.Map<CategoryResponse>(categoryUpdated);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
