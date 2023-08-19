using BaseConfig.BaseDbContext.Common;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Extentions.Datetime;
using BaseConfig.Infrashtructure;
using BaseConfig.Infrashtructure.Interface.Events;
using BaseConfig.MethodResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using BaseDbContextEntity = BaseConfig.BaseDbContext.BaseDbContext;
namespace BaseConfig.BaseDbContext.BaseRepository
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        private readonly AuthContext _authContext;

        protected readonly BaseDbContextEntity _dbBaseContext;

        protected readonly DbSet<T> _dbSet;

        public string CurrentUserId => _authContext.CurrentUserId;

        public string CurrentUsername => _authContext.CurrentUsername;

        public IUnitOfWork UnitOfWork => _dbBaseContext;

        protected IQueryable<T> _queryable => _dbSet.Where((m) => !m.IsDeleted);

        public BaseRepository(BaseDbContextEntity dbContext, AuthContext authContext)
        {
            _authContext = authContext;
            _dbBaseContext = dbContext;
            _dbSet = _dbBaseContext.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(long id, int? siteId = null)
        {
            try
            {
                return await _dbSet.SingleOrDefaultAsync((c) => c.Id == id && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetByGuidAsync(Guid guid, int? siteId = null)
        {
            try
            {
                return await _dbSet.SingleOrDefaultAsync((c) => c.Guid == guid && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual T Add(T newEntity)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                newEntity.CreatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                newEntity.UpdatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
                newEntity.CreatedUserName = _authContext.CurrentUsername;
                newEntity.UpdatedUserName = _authContext.CurrentUsername;
                newEntity.UpdatedUserGuid = new Guid(_authContext.Guid);
                newEntity.Guid = Guid.NewGuid();
                newEntity.AddDomainEvent(new EntityCreatedEvent<T>(newEntity));
                _dbBaseContext.TrackEntity(newEntity);
                return _dbSet.Add(newEntity).Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<bool> AnyAsync(long id, int? siteId = null)
        {
            try
            {
                return await _dbSet.AnyAsync((c) => c.Id == id && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AnyGuidAsync(Guid guid, int? siteId = null)
        {
            try
            {
                return await _dbSet.AnyAsync((c) => c.Guid == guid && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Task<bool> DeleteAsync(T deleteEntity)
        {
            try
            {
                deleteEntity.IsDeleted = true;
                deleteEntity.DeletedDateTS = DateTime.UtcNow.GetTimeStamp(includedTimeValue: true);
                deleteEntity.DeletedUserName = _authContext.CurrentUsername;
                deleteEntity.UpdatedUserGuid = new Guid(_authContext.Guid);
                deleteEntity.AddDomainEvent(new EntityDeletedEvent<T>(deleteEntity));
                _dbBaseContext.TrackEntity(deleteEntity);
                _ = _dbSet.Update(deleteEntity).Entity;
                return Task.FromResult(result: true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual T Update(T updateEntity)
        {
            try
            {
                updateEntity.UpdatedDateTS = DateTime.UtcNow.GetTimeStamp(includedTimeValue: true);
                updateEntity.UpdatedUserName = _authContext.CurrentUsername;
                updateEntity.UpdatedUserGuid = new Guid(_authContext.Guid);
                updateEntity.AddDomainEvent(new EntityChangedEvent<T>(updateEntity));
                _dbBaseContext.TrackEntity(updateEntity);
                return _dbSet.Update(updateEntity).Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual async Task ExecuteTransactionAsync(Func<Task<VoidMethodResult>> action)
        {
            if (_dbBaseContext.Database.IsInMemory() || _dbBaseContext.HasActiveTransaction)
            {
                await action().ConfigureAwait(continueOnCapturedContext: false);
                return;
            }

            IExecutionStrategy strategy = _dbBaseContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async delegate
            {
                using IDbContextTransaction transaction = await _dbBaseContext.BeginTransactionAsync().ConfigureAwait(continueOnCapturedContext: false);
                try
                {
                    if ((await action().ConfigureAwait(continueOnCapturedContext: false))?.IsOK ?? false)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }).ConfigureAwait(continueOnCapturedContext: false);
        }
        public void SetEntityState(T entity, EntityState state)
        {
            _dbBaseContext.Entry(entity).State = state;
        }
    }
}
