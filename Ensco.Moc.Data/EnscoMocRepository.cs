using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using Ensco.Moc.Data;

namespace Ensco.Data
{
    public class EnscoMocRepository<T> : IEnscoMocRepository<T> where T : class
    {
        EnscoMocEntities _entities = new EnscoMocEntities();

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = null;

            try
            {
                query = _entities.Set<T>();
            }
            catch (Exception ex)
            {
                query = null;
            }

            return query;
        }

        public virtual T GetSingle(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().Where(predicate);

            if (query != null && query.Count() > 0)
            {
                return query.FirstOrDefault();
            }

            return null;
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = null;
            try
            {
                query = _entities.Set<T>().Where(predicate);
            }
            catch (Exception ex)
            {
                query = null;
            }

            return query;
        }

        public IQueryable<T> Sort(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = null;

            try
            {
                query = _entities.Set<T>().OrderBy(predicate);
            }
            catch (Exception ex)
            {

            }

            return query;
        }

        public virtual void Add(T entity)
        {
            try
            {
                _entities.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {

            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                _entities.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {

            }
        }

        public virtual void Edit(T entity)
        {
            try
            {
                _entities.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {

            }
        }

        public virtual void Save()
        {
            try
            {
                _entities.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
