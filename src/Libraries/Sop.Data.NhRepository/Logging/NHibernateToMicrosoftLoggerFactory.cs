using System;
using NHibernate;

namespace Sop.Data.NhRepositories.Logging
{

    public class NHibernateToMicrosoftLoggerFactory : INHibernateLoggerFactory
    {
        private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;

        public NHibernateToMicrosoftLoggerFactory(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public INHibernateLogger LoggerFor(string keyName)
        {
            var msLogger = _loggerFactory.CreateLogger(keyName);
            return new NHibernateToMicrosoftLogger(msLogger);
        }

        INHibernateLogger INHibernateLoggerFactory.LoggerFor(Type type)
        {
            return LoggerFor(type.FullName);
        }
    }
}
