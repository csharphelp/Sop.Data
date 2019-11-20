using System.Diagnostics;
using NHibernate;

namespace Sop.Data.NhRepositories.Extensions
{
    /// <summary>
    /// 用于查看生成的Sql的拦截器
    /// </summary>
    internal class DebugInterceptor : EmptyInterceptor
    {
        public override global::NHibernate.SqlCommand.SqlString OnPrepareStatement(global::NHibernate.SqlCommand.SqlString sql)
        {
            Debug.WriteLine(sql.ToString());

            return base.OnPrepareStatement(sql);
        }
    }
}