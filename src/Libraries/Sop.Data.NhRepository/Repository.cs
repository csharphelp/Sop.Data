using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using Sop.Data.Repository;

namespace Sop.Data.NhRepositories
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="T">仓储对应的实体</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private AppSessionFactory AppSessionFactory { get; set; }

        /// <summary>
        /// ISession实例
        /// </summary>
        private ISession Session => AppSessionFactory.OpenSession;

     
 
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="entity">实体</param>
        public object Create(T entity)
        {
            object obj = null;
            using (var transaction = Session.BeginTransaction())
            {
                obj = Session.Save(entity);
                transaction.Commit();
            }

            return obj;
        }

        public Task InsertAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Update(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Update(entity);
                transaction.Commit();
            }
        }

        public void Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        public void Delete(T entity)
        {
            using (var transaction = Session.BeginTransaction())
            {
                Session.Delete(entity);
                transaction.Commit();
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据查询条件批量删除
        /// </summary>
        /// <param name="predicate">查询条件</param>
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            using (var transaction = Session.BeginTransaction())
            {
                //todo 扩展需要测试，目前还没有测试
                QueryableExtensions.Delete(this.Fetch(predicate));
                transaction.Commit();
            }
        }

        public T GetById(object id)
        {
            return Session.Get<T>(id);
        }

        public Task<T> GetByIdAsync(object id)
        {
            return  Session.GetAsync<T>(id);
        }

        /// <summary>
        /// 实体集合
        /// </summary>
        public IQueryable<T> Table => Session.Query<T>();

        public IQueryable<T> TableNoTracking => Session.Query<T>();

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>IQueryable类型的实体集合</returns>
        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? Table : Table.Where(predicate);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns>IQueryable类型的实体集合</returns>
        public IQueryable<T> Fetch(Expression<Func<T, bool>> predicate, Action<OrderTable<T>> order)
        {
            if (order == null)
            {
                return Fetch(predicate);
            }
            var deferrable = new OrderTable<T>(Fetch(predicate));
            order(deferrable);
            return deferrable.Queryable;
        }

      

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">第几页</param>
        public IPageList<T> Gets(Expression<Func<T, bool>> predicate, Action<OrderTable<T>> order, int pageSize, int pageIndex)
        {
            var ts = Fetch(predicate, order);

            var totalCount = ts.ToFuture<T>().Count();
            var pagingTs = ts.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToFuture();

            return new PageList<T>(pagingTs.AsQueryable(), pageIndex, pageSize);
        } 
    }
}