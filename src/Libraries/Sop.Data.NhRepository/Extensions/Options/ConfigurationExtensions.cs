using NHibernate.Cfg;

namespace Sop.Data.NhRepository.Extensions.Options
{
    public static class ConfigurationExtensions
    {
        public static Configuration SetDbIntegration(this Configuration configure)
        {
            //获取netCore appsettings.json
//            var name = Configuration.DefaultHibernateCfgFileName;



            //configure.DataBaseIntegration(db =>
            //    {
            //        db.Dialect<NHibernate.Dialect.MySQL57Dialect>();
            //        db.Driver<NHibernate.Driver.MySqlDataDriver>();


            //        db.ConnectionString =
            //            @"server=127.0.0.1;port=3306;database=Sop;user=root;password=123456;Character Set=utf8;sslmode=none";


            //        db.Timeout = 100;

            //        //*** Testing Settings
            //        db.LogFormattedSql = true;
            //        db.LogSqlInConsole = true;
            //        db.AutoCommentSql = true;
            //    });


            return configure;
        }


        public static Configuration SetDbIntegration(this Configuration configure, string fileName)
        {
            return configure;
        }





    }
}
