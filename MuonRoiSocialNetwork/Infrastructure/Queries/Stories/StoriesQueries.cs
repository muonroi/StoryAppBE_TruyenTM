using AutoMapper;
using BaseConfig.BaseDbContext.BaseQuery;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.Extentions.Image;
using BaseConfig.Extentions.String;
using BaseConfig.Infrashtructure;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MuonRoi.Social_Network.Storys;
using MuonRoi.Social_Network.Tags;
using MuonRoiSocialNetwork.Common.Models.Stories.Request.Search;
using MuonRoiSocialNetwork.Common.Models.Stories.Response;
using MuonRoiSocialNetwork.Common.Models.Stories.Response.Dto;
using MuonRoiSocialNetwork.Common.Models.Tuples;
using MuonRoiSocialNetwork.Common.Settings.StorySettings;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.TagsAndTagInStories;
using MuonRoiSocialNetwork.Infrastructure.Helpers;
using Newtonsoft.Json;
using CategoryEntities = MuonRoi.Social_Network.Categories.Category;
namespace MuonRoiSocialNetwork.Infrastructure.Queries.Stories
{
    /// <summary>
    /// Handler queries
    /// </summary>
    public class StoriesQueries : BaseQuery<Story>, IStoriesQueries
    {
        private readonly ITagInStoriesQueries _tagInStoriesQueries;
        private readonly ICategoryQueries _categoryQueries;
        private readonly ITagQueries _tagQueries;
        private readonly IConfiguration _configuration;
        private readonly IChapterQueries _chapterQueries;
        private readonly IBookmarkStoryQueries _bookmarkQueries;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="authContext"></param>
        /// <param name="mapper"></param>
        /// <param name="tagInStoriesQueries"></param>
        /// <param name="categoryQueries"></param>
        /// <param name="tagQueries"></param>
        /// <param name="cache"></param>
        /// <param name="configuration"></param>
        /// <param name="chapterQueries"></param>
        /// <param name="bookmarkQueries"></param>
        public StoriesQueries(MuonRoiSocialNetworkDbContext dbContext, AuthContext authContext, IMapper mapper, ITagInStoriesQueries tagInStoriesQueries, ICategoryQueries categoryQueries, ITagQueries tagQueries, IDistributedCache cache, IConfiguration configuration,
            IChapterQueries chapterQueries, IBookmarkStoryQueries bookmarkQueries) : base(dbContext, authContext, cache, mapper)
        {
            _tagInStoriesQueries = tagInStoriesQueries;
            _categoryQueries = categoryQueries;
            _tagQueries = tagQueries;
            _configuration = configuration;
            _chapterQueries = chapterQueries;
            _bookmarkQueries = bookmarkQueries;
        }
        /// <summary>
        /// Search story
        /// </summary>
        /// <param name="requestSearch"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<StoryModelResponse>>> GetStoriesByParameters(SearchStoriesModelRequest requestSearch)
        {
            MethodResult<PagingItemsDTO<StoryModelResponse>> methodResult = new();
            List<TagInStory> filterTaginstory = new();
            var checkStatus = await IsCacheExistAsync();
            if (checkStatus is null || checkStatus.Status)
            {
                await SetCacheAsync();
                checkStatus = await IsCacheExistAsync();
            }
            var filteredStoryGuids = checkStatus.TagsInStories
            .Where(x => requestSearch.SearchByTagName.Contains(x.TagId))
            .Select((x, y) => new { x.StoryId, x.TagId })
            .ToList();
            var storyListCache = await _cache.GetRecordAsync<List<Story>>(StorySettingDefault.Instance.keyModelResponseTotalAllStories);
            IQueryable<Story> querySearch = storyListCache?.AsQueryable() ?? _queryable.AsNoTracking().Select(x => x);
            if (requestSearch.PageSize > 200)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST12),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(requestSearch.PageSize), requestSearch.PageSize) }
                );
                return methodResult;
            }
            if (requestSearch.IsNewUpdate)
            {
                querySearch = querySearch.OrderByDescending(x => x.CreatedDateTS).Select(x => x);
            }
            if (requestSearch.SearchByCategory > 0)
            {
                querySearch = querySearch.Where(x => x.CategoryId == requestSearch.SearchByCategory).Select(x => x);
            }
            if (!string.IsNullOrEmpty(requestSearch.SearchByTitle))
            {
                querySearch = querySearch.Where(x => x.StoryTitle.ToLower().Contains(requestSearch.SearchByTitle.ToLower())).Select(x => x);
            }
            if (requestSearch.SearchByTagName.Length > 0)
            {
                List<long> filteredStoryGuidForTag = checkStatus.TagsInStories
                .Where(x => requestSearch.SearchByTagName.Contains(x.TagId))
                .Select(x => x.StoryId)
                .ToList();

                querySearch = querySearch
                    .Where(x => filteredStoryGuidForTag.Contains(x.Id))
                    .Select(x => x);
            }
            if (requestSearch.SearchByNumberChapter != Common.Enums.Chapters.EnumNumberChapter.None)
            {
                var storyList = await _cache.GetRecordAsync<IQueryable<Story>>(StorySettingDefault.Instance.keyModelResponseTotalAllStories) ?? querySearch.AsQueryable().Select(x => x);
                switch (requestSearch.SearchByNumberChapter)
                {
                    case Common.Enums.Chapters.EnumNumberChapter.Low:
                        querySearch = storyList.Where(x => x.TotalChapter <= 100).Select(x => x);
                        break;
                    case Common.Enums.Chapters.EnumNumberChapter.Medium:
                        querySearch = storyList.Where(x => x.TotalChapter > 100 && x.TotalChapter <= 1000).Select(x => x);
                        break;
                    case Common.Enums.Chapters.EnumNumberChapter.High:
                        querySearch = storyList.Where(x => x.TotalChapter > 1000 && x.TotalChapter <= 3000).Select(x => x);
                        break;
                    case Common.Enums.Chapters.EnumNumberChapter.SupperHigh:
                        querySearch = storyList.Where(x => x.TotalChapter > 3000).Select(x => x);
                        break;
                    default:
                        break;
                }
            }
            CategoryEntities? categoryOfStory = new();
            PagingItemsDTO<Story> pagingStoryItemsDTO = await GetListPaging(querySearch, requestSearch.PageIndex, requestSearch.PageSize).ConfigureAwait(false);
            if (checkStatus.Categories != null && requestSearch.SearchByCategory > 0)
            {
                categoryOfStory = checkStatus.Categories.FirstOrDefault(x => x.Id == requestSearch.SearchByCategory && !x.IsDeleted);
            }
            List<StoryModelResponse> resultStories = _mapper.Map<List<StoryModelResponse>>(pagingStoryItemsDTO.Items);
            var tagOfStory = (from story in resultStories
                              join tagsInStory in filteredStoryGuids on story.Id equals tagsInStory.StoryId
                              join tagsSingle in checkStatus.Tags on tagsInStory.TagId equals tagsSingle.Id
                              where !tagsSingle.IsDeleted
                              select new { tagsSingle, tagsInStory }).ToList();

            var categoryOfStorys = (from story in pagingStoryItemsDTO.Items
                                    join categorys in checkStatus.Categories ?? await _categoryQueries.GetAllAsync().ConfigureAwait(false)
                                    on story.CategoryId equals categorys.Id
                                    where !categorys.IsDeleted
                                    select new { categorys, story }).ToList();
            AssignValueToStories(ref resultStories, categoryOfStorys, tagOfStory);
            for (int i = 0; i < resultStories.Count; i++)
            {
                resultStories[i].UpdatedDateString = GetTimeDifferenceText(resultStories[i].UpdatedDateTs ?? 0);
                resultStories[i].TotalPageIndex = await GetTotalPageIndex(resultStories[i].Id);
            }
            methodResult.Result = new PagingItemsDTO<StoryModelResponse>
            {
                Items = resultStories,
                PagingInfo = pagingStoryItemsDTO.PagingInfo
            };
            return methodResult;
        }
        private async Task<int> GetTotalPageIndex(long storyId)
        {
            var totalChapter = await _chapterQueries.GetTotalChapterByStoryId(storyId);
            if (!totalChapter.IsOK || totalChapter.Result is null) return 0;
            return (int)Math.Ceiling((double)totalChapter.Result.ChapterTotal / 100);
        }
        /// <summary>
        /// Get all story
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<MethodResult<PagingItemsDTO<StoryModelResponse>>> GetStoriesAsync(int pageIndex = 1, int pageSize = 20)
        {
            MethodResult<PagingItemsDTO<StoryModelResponse>> methodResult = new();
            var checkStatus = await IsCacheExistAsync();
            if (checkStatus is null || checkStatus.Status)
            {
                await SetCacheAsync();
                checkStatus = await IsCacheExistAsync();
            }
            var filteredStoryGuids = checkStatus.TagsInStories
            .Select((x, y) => new { x.StoryId, x.TagId })
            .ToList();
            var storyListCache = await _cache.GetRecordAsync<List<Story>>(StorySettingDefault.Instance.keyModelResponseTotalAllStories);
            IQueryable<Story> querySearch = storyListCache?.AsQueryable() ?? _queryable.AsNoTracking().Select(x => x);
            if (pageSize > 50)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST12),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(pageSize), pageSize) }
                );
                return methodResult;
            }
            PagingItemsDTO<Story> pagingStoryItemsDTO = await GetListPaging(querySearch, pageIndex, pageSize).ConfigureAwait(false);
            List<StoryModelResponse> resultStories = _mapper.Map<List<StoryModelResponse>>(pagingStoryItemsDTO.Items);

            var tagOfStory = (from story in resultStories
                              join tagsInStory in filteredStoryGuids on story.Id equals tagsInStory.StoryId
                              join tagsSingle in checkStatus.Tags on tagsInStory.TagId equals tagsSingle.Id
                              where !tagsSingle.IsDeleted
                              select new { tagsSingle, tagsInStory }).ToList();

            var categoryOfStorys = (from story in pagingStoryItemsDTO.Items
                                    join categorys in checkStatus.Categories ?? await _categoryQueries.GetAllAsync().ConfigureAwait(false)
                                    on story.CategoryId equals categorys.Id
                                    where !categorys.IsDeleted
                                    select new { categorys, story }).ToList();
            AssignValueToStories(ref resultStories, categoryOfStorys, tagOfStory);
            resultStories = resultStories.OrderByDescending(st => st.Rating)
                .ThenByDescending(x => x.TotalFavorite)
                .ThenByDescending(x => x.TotalView)
                .ThenByDescending(x => x.TotalChapter)
                .ToList();
            for (int i = 0; i < resultStories.Count; i++)
            {
                var totalChapterResult = await _chapterQueries.GetTotalChapterOfStoryIdAsync(resultStories[i].Id);
                resultStories[i].TotalChapter = totalChapterResult.Result;
                resultStories[i].UpdatedDateString = GetTimeDifferenceText(resultStories[i].UpdatedDateTs ?? 0);
                resultStories[i].RankNumber = i + 1;
                resultStories[i].TotalPageIndex = await GetTotalPageIndex(resultStories[i].Id);
            }
            methodResult.Result = new PagingItemsDTO<StoryModelResponse>
            {
                Items = resultStories,
                PagingInfo = pagingStoryItemsDTO.PagingInfo
            };
            return methodResult;
        }
        private void AssignValueToStories(ref List<StoryModelResponse> resultStories, dynamic categoryOfStorys, dynamic tagOfStory)
        {
            for (int i = 0; i < resultStories.Count; i++)
            {
                foreach (var item in categoryOfStorys)
                {
                    if (resultStories[i].Guid == item.story.Guid)
                    {
                        resultStories[i].NameCategory = item.categorys.NameCategory ?? string.Empty;
                    }
                }
                foreach (var item in tagOfStory)
                {
                    if (item.tagsInStory.StoryId == resultStories[i].Id && item.tagsInStory.TagId == item.tagsSingle.Id)
                    {
                        resultStories[i].NameTag.Add(item.tagsSingle.TagName);
                    }
                }
                resultStories[i].ImgUrl = HandlerImages.TakeLinkImage(_configuration, resultStories[i].ImgUrl);
                resultStories[i].SlugAuthor = StringManagers.GenerateSlug(resultStories[i].AuthorName);
            }
        }
        /// <summary>
        /// Get special story
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public async Task<MethodResult<StoryModelResponse>> GetStoryAsync(int storyId)
        {

            MethodResult<StoryModelResponse> methodResult = new();
            var checkStatus = await IsCacheExistAsync();
            if (checkStatus is null || checkStatus.Status)
            {
                await SetCacheAsync();
                checkStatus = await IsCacheExistAsync();
            }

            var filteredStoryGuids = checkStatus.TagsInStories
            .Select((x, y) => new { x.StoryId, x.TagId })
            .ToList();
            Story? querySearch = await _queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == storyId);

            if (querySearch == null)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST10),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), EnumStoryErrorCode.ST10) }
                );
                return methodResult;
            }

            StoryModelResponse resultStories = _mapper.Map<StoryModelResponse>(querySearch);
            var firstAndLastChapter = await _chapterQueries.GetFirstAndLastChapterByStory(storyId);
            var tagOfStory = (from tagsInStory in filteredStoryGuids
                              join tagsSingle in checkStatus.Tags on tagsInStory.TagId equals tagsSingle.Id
                              where !tagsSingle.IsDeleted && tagsInStory.StoryId == querySearch.Id
                              select new { tagsSingle, tagsInStory }).ToList();
            CategoryEntities? categoryOfStorys = (from categorys in checkStatus.Categories ?? await _categoryQueries.GetAllAsync().ConfigureAwait(false)
                                                  where !categorys.IsDeleted && categorys.Id == querySearch.CategoryId
                                                  select categorys).FirstOrDefault();
            _authContext.CurrentUserId = string.IsNullOrEmpty(_authContext.CurrentUserId) ? Guid.NewGuid().ToString() : _authContext.CurrentUserId;
            var bookmarkResult = await _bookmarkQueries.ExistBookmarkStoryOfUser(resultStories.Guid, new Guid(_authContext.CurrentUserId));

            SetMoreVariableStory(ref resultStories, tagOfStory.Select(x => x.tagsSingle.TagName).ToList(), categoryOfStorys == null ? string.Empty : categoryOfStorys.NameCategory ?? string.Empty, JsonConvert.DeserializeObject<StoryRattings>(querySearch.ListRattings)?.Data.Count ?? 0, firstAndLastChapter?.Result?.Keys?.FirstOrDefault() ?? 0, firstAndLastChapter?.Result?.Values?.FirstOrDefault() ?? 0);
            resultStories.IsBookmark = bookmarkResult.Result != null;
            resultStories.TotalPageIndex = await GetTotalPageIndex(resultStories.Id);
            methodResult.Result = resultStories;

            return methodResult;
        }
        private async Task<CustomTupleItemOfStory> IsCacheExistAsync()
        {
            List<Tag>? tagInfo = await _cache.GetRecordAsync<List<Tag>>($"{StorySettingDefault.Instance.keyModelResponseTags}");
            List<TagInStory>? tagInStoryInfo = await _cache.GetRecordAsync<List<TagInStory>>($"{StorySettingDefault.Instance.keyModelResponseTagInStory}");
            List<CategoryEntities>? categoryInfo = await _cache.GetRecordAsync<List<CategoryEntities>>($"{StorySettingDefault.Instance.keyModelResponseCategorys}");

            List<Tag> tags = tagInfo ?? await _tagQueries.GetAllAsync().ConfigureAwait(false);
            List<TagInStory> tagInStories = tagInStoryInfo ?? await _tagInStoriesQueries.GetAllAsync().ConfigureAwait(false);
            List<CategoryEntities> categories = categoryInfo ?? await _categoryQueries.GetAllAsync().ConfigureAwait(false);
            return new CustomTupleItemOfStory(tagInfo == null || tagInStoryInfo == null || categoryInfo == null, tags, tagInStories, categories);
        }
        private async Task SetCacheAsync()
        {
            List<Tag> tagTemp = await _tagQueries.GetAllAsync().ConfigureAwait(false);
            List<TagInStory> tagInStoryTemp = await _tagInStoriesQueries.GetAllAsync().ConfigureAwait(false);
            List<CategoryEntities> categoryTemp = await _categoryQueries.GetAllAsync().ConfigureAwait(false);
            await _cache.SetRecordAsync($"{StorySettingDefault.Instance.keyModelResponseTags}", tagTemp, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            await _cache.SetRecordAsync($"{StorySettingDefault.Instance.keyModelResponseTagInStory}", tagInStoryTemp, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
            await _cache.SetRecordAsync($"{StorySettingDefault.Instance.keyModelResponseCategorys}", categoryTemp, StorySettingDefault.Instance.expirationTimeLogin, StorySettingDefault.Instance.slidingExpirationLogin);
        }
        private void SetMoreVariableStory(ref StoryModelResponse resultStories, List<string> tagName, string nameCategory, int totalVote, long firstChapterId, long lastChapterId)
        {
            resultStories.ImgUrl = HandlerImages.TakeLinkImage(_configuration, resultStories.ImgUrl);
            resultStories.NameTag = tagName;
            resultStories.NameCategory = nameCategory;
            resultStories.UpdatedDateString = GetTimeDifferenceText(resultStories.UpdatedDateTs ?? 0);
            resultStories.TotalVote = totalVote;
            resultStories.Rating = Math.Round(resultStories.Rating, 1);
            resultStories.FirstChapterId = firstChapterId;
            resultStories.LastChapterId = lastChapterId;
            resultStories.SlugAuthor = StringManagers.GenerateSlug(resultStories.AuthorName);
        }
        /// <summary>
        /// Recommend stories
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MethodResult<PagingItemsDTO<StoryModelResponse>>> RecommendStoriesById(int storyId, int pageIndex = 1, int pageSize = 10)
        {
            MethodResult<PagingItemsDTO<StoryModelResponse>> methodResult = new();
            var checkStatus = await IsCacheExistAsync();
            if (checkStatus is null || checkStatus.Status)
            {
                await SetCacheAsync();
                checkStatus = await IsCacheExistAsync();
            }
            var filteredStoryId = checkStatus.TagsInStories
           .Select((x, y) => new { x.StoryId, x.TagId })
           .ToList();
            var storySpecial = _queryable.AsNoTracking().FirstOrDefault(x => x.Id == storyId);
            if (storySpecial == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST10),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), EnumStoryErrorCode.ST10) }
                );
                return methodResult;
            }
            IQueryable<Story> querySearch = _queryable.AsNoTracking()
                .Where(x => x.CategoryId == storySpecial.CategoryId)
                .OrderBy(x => x.TotalFavorite)
                .ThenBy(x => x.TotalView)
                .ThenBy(x => x.Rating)
                .Select(x => x);
            if (querySearch == null)
            {
                methodResult.StatusCode = StatusCodes.Status404NotFound;
                methodResult.AddApiErrorMessage(
                    nameof(EnumStoryErrorCode.ST10),
                    new[] { BaseConfig.EntityObject.Entity.Helpers.GenerateErrorResult(nameof(EnumStoryErrorCode.ST10), EnumStoryErrorCode.ST10) }
                );
                return methodResult;
            }
            PagingItemsDTO<Story> pagingStoryItemsDTO = await GetListPaging(querySearch, pageIndex, pageSize).ConfigureAwait(false);
            List<StoryModelResponse> resultStories = _mapper.Map<List<StoryModelResponse>>(pagingStoryItemsDTO.Items);

            var tagOfStory = (from story in resultStories
                              join tagsInStory in filteredStoryId on story.Id equals tagsInStory.StoryId
                              join tagsSingle in checkStatus.Tags on tagsInStory.TagId equals tagsSingle.Id
                              where !tagsSingle.IsDeleted
                              select new { tagsSingle, tagsInStory }).ToList();

            var categoryOfStorys = (from story in pagingStoryItemsDTO.Items
                                    join categorys in checkStatus.Categories ?? await _categoryQueries.GetAllAsync().ConfigureAwait(false)
                                    on story.CategoryId equals categorys.Id
                                    where !categorys.IsDeleted
                                    select new { categorys, story }).ToList();
            AssignValueToStories(ref resultStories, categoryOfStorys, tagOfStory);
            for (int i = 0; i < resultStories.Count; i++)
            {
                resultStories[i].UpdatedDateString = GetTimeDifferenceText(resultStories[i].UpdatedDateTs ?? 0);
                resultStories[i].TotalPageIndex = await GetTotalPageIndex(resultStories[i].Id);
            }
            methodResult.Result = new PagingItemsDTO<StoryModelResponse>
            {
                Items = resultStories,
                PagingInfo = pagingStoryItemsDTO.PagingInfo
            };
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
        /// <summary>
        /// Create time latest string (develop)
        /// </summary>
        /// <param name="timestampFromDatabase"></param>
        /// <returns></returns>
        private static string GetTimeDifferenceText(double timestampFromDatabase)
        {
            DateTime timestamp = DateTimeOffset.FromUnixTimeSeconds((long)timestampFromDatabase).DateTime;
            DateTime currentTime = DateTime.Now;
            TimeSpan difference = currentTime - timestamp;

            if (difference.TotalSeconds < 60)
            {
                return $"Updated {Math.Floor(difference.TotalSeconds)} second(s) ago";
            }
            else if (difference.TotalMinutes < 60)
            {
                return $"Updated {Math.Floor(difference.TotalMinutes)} minute(s) ago";
            }
            else if (difference.TotalHours < 24)
            {
                return $"Updated {Math.Floor(difference.TotalHours)} hour(s) ago";
            }
            else if (difference.TotalDays < 30)
            {
                return $"Updated {Math.Floor(difference.TotalDays)} day(s) ago";
            }
            else if (difference.TotalDays < 365)
            {
                int months = (int)(difference.TotalDays / 30);
                return $"Updated {months} month(s) ago";
            }
            else
            {
                int years = (int)(difference.TotalDays / 365);
                return $"Updated {years} year(s) ago";
            }
        }
        /// <summary>
        /// Update new all chapter and set cache
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsyncPrepareReferencesStoriesTableJob()
        {
            var storiesData = await _queryable.AsNoTracking().Select(x => x).ToListAsync();
            await _cache.SetRecordAsync($"{StorySettingDefault.Instance.keyModelResponseTotalAllStories}", storiesData, StorySettingDefault.Instance.expirationTimeModelAllStories, StorySettingDefault.Instance.slidingExpirationModelAllStories);
        }
    }
}
