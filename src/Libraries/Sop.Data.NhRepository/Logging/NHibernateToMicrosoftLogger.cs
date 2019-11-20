using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace Sop.Data.NhRepositories.Logging
{
    public class NHibernateToMicrosoftLogger : INHibernateLogger
    {
        private readonly ILogger _msLogger;

        public NHibernateToMicrosoftLogger(ILogger msLogger)
        {
            _msLogger = msLogger ?? throw new ArgumentNullException(nameof(msLogger));
        }

        private static readonly Dictionary<NHibernateLogLevel, LogLevel> MapLevels = new Dictionary<NHibernateLogLevel, LogLevel>
        {
            { NHibernateLogLevel.Trace, LogLevel.Trace },
            { NHibernateLogLevel.Debug, LogLevel.Debug },
            { NHibernateLogLevel.Info, LogLevel.Information },
            { NHibernateLogLevel.Warn, LogLevel.Warning },
            { NHibernateLogLevel.Error, LogLevel.Error },
            { NHibernateLogLevel.Fatal, LogLevel.Critical },
            { NHibernateLogLevel.None, LogLevel.None },
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        public void Log(NHibernateLogLevel logLevel, NHibernateLogValues state, Exception exception)
        {
            //_msLogger.Log(MapLevels[logLevel], 0, new FormattedLogValues(state.Format, state.Args), exception, MessageFormatter);
            //var format = new FormattedLogValues(state.Format, state.Args);

            string Predicate
                (object obj, Exception ex) => string.Concat(obj, ex);
            Func<String, int, bool> predicate1 = (str, index) => str.Length == index;

            _msLogger.Log<object>(MapLevels[logLevel], 0, null, exception, Predicate);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(NHibernateLogLevel logLevel)
        {
            return _msLogger.IsEnabled(MapLevels[logLevel]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static string MessageFormatter(object state, Exception error)
        {
            return state.ToString();
        }
    }
}