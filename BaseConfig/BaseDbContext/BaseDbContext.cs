using BaseConfig.BaseDbContext.Common;
using BaseConfig.EntityObject.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Diagnostics;

namespace BaseConfig.BaseDbContext
{
    public class BaseDbContext : DbContext, IUnitOfWork, IDisposable
    {
        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;

        private readonly List<Entity> _trackEntities;

        public bool HasActiveTransaction => _currentTransaction != null;
        public IDbContextTransaction GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        public async Task<Guid> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            IExecutionStrategy strategy = Database.CreateExecutionStrategy();
            if (Database.IsInMemory())
            {
                await base.SaveChangesAsync(cancellationToken);
                await DispatchDomainEventsAsync();
                return Guid.NewGuid();
            }

            if (HasActiveTransaction)
            {
                await base.SaveChangesAsync(cancellationToken);
                await DispatchDomainEventsAsync();
                return _currentTransaction.TransactionId;
            }

            return await strategy.ExecuteAsync(async delegate
            {
                IDbContextTransaction dbContextTransaction = await BeginTransactionAsync().ConfigureAwait(continueOnCapturedContext: false);
                try
                {
                    _ = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                    await DispatchDomainEventsAsync();
                    await CommitTransactionAsync(dbContextTransaction).ConfigureAwait(continueOnCapturedContext: false);
                    return dbContextTransaction.TransactionId;
                }
                catch (Exception)
                {
                    RollbackTransaction();
                    throw;
                }
            });
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return null;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
            return _currentTransaction;
        }
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != _currentTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void TrackEntity(Entity entity)
        {
            _trackEntities.Add(entity);
        }
        private async Task DispatchDomainEventsAsync()
        {
            IEnumerable<Entity> domainEntities = _trackEntities.Where((Entity x) => x.DomainEvents != null && x.DomainEvents.Any());
            List<INotification> domainEvents = domainEntities.SelectMany((Entity x) => x.DomainEvents).ToList();
            domainEntities.ToList().ForEach(delegate (Entity entity)
            {
                entity.ClearDomainEvents();
            });
            IEnumerable<Task> tasks = ((IEnumerable<INotification>)domainEvents).Select((Func<INotification, Task>)async delegate (INotification domainEvent)
            {
                Console.WriteLine($"Dispatching InternalEvent: {domainEvent.GetType()}");
                await _mediator.Publish(domainEvent);
                Console.WriteLine($"Dispatched InternalEvent: {domainEvent.GetType()}");
            });
            await Task.WhenAll(tasks);
        }

        public BaseDbContext(DbContextOptions options, IMediator mediator)
           : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
            _trackEntities = new List<Entity>();
            Debug.WriteLine("BaseDbContext::ctor ->" + GetHashCode());
        }
    }
}
