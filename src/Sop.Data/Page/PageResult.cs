using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 分页标准输出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>
    {
        #region 构造函数
        /// <summary>
        /// 默认
        /// </summary>
        public PageResult()
        {
        }
        /// <summary>
        /// 分页扩展
        /// </summary>
        /// <param name="allList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageResult(List<T> allList, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;

            this.Total = allList.Count;
            this.Items = allList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.PageCount = Total > PageSize ? Total / pageSize : 1;
            if ((this.Total % pageSize) != 0)
                this.PageCount++;
        }
        /// <summary>
        ///  分页扩展
        /// </summary>
        /// <param name="allList"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageResult(IEnumerable<T> allList, int pageIndex, int pageSize)
        {
            var enumerable = allList.ToList();
            this.Total = enumerable.Count;
            this.Items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.PageCount = this.Total / pageSize;
            if ((this.Total % pageSize) != 0)
                this.PageCount++;
        }
        #endregion
        /// <summary>
        /// 总条数
        /// </summary>
        public long Total { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public long PageIndex { get; set; }
        /// <summary>
        /// 一个显示条数
        /// </summary>
        public long PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public long PageCount { get; set; }

        /// <summary>
        /// 返回实体
        /// </summary>
        public List<T> Items { get; set; }


    }
}