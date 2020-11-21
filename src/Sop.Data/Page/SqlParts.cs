using System.Text.RegularExpressions;

namespace Sop.Data.Page
{
    /// <summary>
    ///     Presents the SQL parts.
    /// </summary>
    public struct SqlParts
    {
        /// <summary>
        ///  原始的分页SQL
        /// </summary>
        public string Sql;

        /// <summary>
        ///  查询当前sql 多少条
        /// </summary>
        public string SqlCount;

        /// <summary>
        ///   去除的查询语句
        /// </summary>
        public string SqlSelectRemoved;

        /// <summary>
        ///  排序语句（有些必须存在）
        /// </summary>
        public string SqlOrderBy;
    }


    /// <summary>
    /// 
    /// </summary>
    public static class PagingHelper
    {
        public static Regex RegexColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex RegexDistinct = new Regex(@"\ADISTINCT\s",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex RegexOrderBy =
            new Regex(
                @"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\[\]`""\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\[\]`""\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*",
                RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex SimpleRegexOrderBy = new Regex(@"\bORDER\s+BY\s+", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        ///     Splits the given <paramref name="sql" /> into <paramref name="parts" />;
        /// </summary>
        /// <param name="sql">The SQL to split.</param>
        /// <param name="parts">The SQL parts.</param>
        /// <returns><c>True</c> if the SQL could be split; else, <c>False</c>.</returns>
        public static bool SplitSql(string sql, out SqlParts parts)
        {
            parts.Sql = sql;
            parts.SqlSelectRemoved = null;
            parts.SqlCount = null;
            parts.SqlOrderBy = null;

            // Extract the columns from "SELECT <whatever> FROM"
            var m = RegexColumns.Match(sql);
            if (!m.Success)
                return false;

            // Save column list and replace with COUNT(*)
            var g = m.Groups[1];
            parts.SqlSelectRemoved = sql.Substring(g.Index);

            if (RegexDistinct.IsMatch(parts.SqlSelectRemoved))
                parts.SqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " + sql.Substring(g.Index + g.Length);
            else
                parts.SqlCount = sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);

            // Look for the last "ORDER BY <whatever>" clause not part of a ROW_NUMBER expression
            m = SimpleRegexOrderBy.Match(parts.SqlCount);
            if (m.Success)
            {
                g = m.Groups[0];
                parts.SqlOrderBy = g + parts.SqlCount.Substring(g.Index + g.Length);
                parts.SqlCount = parts.SqlCount.Substring(0, g.Index);
            }

            return true;
        }

        /// <summary>
        /// 获取分页的SQL
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="parts"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static string GetSqlPage(long skip, long take, SqlParts parts, DatabaseType databaseType)
        {
            string sqlPage = string.Empty;
            //TODO 必须大于1 ，有些分页要求起始是1  有些是0 
            skip = skip > 0 ? skip : 0;
            take = take > 1 ? take : 1;

            switch (databaseType)
            {
                case DatabaseType.SqlServer2012:
                    if (string.IsNullOrEmpty(parts.SqlOrderBy))
                        parts.Sql += "  ORDER BY (SELECT NULL)";
                    sqlPage = $"{parts.Sql} \n OFFSET @{skip} ROWS FETCH NEXT @{take} ROWS ONLY";
                    break;
                case DatabaseType.SqlServer:
                    // when the query does not contain an "order by", it is very slow
                    if (SimpleRegexOrderBy.IsMatch(parts.SqlSelectRemoved))
                    {
                        var m = SimpleRegexOrderBy.Match(parts.SqlSelectRemoved);
                        if (m.Success)
                        {
                            var g = m.Groups[0];
                            parts.SqlSelectRemoved = parts.SqlSelectRemoved.Substring(0, g.Index);
                        }
                    }
                    if (RegexDistinct.IsMatch(parts.SqlSelectRemoved))
                    {
                        parts.SqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.SqlSelectRemoved + ") peta_inner";
                    }

                    sqlPage =
                        $"SELECT * FROM (SELECT ROW_NUMBER() OVER ({(parts.SqlOrderBy ?? "ORDER BY (SELECT NULL)")}) page_row, {parts.SqlSelectRemoved}) dapper_paged_table WHERE page_row > {skip} AND page_row <= {skip + take }";
                    break;

                case DatabaseType.PostgreSql:
                case DatabaseType.SqLite:
                case DatabaseType.MySql:
                    sqlPage = $"{parts.Sql}\n LIMIT @{skip} OFFSET @{take}";
                    break;
            }

            return sqlPage;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// 
        /// </summary>
        SqlServer,
        /// <summary>
        /// 
        /// </summary>
        PostgreSql,
        /// <summary>
        /// 
        /// </summary>
        SqLite,
        /// <summary>
        /// 
        /// </summary>
        MySql,
        /// <summary>
        /// 
        /// </summary>
        SqlServer2012,
    }
}