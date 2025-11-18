using System.Linq.Expressions;

namespace OrderManagementSystem.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetByWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
} 