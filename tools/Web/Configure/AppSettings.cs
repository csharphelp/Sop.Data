namespace Web.Configure
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class AppSettings
    {
        public RedisCaching RedisCaching { get; set; }
    }
    /// <summary>
    /// Redis
    /// </summary>
    public class RedisCaching
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 链接信息
        /// </summary>
        public string ConnectionString { get; set; }
    }
}