using BaseConfig.EntityObject.Entity;
using BaseConfig.MethodResult;

namespace BaseConfig.BaseDbContext.Common
{
    public interface IRepository<T> where T : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> GetByIdAsync(long id, int? siteId = null);

        Task<T> GetByGuidAsync(Guid guid, int? siteId = null);

        Task<IEnumerable<T>> GetWhereAsync(Func<T, bool> predicate);

        Task<bool> AnyAsync(long id, int? siteId = null);

        Task<bool> AnyGuidAsync(Guid guid, int? siteId = null);

        T Add(T newEntity);

        T Update(T updateEntity);

        Task<bool> DeleteAsync(T deleteEntity);

        Task ExecuteTransactionAsync(Func<Task<VoidMethodResult>> action);
    }
}
