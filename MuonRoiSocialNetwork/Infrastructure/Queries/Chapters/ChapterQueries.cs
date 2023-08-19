using Amazon.Runtime.Internal.Util;
using Autofac.Core;
using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Chapters;
using MuonRoi.Social_Network.Storys;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;
using MuonRoiSocialNetwork.Common.Settings.StorySettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using MuonRoiSocialNetwork.Infrastructure.Helpers;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Chapters
{
    /// <summary>
    /// Chapter queries
    /// </summary>
    public class ChapterQueries : BaseQuery<Chapter>, IChapterQueries
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="mapper"></param>
        /// <param name="cache"></param>
        public ChapterQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IMapper mapper, IDistributedCache cache) : base(dbContext, authContext, cache, mapper)
        {
        }
        /// <summary>
        /// Get list chapter of story
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="storyId"></param>
        /// <param name="isLatest"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<ChapterPreviewResponse>>> GetListChapterOfStory(long storyId, int pageIndex = 1, int pageSize = 10, bool isLatest = false)
        {
            MethodResult<PagingItemsDTO<ChapterPreviewResponse>> methodResult = new();
            List<Chapter>? cacheQueryChapter = await _cache.GetRecordAsync<List<Chapter>>($"{string.Format(StorySettingDefault.Instance.keyModelResponseChapters, storyId)}");
            if (cacheQueryChapter == null || !cacheQueryChapter.Any())
            {
                cacheQueryChapter = _queryable.AsNoTracking().Where(x => x.StoryId == storyId).Select(x => x).ToList();
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelResponseChapters, storyId)}", cacheQueryChapter, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            }
            IQueryable<Chapter> queryChapter = cacheQueryChapter.AsQueryable() ?? _queryable.AsNoTracking().Where(x => x.StoryId == storyId).Select(x => x);
            queryChapter = isLatest ? queryChapter.OrderByDescending(x => x.CreatedDateTS).Take(5) : queryChapter;
            queryChapter = queryChapter.OrderByDescending(x => x.NumberOfChapter);
            if (queryChapter == null || !queryChapter.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            PagingItemsDTO<Chapter> pagingTagItemsDTO = await GetListPaging(queryChapter, pageIndex, pageSize).ConfigureAwait(false);
            IEnumerable<Chapter> resultListChapter = _mapper.Map<IEnumerable<Chapter>>(pagingTagItemsDTO.Items);
            IEnumerable<ChapterPreviewResponse> listChapterAndId = resultListChapter.Select(x => new ChapterPreviewResponse
            {
                ChapterId = x.Id,
                NumberOfChapter = x.NumberOfChapter,
                ChapterName = x.ChapterTitle
            });
            if (listChapterAndId == null || !listChapterAndId.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = new PagingItemsDTO<ChapterPreviewResponse>
            {
                Items = isLatest ? listChapterAndId : listChapterAndId.OrderBy(x => x.NumberOfChapter),
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }
        /// <summary>
        /// Get group chapter each view time
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fromChapterId"></param>
        /// <param name="isSetCache"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<PagingItemsDTO<ChapterModelResponse>>> GetGroupChapterAsync(long storyId, int fromChapterId = 0, int pageIndex = 1, int pageSize = 20, bool isSetCache = false)
        {
            MethodResult<PagingItemsDTO<ChapterModelResponse>> methodResult = new();
            //List<Chapter>? cacheQueryChapter = await _cache.GetRecordAsync<List<Chapter>>($"{string.Format(StorySettingDefault.Instance.keyModelResponseChapters, storyId)}");
            var chapterNumber = _queryable.AsNoTracking().FirstOrDefault(x => x.Id == fromChapterId)?.NumberOfChapter;
            //if (isSetCache && (cacheQueryChapter == null || !cacheQueryChapter.Any()))
            //{
            //    cacheQueryChapter = fromChapterId == 0 ? _queryable.Where(x => x.StoryId == storyId).Select(x => x).ToList() : _queryable.Where(x => x.StoryId == storyId && x.NumberOfChapter >= chapterNumber).Select(x => x).ToList();
            //    await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelResponseChapters, storyId)}", cacheQueryChapter, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            //}
            IQueryable<Chapter> queryChapter = /*cacheQueryChapter?.AsQueryable() ??*/ fromChapterId == 0 ? _queryable.AsNoTracking().Where(x => x.StoryId == storyId).Select(x => x)
                : _queryable.AsNoTracking().Where(x => x.StoryId == storyId && x.NumberOfChapter >= chapterNumber).Select(x => x);
            if (queryChapter == null || !queryChapter.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            PagingItemsDTO<Chapter> pagingTagItemsDTO = await GetListPaging(queryChapter, pageIndex, pageSize).ConfigureAwait(false);
            IEnumerable<Chapter> resultListChapter = _mapper.Map<IEnumerable<Chapter>>(pagingTagItemsDTO.Items);
            IEnumerable<ChapterModelResponse> resultGroupChapter = _mapper.Map<IEnumerable<ChapterModelResponse>>(resultListChapter);
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = new PagingItemsDTO<ChapterModelResponse>
            {
                Items = resultGroupChapter.OrderBy(x => int.Parse(x.NumberOfChapter)),
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }
        /// <summary>
        /// Get total chapter by id story
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<MethodResult<int>> GetTotalChapterOfStoryIdAsync(long storyId)
        {
            MethodResult<int> methodResult = new();
            int? cacheChapterTotal = await _cache.GetRecordAsync<int>($"{string.Format(StorySettingDefault.Instance.keyModelResponseTotalChapters, storyId)}");
            if (cacheChapterTotal == 0)
            {
                cacheChapterTotal = _queryable.AsNoTracking().Where(x => x.StoryId == storyId).Select(x => x).Count();
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelResponseTotalChapters, storyId)}",
                    cacheChapterTotal, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            }
            methodResult.Result = cacheChapterTotal.Value;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// Get all chapter latest paging
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isSetCache"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<ChapterModelResponse>>> GetAllChapterAsync(int pageIndex, int pageSize, bool isSetCache = false)
        {
            MethodResult<PagingItemsDTO<ChapterModelResponse>> methodResult = new();
            IQueryable<Chapter> queryChapter = _queryable.AsNoTracking().Select(x => x).OrderByDescending(x => x.CreatedDateTS);
            if (queryChapter == null || !queryChapter.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            PagingItemsDTO<Chapter> pagingTagItemsDTO = await GetListPaging(queryChapter, pageIndex, pageSize).ConfigureAwait(false);
            IEnumerable<Chapter> resultListChapter = _mapper.Map<IEnumerable<Chapter>>(pagingTagItemsDTO.Items);
            IEnumerable<ChapterModelResponse> resultGroupChapter = _mapper.Map<IEnumerable<ChapterModelResponse>>(resultListChapter);
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = new PagingItemsDTO<ChapterModelResponse>
            {
                Items = resultGroupChapter.OrderBy(x => int.Parse(x.NumberOfChapter)),
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }

    }
}
