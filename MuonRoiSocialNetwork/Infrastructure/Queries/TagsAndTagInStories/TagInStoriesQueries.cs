using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Common.Enums.Tags;
using MuonRoiSocialNetwork.Common.Models.TagInStories.Response;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using System.Collections.Generic;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.TagsAndTagInStories
{
    /// <summary>
    /// Define action queries class
    /// </summary>
    public class TagInStoriesQueries : BaseQuery<TagInStory>, ITagInStoriesQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="auth"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public TagInStoriesQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext auth, IMapper mapper, IDistributedCache cache) : base(dbContext, auth, cache, mapper)
        {
        }
        /// <summary>
        /// Handle get tag in story by idTag and storyId
        /// </summary>
        /// <param name="idTag"></param>
        /// <param name="storyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<TagInStoriesModelResponse>> GetTagById(int idTag, int storyId)
        {
            MethodResult<TagInStoriesModelResponse> methodResult = new();
            TagInStory? tagInStoryResult = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.TagId == idTag && x.StoryId == storyId);
            if (tagInStoryResult is null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumTagInStoryErrorCode.TIS01),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumTagInStoryErrorCode.TIS01), EnumTagInStoryErrorCode.TIS01) }
                );
                return methodResult;
            }
            TagInStoriesModelResponse tagInStoriesModelResponseResult = _mapper.Map<TagInStoriesModelResponse>(tagInStoryResult);
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = tagInStoriesModelResponseResult;
            return methodResult;
        }
        /// <summary>
        /// Get all tag in story
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<MethodResult<List<TagInStoriesModelResponse>>> GetAllTagInStory(int pageSize, int pageIndex)
        {
            MethodResult<List<TagInStoriesModelResponse>> methodResult = new();
            List<TagInStory> tagInStoryResult = await _queryable.AsNoTracking().Select(x => x).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            if (tagInStoryResult is null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumTagInStoryErrorCode.TIS01),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumTagInStoryErrorCode.TIS01), EnumTagInStoryErrorCode.TIS01) }
                );
                return methodResult;
            }
            List<TagInStoriesModelResponse> tagInStoriesModelResponseResult = _mapper.Map<List<TagInStoriesModelResponse>>(tagInStoryResult);
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = tagInStoriesModelResponseResult;
            return methodResult;
        }
    }
}
