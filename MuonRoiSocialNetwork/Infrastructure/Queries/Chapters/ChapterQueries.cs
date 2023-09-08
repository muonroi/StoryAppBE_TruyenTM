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
using System.Text;

namespace MuonRoiSocialNetwork.Infrastructure.Queries.Chapters
{
    /// <summary>
    /// Chapter queries
    /// </summary>
    public class ChapterQueries : BaseQuery<Chapter>, IChapterQueries
    {
        private readonly int _chunkSize = 900;
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

            queryChapter = isLatest ? queryChapter.OrderByDescending(x => x.CreatedDateTS).Take(5) : queryChapter.OrderBy(x => x.NumberOfChapter);
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
        /// <param name="fromChapterId"></param>
        /// <param name="toChapterId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<IEnumerable<ChapterModelResponse>>> GetGroupChapterAsync(long storyId, long fromChapterId = 0, long toChapterId = 0)
        {
            MethodResult<IEnumerable<ChapterModelResponse>> methodResult = new();
            var fromChapterNumber = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fromChapterId) ?? null;
            var toChapterNumber = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == toChapterId) ?? null;
            long fromChapter = fromChapterNumber == null ? 1 : fromChapterNumber.NumberOfChapter;
            long toChapter = toChapterNumber == null ? 100 : toChapterNumber.NumberOfChapter;
            List<Chapter>? cacheChapterPaging = await _cache.GetRecordAsync<List<Chapter>>($"{string.Format(StorySettingDefault.Instance.keyModelResponseTotalChaptersPagingByStory, storyId, fromChapter, toChapter)}");
            var chapterInfo = cacheChapterPaging is null || !cacheChapterPaging.Any() ? await _queryable.AsNoTracking().Where(x => x.StoryId == storyId && x.NumberOfChapter >= fromChapter && x.NumberOfChapter <= toChapter).Select(x => x).ToListAsync() : cacheChapterPaging;

            if (cacheChapterPaging is null || !cacheChapterPaging.Any())
            {
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelResponseTotalChaptersPagingByStory, storyId, fromChapter, toChapter)}",
                   chapterInfo, StorySettingDefault.Instance.expirationTimeModelAllStories, StorySettingDefault.Instance.slidingExpirationModelAllStories);
            }
            chapterInfo = chapterInfo.OrderBy(x => x.NumberOfChapter).ToList();
            if (chapterInfo == null || !chapterInfo.Any())
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            IEnumerable<ChapterModelResponse> resultGroupChapter = _mapper.Map<IEnumerable<ChapterModelResponse>>(chapterInfo);
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = resultGroupChapter;
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
                Items = resultGroupChapter.OrderBy(x => x.NumberOfChapter),
                PagingInfo = pagingTagItemsDTO.PagingInfo
            };
            return methodResult;
        }
        /// <summary>
        /// Get first and last chapter 
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<MethodResult<Dictionary<long, long>>> GetFirstAndLastChapterByStory(long storyId)
        {
            MethodResult<Dictionary<long, long>> methodResult = new();
            var chapterList = await _queryable.AsNoTracking().Where(x => x.StoryId == storyId).Select(x => x).OrderBy(x => x.NumberOfChapter).ToListAsync();
            var fistChapter = chapterList.FirstOrDefault();
            var lastChapter = chapterList.LastOrDefault();
            if (fistChapter == null || lastChapter == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = new Dictionary<long, long>
            {
                { fistChapter.Id, lastChapter.Id}
            };
            return methodResult;
        }
        /// <summary>
        /// Get next chapter
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public async Task<MethodResult<ChapterModelResponse>> NextChapterByStoryId(int storyId, long chapterId)
        {
            MethodResult<ChapterModelResponse> methodResult = new();
            var chapterNumber = _queryable.AsNoTracking().FirstOrDefault(x => x.Id == chapterId)?.NumberOfChapter;
            Chapter? cacheChapter = await _cache.GetRecordAsync<Chapter>($"{string.Format(StorySettingDefault.Instance.keyModelNextResponseChaptersByStory, chapterId)}");
            var chapterResult = cacheChapter is null ? await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.StoryId == storyId && x.NumberOfChapter == chapterNumber + 1) : cacheChapter;
            if (cacheChapter is null)
            {
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelNextResponseChaptersByStory, chapterId)}",
                   chapterResult, StorySettingDefault.Instance.expirationTimeModelAllStories, StorySettingDefault.Instance.slidingExpirationModelAllStories);
            }
            if (chapterResult == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            ChapterModelResponse resultGroupChapter = _mapper.Map<ChapterModelResponse>(chapterResult);
            List<string> chunksContent = SplitToChunks(chapterResult.Body, _chunkSize).ToList();
            chunksContent.RemoveAll(x => x is null);
            resultGroupChapter.BodyChunk = chunksContent;
            resultGroupChapter.ChunkSize = chunksContent.Count;
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = resultGroupChapter;
            return methodResult;
        }
        /// <summary>
        /// Get previous chapter
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public async Task<MethodResult<ChapterModelResponse>> PreviousChapterByStoryId(int storyId, long chapterId)
        {
            MethodResult<ChapterModelResponse> methodResult = new();
            Chapter? cacheChapter = await _cache.GetRecordAsync<Chapter>($"{string.Format(StorySettingDefault.Instance.keyModelPreviousResponseChaptersByStory, chapterId)}");
            var chapterNumber = _queryable.AsNoTracking().FirstOrDefault(x => x.Id == chapterId)?.NumberOfChapter;
            var chapterResult = cacheChapter is null ? await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.StoryId == storyId && x.NumberOfChapter == chapterNumber - 1) : cacheChapter;
            if (cacheChapter is null)
            {
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelPreviousResponseChaptersByStory, chapterId)}",
                   chapterResult, StorySettingDefault.Instance.expirationTimeModelAllStories, StorySettingDefault.Instance.slidingExpirationModelAllStories);
            }
            if (chapterResult == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            ChapterModelResponse resultGroupChapter = _mapper.Map<ChapterModelResponse>(chapterResult);
            List<string> chunksContent = SplitToChunks(chapterResult.Body, _chunkSize).ToList();
            chunksContent.RemoveAll(x => x is null);
            resultGroupChapter.BodyChunk = chunksContent;
            resultGroupChapter.ChunkSize = chunksContent.Count;
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = resultGroupChapter;
            return methodResult;
        }
        /// <summary>
        /// Get list chapter and paging according by 100 chapters each chunk
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<MethodResult<List<ChapterListPagingResponse>>> PagingChapterListByStoryId(int storyId)
        {
            MethodResult<List<ChapterListPagingResponse>> methodResult = new()
            {
                Result = new List<ChapterListPagingResponse>()
            };
            List<Chapter>? cacheChapterTotal = await _cache.GetRecordAsync<List<Chapter>>($"{string.Format(StorySettingDefault.Instance.keyModelResponseTotalChaptersByStory, storyId)}");
            var chapterTotal = cacheChapterTotal is null || !cacheChapterTotal.Any() ? await _queryable.Where(x => x.StoryId == storyId).Select(x => x).ToListAsync() : cacheChapterTotal;
            if (cacheChapterTotal is null || !cacheChapterTotal.Any())
            {
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelResponseTotalChaptersByStory, storyId)}",
                   chapterTotal, StorySettingDefault.Instance.expirationTimeModelAllStories, StorySettingDefault.Instance.slidingExpirationModelAllStories);
            }
            var totalLength = chapterTotal.Count;
            for (int i = 0; i < totalLength; i += 100)
            {
                var tempChapterListPaging = new ChapterListPagingResponse
                {
                    From = i + 1,
                    To = Math.Min(i + 100, totalLength),
                    FromId = chapterTotal.FirstOrDefault(x => x.NumberOfChapter == i + 1)?.Id ?? 0,
                    ToId = chapterTotal.FirstOrDefault(x => x.NumberOfChapter == Math.Min(i + 100, totalLength))?.Id ?? 0
                };
                methodResult.Result.Add(tempChapterListPaging);
            }
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// Chunk chapter by id each 350 character
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public async Task<MethodResult<ChapterChunkResponse>> ChunkChapterListByStoryId(int chapterId, int chunkSize = 250)
        {
            MethodResult<ChapterChunkResponse> methodResult = new();
            var chapterInfo = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == chapterId);
            if (chapterInfo == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            List<string> chunksContent = SplitToChunks(chapterInfo.Body, chunkSize).ToList();
            ChapterChunkResponse resultGroupChapter = _mapper.Map<ChapterChunkResponse>(chapterInfo);
            chunksContent.RemoveAll(x => x is null);
            resultGroupChapter.BodyChunk = chunksContent;
            resultGroupChapter.PageSize = chunksContent.Count;
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = resultGroupChapter;
            return methodResult;
        }
        /// <summary>
        /// Get detail chapter by id
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public async Task<MethodResult<ChapterModelResponse>> GetDetailChapterById(int chapterId)
        {
            MethodResult<ChapterModelResponse> methodResult = new();
            Chapter? cacheChapter = await _cache.GetRecordAsync<Chapter>($"{string.Format(StorySettingDefault.Instance.keyModelDetailChaptersByStory, chapterId)}");
            var chapterResult = cacheChapter is null ? await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == chapterId) : cacheChapter;
            if (cacheChapter is null)
            {
                await _cache.SetRecordAsync($"{string.Format(StorySettingDefault.Instance.keyModelDetailChaptersByStory, chapterId)}",
                   chapterResult, StorySettingDefault.Instance.expirationTimeModelAllStories, StorySettingDefault.Instance.slidingExpirationModelAllStories);
            }
            if (chapterResult == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumChapterErrorCode.CT11),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumChapterErrorCode.CT11), nameof(EnumChapterErrorCode.CT11)) }
                );
                return methodResult;
            }
            ChapterModelResponse resultGroupChapter = _mapper.Map<ChapterModelResponse>(chapterResult);
            List<string> chunksContent = SplitToChunks(chapterResult.Body, _chunkSize).ToList();
            chunksContent.RemoveAll(x => x is null);
            resultGroupChapter.BodyChunk = chunksContent;
            resultGroupChapter.ChunkSize = chunksContent.Count;
            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = resultGroupChapter;
            return methodResult;
        }
        private static string[] SplitToChunks(string content, int targetChunkSize)
        {
            string[] paragraphs = content.Split(new[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> chunksList = new();

            StringBuilder currentChunk = new();
            int currentChunkSize = 0;

            foreach (string paragraph in paragraphs)
            {
                if (currentChunkSize + paragraph.Length + 1 > targetChunkSize)
                {
                    chunksList.Add(currentChunk.ToString());
                    currentChunk.Clear();
                    currentChunkSize = 0;
                }

                if (currentChunkSize > 0)
                {
                    currentChunk.Append('\n');
                    currentChunkSize++;
                }

                currentChunk.Append(paragraph);
                currentChunkSize += paragraph.Length;
            }

            if (currentChunk.Length > 0)
            {
                chunksList.Add(currentChunk.ToString());
            }

            return chunksList.ToArray();
        }

    }
}
