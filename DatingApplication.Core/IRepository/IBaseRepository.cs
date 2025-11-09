using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll(bool? isNoTracking = false);
        IEnumerable<T> FindAll(Expression<Func<T,bool>> criteria,int? skip=null,int? take=null, Expression<Func< T,object>>? orderBy=null, bool? isNoTracking = false);
        T Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        IQueryable<T> FindAllWithInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        T? FindWithInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);

    }
}
