using Microsoft.EntityFrameworkCore.Storage;
using OrderManagementSystem.Interfaces;

namespace OrderManagementSystem.Data
{
    /// <summary>
    /// Unit of Work implementation for transaction management
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderManagementDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(OrderManagementDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Get repository for specific entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_context);
            }

            return (IRepository<T>)_repositories[type];
        }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        /// <returns>Number of affected rows</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        /// <returns>Database transaction</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 