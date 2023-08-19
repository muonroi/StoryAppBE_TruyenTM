using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Enums.Storys;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Common.Settings.StorySettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Infrastructure.Helpers;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Stories.Review
{
    /// <summary>
    /// Review story queries
    /// </summary>
    public class ReviewStoryQueries : BaseQuery<StoryReview>, IReviewStoryQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="cache"></param>
        /// <param name="mapper"></param>
        public ReviewStoryQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IDistributedCache cache, IMapper mapper) : base(dbContext, authContext, cache, mapper)
        {
        }
        /// <summary>
        /// Get list comments of story
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<StoryReviewModelResponse>>> GetListCommentsOfStory(int storyId, int pageIndex = 1, int pageSize = 10)
        {
            MethodResult<PagingItemsDTO<StoryReviewModelResponse>> methodResult = new();
            List<StoryReview>? cacheQueryComments = await _cache.GetRecordAsync<List<StoryReview>>($"{string.Format(StorySettingDefault.Instance.keyQueryableResponseComments, storyId)}");
            if (cacheQueryComments == null || !cacheQueryComments.Any())
            {
                cacheQueryComments = _queryable.AsNoTracking().Where(x => x.Id == storyId).Select(x => x).ToList();
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyQueryableResponseComments, storyId)}", cacheQueryComments, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            }
            IQueryable<StoryReview> queryComments = cacheQueryComments.AsQueryable() ?? _queryable.AsNoTracking().Where(x => x.Id == storyId).Select(x => x);
            if (queryComments == null || !queryComments.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumReviewStorys.RVST04),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumReviewStorys.RVST04), EnumReviewStorys.RVST04) }
                );
                return methodResult;
            }
            PagingItemsDTO<StoryReview> pagingTagItemsDTO = await GetListPaging(queryComments, pageIndex, pageSize).ConfigureAwait(false);
            IEnumerable<StoryReviewModelResponse> resultListComments = _mapper.Map<IEnumerable<StoryReviewModelResponse>>(pagingTagItemsDTO.Items);
            if (resultListComments == null || !resultListComments.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumReviewStorys.RVST04),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumReviewStorys.RVST04), nameof(EnumReviewStorys.RVST04)) }
                );
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = new PagingItemsDTO<StoryReviewModelResponse>
            {
                Items = resultListComments,
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }
    }
}
