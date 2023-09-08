using BaseConfig.BaseDbContext.BaseQuery;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using BaseConfig.Infrashtructure;
using AutoMapper;
using BaseConfig.MethodResult;
using BaseConfig.BaseDbContext.Common;
using MuonRoiSocialNetwork.Common.Models.Category.Response;
using MuonRoi.Social_Network.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using MuonRoiSocialNetwork.Common.Settings.StorySettings;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Category
{
    /// <summary>
    /// Handle category query
    /// </summary>
    public class CategoryQueries : BaseQuery<CategoryEntities>, ICategoryQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="auth"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public CategoryQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext auth, IMapper mapper, IDistributedCache cache) : base(dbContext, auth, cache, mapper)
        {
        }
        /// <summary>
        /// Handle get all category
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<CategoryResponse>>> GetAllCategory(int pageIndex = 1, int pageSize = 10)
        {
            MethodResult<PagingItemsDTO<CategoryResponse>> methodResult = new();
            List<CategoryEntities>? cacheQueryCategory = await _cache.GetRecordAsync<List<CategoryEntities>>($"{StorySettingDefault.Instance.keyQueryableResponseCategorys}");
            if (cacheQueryCategory == null || !cacheQueryCategory.Any())
            {
                cacheQueryCategory = _queryable.AsNoTracking().Select(x => x).ToList();
                await _cache.SetRecordAsync($"{StorySettingDefault.Instance.keyQueryableResponseCategorys}", cacheQueryCategory, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            }
            if (pageSize > 100)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumCategoriesErrorCode.CTS03),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(pageSize), pageSize) });
                return methodResult;
            }
            IQueryable<CategoryEntities> queryCategory = cacheQueryCategory.AsQueryable() ?? _queryable.AsNoTracking().Select(x => x);
            PagingItemsDTO<CategoryEntities> pagingTagItemsDTO = await GetListPaging(queryCategory, pageIndex, pageSize).ConfigureAwait(false);
            IEnumerable<CategoryResponse> resultCategory = _mapper.Map<IEnumerable<CategoryResponse>>(pagingTagItemsDTO.Items);
            methodResult.Result = new PagingItemsDTO<CategoryResponse>
            {
                Items = resultCategory,
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }
        /// <summary>
        /// Handle get category by id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<MethodResult<CategoryResponse>> GetCategoryById(int categoryId)
        {
            MethodResult<CategoryResponse> methodResult = new();
            CategoryEntities? tagResultRaw = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == categoryId);
            if (tagResultRaw is null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumCategoriesErrorCode.CTS02),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumCategoriesErrorCode.CTS02), EnumCategoriesErrorCode.CTS02) }
                );
                return methodResult;
            }
            CategoryResponse tagDataResponse = _mapper.Map<CategoryResponse>(tagResultRaw);
            methodResult.Result = tagDataResponse;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// Handle get category by name
        /// </summary>
        /// <param name="nameCategory"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> GetCategoryByName(string nameCategory)
        {
            MethodResult<bool> methodResult = new()
            {
                Result = await _queryable.AsNoTracking().AnyAsync(x => x != null && x.NameCategory != null && x.NameCategory.ToLower().Trim() == nameCategory.ToLower().Trim()),
                StatusCode = StatusCodes.Status200OK
            };
            return methodResult;
        }
    }
}