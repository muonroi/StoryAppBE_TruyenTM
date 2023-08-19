using AutoMapper;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Exeptions;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
using BaseConfig.MethodResult;
using MediatR;
using MuonRoiSocialNetwork.Common.Models.Category.Request;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using MuonRoi.Social_Network.Users;
using MuonRoi.Social_Network.Categories;
using Serilog;

namespace MuonRoiSocialNetwork.Application.Commands.Category
{
    /// <summary>
    /// Request create new category
    /// </summary>
    public class CreateCategoryCommand : CategoryRequest, IRequest<MethodResult<List<CategoryResponse>>>
    {

    }
    /// <summary>
    /// Handler create new category
    /// </summary>
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, MethodResult<List<CategoryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly ICategoryQueries _categoryQueries;
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryQueries"></param>
        /// <param name="categoryRepository"></param>
        /// <param name="mapper"></param>
        public CreateCategoryCommandHandler(ILoggerFactory logger, ICategoryQueries categoryQueries, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger.CreateLogger<CreateCategoryCommandHandler>();
            _categoryQueries = categoryQueries;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Function handler create category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<List<CategoryResponse>>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            MethodResult<List<CategoryResponse>> methodResult = new();
            try
            {
                #region Validation
                CategoryEntities newCategory = _mapper.Map<CategoryEntities>(request);
                if (!newCategory.IsValid())
                {
                    throw new CustomException(newCategory.ErrorMessages);
                }
                #endregion

                #region Check exist category by name
                var isExistCategory = await _categoryQueries.GetCategoryByName(request.NameCategory ?? string.Empty);
                if (isExistCategory.Result)
                {
                    methodResult.StatusCode = StatusCodes.Status400BadRequest;
                    methodResult.AddApiErrorMessage(
                        nameof(EnumCategoriesErrorCode.CTS04),
                        new[] { Helpers.GenerateErrorResult(nameof(EnumCategoriesErrorCode.CTS04), nameof(EnumCategoriesErrorCode.CTS04)) }
                    );
                    return methodResult;
                }
                #endregion

                #region Create new category
                _categoryRepository.Add(newCategory);
                int checkStatus = await _categoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
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
                _logger.LogError($" -->(Create category command) STEP CUSTOMEXCEPTION --> {ex} ---->");
                methodResult.AddResultFromErrorList(ex.ErrorMessages);
                return methodResult;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                _logger.LogError($" -->(Create category) STEP EXEPTION MESSAGE --> {ex} ---->");
                _logger.LogError($" -->(Create category) STEP EXEPTION STACK --> {ex.StackTrace} ---->");
                methodResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace ?? "");
                return methodResult;
            }
            List<CategoryEntities> listCategory = await _categoryQueries.GetAllAsync();
            methodResult.Result = _mapper.Map<List<CategoryResponse>>(listCategory);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
