using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Categories;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Common.Models.Tags.Response;
using MuonRoiSocialNetwork.Common.Settings.StorySettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using MuonRoiSocialNetwork.Infrastructure.Helpers;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.TagsAndTagInStories
{
    /// <summary>
    /// Define queries class
    /// </summary>
    public class TagQueries : BaseQuery<Tag>, ITagQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="auth"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public TagQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext auth, IMapper mapper, IDistributedCache cache) : base(dbContext, auth, cache, mapper)
        {
        }
        /// <summary>
        /// Handle queries get all
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<PagingItemsDTO<TagModelResponse>>> GetAllTag(int pageIndex = 1, int pageSize = 10)
        {
            MethodResult<PagingItemsDTO<TagModelResponse>> methodResult = new();
            List<Tag>? cacheTagInfo = await _cache.GetRecordAsync<List<Tag>>($"{StorySettingDefault.Instance.keyQueryableResponseTags}");
            if (cacheTagInfo == null || !cacheTagInfo.Any())
            {
                cacheTagInfo = _queryable.AsNoTracking().Select(x => x).ToList();
                await _cache.SetRecordAsync($"{StorySettingDefault.Instance.keyQueryableResponseTags}", cacheTagInfo, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            }
            if (pageSize > 30)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumTagsErrorCode.TT10),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(pageSize), pageSize) });
                return methodResult;
            }
            IQueryable<Tag> queryTag = cacheTagInfo.AsQueryable() ?? _queryable.AsNoTracking().Select(x => x);
            PagingItemsDTO<Tag> pagingTagItemsDTO = await GetListPaging(queryTag, pageIndex, pageSize).ConfigureAwait(false);
            IEnumerable<TagModelResponse> resultTags = _mapper.Map<IEnumerable<TagModelResponse>>(pagingTagItemsDTO.Items);
            methodResult.Result = new PagingItemsDTO<TagModelResponse>
            {
                Items = resultTags,
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }
        /// <summary>
        /// Handle queries get special by tag id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MethodResult<TagModelResponse>> GetTagById(int id)
        {
            MethodResult<TagModelResponse> methodResult = new();
            Tag? tagResultRaw = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (tagResultRaw is null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumTagsErrorCode.TT08),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumTagsErrorCode.TT08), EnumTagsErrorCode.TT08) }
                );
                return methodResult;
            }
            TagModelResponse tagDataResponse = _mapper.Map<TagModelResponse>(tagResultRaw);
            methodResult.Result = tagDataResponse;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// Handle queries get special by tag name
        /// </summary>
        /// <param name="nameTag"></param>
        /// <returns></returns>
        public async Task<MethodResult<bool>> GetTagByName(string nameTag)
        {
            MethodResult<bool> methodResult = new()
            {
                Result = await _queryable.AsNoTracking().AnyAsync(x => x != null && x.TagName != null && x.TagName.ToLower().Trim() == nameTag.ToLower().Trim()),
                StatusCode = StatusCodes.Status200OK
            };
            return methodResult;
        }
    }
}
