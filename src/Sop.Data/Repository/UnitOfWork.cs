using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sop.Data.Page;

namespace Sop.Data.Repository
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/> 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the 
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        /// <summary>
        /// SaveChangesAsync
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        /// <summary>
        /// QueryAsync
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sql, object param = null, IDbContextTransaction trans = null, int? commandTimeout = null, CommandType? commandType = null) where TEntity : class
        {
            var conn = GetConnection();
            return conn.QueryAsync<TEntity>(sql, param, trans?.GetDbTransaction(), commandTimeout, commandType);
        }
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public Task<int> ExecuteAsync(string sql, object param, IDbContextTransaction trans = null)
        {
            var conn = GetConnection();
            return conn.ExecuteAsync(sql, param, trans?.GetDbTransaction());

        }
        /// <summary>
        /// QueryPageListAsync
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<PageResult<TEntity>> QueryPageListAsync<TEntity>(int pageIndex, int pageSize, string sql, object param = null, IDbContextTransaction trans = null) where TEntity : class
        {
            var connection = GetConnection();
            pageSize = pageSize > 0 ? pageSize : 1;
            pageIndex = pageIndex > 0 ? pageIndex : 1;

            if (!PagingHelper.SplitSql(sql, out SqlParts parts))
                throw new Exception("Unable to parse SQL statement for paged query");
            long skip = (pageIndex - 1) * pageSize;
            long take = pageSize;
            var databaseType = DatabaseType.MySql;
            if (_context.Database.IsMySql())
            {
                databaseType = DatabaseType.MySql;
            }
            else if (_context.Database.IsSqlServer())
            {
                databaseType = DatabaseType.SqlServer2012;
            }


            var sqlPage = PagingHelper.GetSqlPage(skip, take, parts, databaseType);
            var sqlCount = parts.SqlCount;
            var result = new PageResult<TEntity>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = connection.ExecuteScalar<long>(sqlCount)
            };
            if (result.Total == 0)
            {
                return Task.FromResult(result);
            }
            result.PageCount = result.Total / pageSize;

            if ((result.Total % pageSize) != 0)
                result.PageCount++;

            result.Items = connection.Query<TEntity>(sqlPage, param, trans?.GetDbTransaction())?.ToList();
            return Task.FromResult(result);
        }

        /// <summary>
        /// BeginTransaction
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        /// <summary>
        /// BeginTransactionAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }
        /// <summary>
        /// CommitTransaction
        /// </summary>
        /// <returns></returns>
        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();

        }
        /// <summary>
        /// RollbackTransaction
        /// </summary>
        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        /// <summary>
        /// GetConnection
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return _context.Database.GetDbConnection();
        }
    }


}
