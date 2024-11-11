using System.Linq.Expressions;

namespace AwesomeGICBank.Core.Contracts
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        #region Synchronous Methods

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

        #endregion

        #region Asynchronous Methods

        Task<TEntity> InsertAsync(TEntity entity);

        Task InsertRangeAsync(IEnumerable<TEntity> entityList);

        Task DeleteAsync(object id);

        Task<TEntity?> GetByIdAsync(object id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? take = null,
            int? skip = null,
            string includeProperties = "");

        #endregion
    }
}
