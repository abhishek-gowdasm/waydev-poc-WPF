using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Interfaces;
using System.Linq.Expressions;

namespace OrderManagementSystem.Data
{
    /// <summary>
    /// Generic repository implementation for data access operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly OrderManagementDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(OrderManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>IQueryable of entities</returns>
        public virtual IQueryable<T> GetAll()
        {
            try
            {
                Console.WriteLine($"Repository: Getting all entities of type {typeof(T).Name}");
                return _dbSet.AsQueryable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error getting all entities of type {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get entities by predicate
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>IQueryable of filtered entities</returns>
        public virtual IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Console.WriteLine($"Repository: Getting entities of type {typeof(T).Name} with predicate");
                return _dbSet.Where(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error getting entities of type {typeof(T).Name} with predicate: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get entity by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity or null</returns>
        public virtual async Task<T?> GetByIdAsync(object id)
        {
            try
            {
                Console.WriteLine($"Repository: Getting entity of type {typeof(T).Name} with ID {id}");
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error getting entity of type {typeof(T).Name} with ID {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Add new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public virtual async Task AddAsync(T entity)
        {
            try
            {
                Console.WriteLine($"Repository: Adding entity of type {typeof(T).Name}");
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error adding entity of type {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Update existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        public virtual void Update(T entity)
        {
            try
            {
                Console.WriteLine($"Repository: Updating entity of type {typeof(T).Name}");
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error updating entity of type {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public virtual void Delete(T entity)
        {
            try
            {
                Console.WriteLine($"Repository: Deleting entity of type {typeof(T).Name}");
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error deleting entity of type {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Check if entity exists
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>True if exists, false otherwise</returns>
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Console.WriteLine($"Repository: Checking if entity of type {typeof(T).Name} exists");
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error checking if entity of type {typeof(T).Name} exists: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get count of entities
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <returns>Count of entities</returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                Console.WriteLine($"Repository: Getting count of entities of type {typeof(T).Name}");
                if (predicate == null)
                    return await _dbSet.CountAsync();
                
                return await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository: Error getting count of entities of type {typeof(T).Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all entities with includes
        /// </summary>
        /// <param name="includes">Navigation properties to include</param>
        /// <returns>IQueryable with includes</returns>
        public virtual IQueryable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        /// <summary>
        /// Get entities by predicate with includes
        /// </summary>
        /// <param name="predicate">Filter expression</param>
        /// <param name="includes">Navigation properties to include</param>
        /// <returns>IQueryable with includes</returns>
        public virtual IQueryable<T> GetByWithIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.Where(predicate);
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
} 