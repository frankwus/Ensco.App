using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Data
{
    public interface IEnscoMocRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        IQueryable<T> Sort(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}
