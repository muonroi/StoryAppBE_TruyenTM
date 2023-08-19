using BaseConfig.BaseDbContext.Common;
using BaseConfig.MethodResult;
using MuonRoi.Social_Network.Chapters;
using MuonRoiSocialNetwork.Common.Models.Chapter.Response;

namespace MuonRoiSocialNetwork.Domains.Interfaces.Queries.Chapters
{
    /// <summary>
    /// Define interface
    /// </summary>
    public interface IChapterQueries : IQueries<Chapter>
    {
        /// <summary>
        /// Get all chapter latest paging
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isSetCache"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<ChapterModelResponse>>> GetAllChapterAsync(int pageIndex, int pageSize, bool isSetCache = false);

        /// <summary>
        /// Get group chapter each view
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fromChapterId"></param>
        /// <param name="isSetCache"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<ChapterModelResponse>>> GetGroupChapterAsync(long storyId, int fromChapterId = 0, int pageIndex = 1, int pageSize = 20, bool isSetCache = false);

        /// <summary>
        /// Get list chapters of story
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="storyId"></param>
        /// <param name="isLatest"></param>
        /// <returns></returns>
        Task<MethodResult<PagingItemsDTO<ChapterPreviewResponse>>> GetListChapterOfStory(long storyId, int pageIndex = 1, int pageSize = 10, bool isLatest = false);
        /// <summary>
        /// Total chapter of story
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        Task<MethodResult<int>> GetTotalChapterOfStoryIdAsync(long storyId);
    }
}
