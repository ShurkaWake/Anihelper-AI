using AutoFilterer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstractions
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : IEntity<TKey>
        where TKey : IComparable<TKey>
    {
        Task<TEntity> GetAsync(TKey id);

        Task<IQueryable<TEntity>> GetAllAsync(PaginationFilterBase filter);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<int> ConfirmAsync();
    }
}
