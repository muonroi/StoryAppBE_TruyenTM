namespace BaseConfig.BaseDbContext.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<Guid> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
