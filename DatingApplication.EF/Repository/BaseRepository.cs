using DatingApplication.Core.IRepository;
using DatingApplication.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.EF.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationContext _dbContext;

        public BaseRepository(ApplicationContext context) 
        {
            _dbContext = context;
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip = null, int? take = null, Expression<Func<T, object>>? orderBy = null, bool? isNoTracking = false)
        {
            IQueryable<T> query=_dbContext.Set<T>().Where(criteria);
            if (skip is not null)
            {
                query = query.Skip(skip.Value);
            }
            if (take is not null)
            {
                query = query.Take(take.Value);
            }
            return query;
        }

        public IEnumerable<T> GetAll(bool? isNoTracking = false)
        {
            if (isNoTracking.HasValue && isNoTracking.Value)
            {
                return _dbContext.Set<T>().ToList();

            }
            else
            {
               return _dbContext.Set<T>().AsNoTracking().ToList();
            }
        }

        T IBaseRepository<T>.Add(T entity)
        {
            _dbContext.Add(entity);
            return entity;
        }


        IQueryable<T> IBaseRepository<T>.FindAllWithInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> data = _dbContext.Set<T>();
            foreach(var item in includes)
            {
                data=data.Include(item);
            }
            return data.Where(criteria);
        }

        T? IBaseRepository<T>.FindWithInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> data = _dbContext.Set<T>();
            foreach (var item in includes)
            {
                data = data.Include(item);
            }
            return data.Where(criteria).FirstOrDefault();
        }

        T? IBaseRepository<T>.GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        void IBaseRepository<T>.Remove(T entity)
        {
            _dbContext.Remove(entity);
        }

        void IBaseRepository<T>.Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
