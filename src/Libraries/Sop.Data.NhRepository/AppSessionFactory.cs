 using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;
using Sop.Data.Environment;
using Sop.Data.NhRepositories.Caches.DynamicCacheBuster;
using Sop.Data.NhRepositories.Caches.Redis;
 using Sop.Data.NhRepositories.Logging;
 using StackExchange.Redis;
using ISession = NHibernate.ISession;

namespace Sop.Data.NhRepositories
{
    /// <summary>
    /// Session和Transaction管理类，单例类。
    /// </summary>
    public class AppSessionFactory
    { 
        /// <summary>
        /// SessionFactory
        /// </summary>
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger _logger = DiContainer.Resolve<ILogger<AppSessionFactory>>();    

        /// <summary>
        /// 构造器
        /// </summary>
        public AppSessionFactory(Assembly[] assemblies)
        {
            
//            NHibernate.NHibernateLogger.SetLoggersFactory(new NHibernateToMicrosoftLoggerFactory());
            
            //通过Mapping by code加载映射
            var mapper = new ModelMapper();
            foreach (var assembly in assemblies)
            {
                try
                {
                    mapper.AddMappings(assembly.GetExportedTypes());
                }
                catch (Exception)
                {
                    //有些程序集里不包含NH配置信息，会抛异常，捕获但不处理
                }
            }
 
            if (HttpContext.Current == null)
            {
                mapper.CompileMappingForEachExplicitlyAddedEntity().WriteAllXmlMapping();
            }

            var hbmMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configure = new Configuration();
            configure.Configure();
            configure.AddDeserializedMapping(hbmMapping, "");
            configure.CurrentSessionContext<WebSessionContext>();

            
           
            var connectionMultiplexer = DiContainer.Resolve<ConnectionMultiplexer>();
            RedisCacheProvider.SetConnectionMultiplexer(connectionMultiplexer); 
            var options = new RedisCacheProviderOptions()
            {
                Serializer = new NhJsonCacheSerializer()
            };
            RedisCacheProvider.SetOptions(options);
            new CacheBuster().AppendVersionToCacheRegionNames(configure);


            _sessionFactory = configure.BuildSessionFactory();
        }


        /// <summary>
        /// ISession实例
        /// </summary>
        public ISession OpenSession
        {
            get
            {
                ISession session = null;

                //如果当前的HttpContext为空，则从CallContext中获取当前的Session。
                if (HttpContext.Current == null)
                {
                    session =  CallContext<ISession>.GetData(typeof(ISession).FullName);

                    if (session == null || !session.IsOpen)
                    {
                        session = _sessionFactory.OpenSession();
                        CallContext<ISession>.SetData(typeof(ISession).FullName, session);
                    }
                    //if (HttpContext.Current != null && HttpContext.Current.Items.ContainsKey(SessionKey))
                    //{
                    //    //Return the open ISession
                    //    return (ISession)HttpContext.Current.Items[SessionKey];
                    //}
                }
                //从绑定的WebContext获取Session。
                else
                {
                    if (CurrentSessionContext.HasBind(_sessionFactory))
                    {
                        session = _sessionFactory.GetCurrentSession();
                    }

                    if (session == null || !session.IsOpen)
                    {
                        session = _sessionFactory.OpenSession();
                        CurrentSessionContext.Bind(session);
                    }
                }




                return session;
            }
        }

        /// <summary>
        /// 提交事务并关闭Session
        /// </summary>
        public void CloseSession()
        {
            ISession session = null;

            if (HttpContext.Current == null)
            {
                session = CallContext<ISession>.GetData(typeof(ISession).FullName);
            }
            else
            {
                session = CurrentSessionContext.Unbind(_sessionFactory);
            }

            if (session == null || !session.IsOpen)
            {
                return;
            }

            if (session.Transaction != null)
            {
                if (session.Transaction.IsActive)
                {
                    try
                    {
                        session.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        //_logger.Error("Error while committing the transaction.", e);
                        //_logger.LogError("Error while committing the transaction.", e);
                    }
                }

                session.Transaction.Dispose();
            }

            session.Close();
        }




        //const string SessionKey = "NhibernateSessionPerRequest";

        //public static ISession OpenSession()
        //{
        //    var context = HttpContext.Current;

        //    //Check whether there is an already open ISession for this request
        //    if (context != null && context.Items.ContainsKey(SessionKey))
        //    {
        //        //Return the open ISession
        //        return (ISession)context.Items[SessionKey];
        //    }
        //    else
        //    {
        //        //Create a new ISession and store it in HttpContext
        //        var newSession = _factory.OpenSession();
        //        if (context != null)
        //            context.Items[SessionKey] = newSession;

        //        return newSession;
        //    }
        //}



    }
}


