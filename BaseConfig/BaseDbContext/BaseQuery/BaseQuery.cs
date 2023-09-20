using AutoMapper;
using BaseConfig.BaseDbContext.Common;
using BaseConfig.EntityObject.Entity;
using BaseConfig.Infrashtructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BaseConfig.BaseDbContext.BaseQuery
{
    public class BaseQuery<T> : IQueries<T> where T : Entity
    {
        protected readonly IMapper _mapper;

        protected readonly BaseDbContext _dbBaseContext;

        protected readonly AuthContext _authContext;

        protected readonly DbSet<T> _dbSet;

        public string CurrentUserId => _authContext.CurrentUserId;

        public string CurrentUsername => _authContext.CurrentUsername;

        public IQueryable<T> _queryable => from m in _dbSet.AsNoTracking()
                                           where !m.IsDeleted
                                           select m;
        protected readonly IDistributedCache _cache;
        public BaseQuery(BaseDbContext dbContext, AuthContext authContext, IDistributedCache cache, IMapper mapper)
        {
            _dbBaseContext = dbContext;
            _authContext = authContext;
            _dbSet = _dbBaseContext.Set<T>();
            _cache = cache;
            _mapper = mapper;
        }

        public virtual async Task<T> GetByIdAsync(long id, int? siteId = null)
        {
            try
            {
                return await _dbSet.AsNoTracking().SingleOrDefaultAsync((T c) => c.Id == id).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<T> GetByGuidAsync(Guid guid, int? siteId = null)
        {
            try
            {
                return await _dbSet.AsNoTracking().SingleOrDefaultAsync((T c) => c.Guid == guid && !c.IsDeleted).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetAllAsync(int? siteId = null)
        {
            try
            {
                return await (from c in _dbSet.AsNoTracking()
                              where !c.IsDeleted
                              select c).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<PagingItemsDTO<T>> GetAllAsync(int page, int pageSize, int? siteId = null)
        {
            try
            {
                IOrderedQueryable<T> query = from c in _dbSet.AsNoTracking()
                                             where !c.IsDeleted
                                             select c into m
                                             orderby m.Id descending
                                             select m;
                return await GetListPaging(query, page, pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagingItemsDTO<T>> GetListPaging(IQueryable<T> query, int page, int pageSize)
        {
            PagingItemsDTO<T> pagingItemsDTO = new();
            PagingItemsDTO<T> pagingItemsDTO2 = pagingItemsDTO;
            pagingItemsDTO2.Items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PagingItemsDTO<T> pagingItemsDTO3 = pagingItemsDTO;
            PagingInfoDTO pagingInfoDTO = new()
            {
                Page = page,
                PageSize = pageSize
            };
            PagingInfoDTO pagingInfoDTO2 = pagingInfoDTO;
            pagingInfoDTO2.TotalItems = query.Count();
            pagingItemsDTO3.PagingInfo = pagingInfoDTO;
            await Task.FromResult(pagingItemsDTO);
            return pagingItemsDTO;
        }
    }
}
