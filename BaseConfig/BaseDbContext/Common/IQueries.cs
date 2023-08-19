using BaseConfig.EntityObject.Entity;
namespace BaseConfig.BaseDbContext.Common
{
    public interface IQueries<T> where T : Entity
    {
        Task<T> GetByIdAsync(long id, int? siteId = null);

        Task<T> GetByGuidAsync(Guid guid, int? siteId = null);

        Task<List<T>> GetAllAsync(int? siteId = null);

        Task<PagingItemsDTO<T>> GetAllAsync(int page, int pageSize, int? siteId = null);
    }
}
