﻿using AutoFilterer.Extensions;
using AutoFilterer.Types;
using DataAccess;
using DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessLogic.Abstractions
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IComparable<TKey>
    {
        public Repository(ApplicationContext context)
        {
            Context = context;
        }

        protected ApplicationContext Context { get; }

        private DbSet<TEntity> Entities => Context.Set<TEntity>();

        public async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Entities.AddRangeAsync(entities);
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Entities.Where(expression);
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(PaginationFilterBase filter = null)
        {
            if (filter == null)
            {
                return Entities.AsQueryable();
            }
            return Entities.AsQueryable().ApplyFilter(filter);
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await Entities.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public async Task<int> ConfirmAsync() =>
            await Context.SaveChangesAsync();
    }
}
