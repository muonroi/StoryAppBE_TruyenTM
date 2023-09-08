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
        /// <param name="fromChapterId"></param>
        /// <param name="toChapterId"></param>
        /// <returns></returns>
        Task<MethodResult<IEnumerable<ChapterModelResponse>>> GetGroupChapterAsync(long storyId, long fromChapterId = 0, long toChapterId = 0);

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
        /// <summary>
        /// Get first and last chapter
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        Task<MethodResult<Dictionary<long, long>>> GetFirstAndLastChapterByStory(long storyId);
        /// <summary>
        /// Get chapter next by story id
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        Task<MethodResult<ChapterModelResponse>> NextChapterByStoryId(int storyId, long chapterId);
        /// <summary>
        /// Get chapter previous by story id
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        Task<MethodResult<ChapterModelResponse>> PreviousChapterByStoryId(int storyId, long chapterId);
        /// <summary>
        /// Get list chapter and paging according by 100 chapters each chunk
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        Task<MethodResult<List<ChapterListPagingResponse>>> PagingChapterListByStoryId(int storyId);
        /// <summary>
        /// Chunk chapter by id each 350 character
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        Task<MethodResult<ChapterChunkResponse>> ChunkChapterListByStoryId(int chapterId, int chunkSize = 350);
        /// <summary>
        /// Get detail chapter
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        Task<MethodResult<ChapterModelResponse>> GetDetailChapterById(int chapterId);
    }
}
