using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AwesomeGICBank.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> :
        IRepositoryBase<TEntity> where TEntity : class
    {
        internal BankDbContext dbContext;
        internal DbSet<TEntity> dbSet;

        public RepositoryBase(BankDbContext context)
        {
            dbContext = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual TEntity Insert(TEntity entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public virtual void InsertRange(IEnumerable<TEntity> entityList)
        {
            if (entityList.Any())
            {
                foreach (var entity in entityList)
                    dbSet.Add(entity);
            }
        }

        public virtual void Delete(object id)
        {
            var entity = dbSet.Find(id);
            if (entity != null)
                Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (dbContext.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual TEntity? GetById(object id) => dbSet.Find(id);

        public IEnumerable<TEntity> GetAll() => dbSet.AsNoTracking().ToList();

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? take = null,
            int? skip = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                query = orderBy(query);
            if (skip.HasValue)
                query = query.Skip(skip.Value);
            if (take.HasValue)
                query = query.Take(take.Value);

            return query.AsNoTracking().ToList();
        }
    }
}
