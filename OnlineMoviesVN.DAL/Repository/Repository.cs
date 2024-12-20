﻿using Microsoft.EntityFrameworkCore;
using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository.IRepository;
using System.Linq.Expressions;

namespace OnlineMoviesVN.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IGrouping<TKey, T>>> GetGroupedAsync<TKey>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, TKey>> groupBy)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var groupedQuery = query.GroupBy(groupBy);

            return await Task.FromResult(groupedQuery.ToList());
        }


        public async Task RemoveAsync(T entity)
        {
            DbSet.Remove(entity);
            await Task.CompletedTask; // No async operation, just completing the task
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            await Task.CompletedTask; // No async operation, just completing the task
        }
    }
}
