using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public class EfEntityRepositoryBase<T, TContext> : IEntityRepository<T>
        where T : class, new()
        where TContext : DbContext, new()
    {
        public void Add(T entity)
        {
            using var context = new TContext();
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            using var context = new TContext();
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new TContext();
            var entity = context.Set<T>().Find(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            using var context = new TContext();
            return context.Set<T>().FirstOrDefault(filter);
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            using var context = new TContext();
            return filter == null
                ? context.Set<T>().ToList()
                : context.Set<T>().Where(filter).ToList();
        }
    }
}
