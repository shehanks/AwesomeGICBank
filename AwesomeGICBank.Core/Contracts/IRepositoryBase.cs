using System.Linq.Expressions;

namespace AwesomeGICBank.Core.Contracts
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        TEntity Insert(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entityList);

        void Delete(object id);

        void Delete(TEntity entity);

        void Update(TEntity entity);

        TEntity? GetById(object id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? take = null,
            int? skip = null,
            string includeProperties = "");
    }
}
