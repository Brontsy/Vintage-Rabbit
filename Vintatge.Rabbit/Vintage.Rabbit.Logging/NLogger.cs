using System;
using System.Collections.Generic;
using System.Threading;
using Vintage.Rabbit.Logging.Attributes;
using Vintage.Rabbit.Logging.Extensions;
using NLog;

namespace Vintage.Rabbit.Logging
{
    internal class NLogger : ILogger
    {
        private const string EventId = "eventId";
        private static readonly LogEventInfo nullLogEventInfo;

        private readonly Logger logger;
        private readonly Type type;
        private readonly GetContext preSetTags;
        private readonly LogInfoData info;

        static NLogger()
        {
            nullLogEventInfo = LogEventInfo.CreateNullEvent();
        }

        public NLogger(Type type, GetContext preSetTags = null)
        {
            logger = LogManager.GetLogger(type.FullName);
            this.type = type;
            this.preSetTags = preSetTags;
            this.info = GetLoggingInfo(type);
        }

        #region ILogger Implementation

        public Type Type
        {
            get { return type; }
        }

        public bool IsEnabled(LogType level)
        {
            return logger.IsEnabled(ToLogLevel(level));
        }

        #region Fatal

        public bool IsFatalEnabled
        {
            get { return logger.IsFatalEnabled; }
        }

        public LogEventInfo Fatal(Exception exception, string message)
        {
            if (!logger.IsFatalEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Fatal, null, exception, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Fatal(Exception exception, LogTags tags, string message)
        {
            if (!logger.IsFatalEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Fatal, tags, exception, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Fatal(Exception exception, string message, params object[] args)
        {
            if (!logger.IsFatalEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Fatal, null, exception, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Fatal(Exception exception, LogTags tags, string message, params object[] args)
        {
            if (!logger.IsFatalEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Fatal, tags, exception, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        #endregion

        #region Error

        public bool IsErrorEnabled
        {
            get { return logger.IsErrorEnabled; }
        }

        public LogEventInfo Error(Exception exception, string message)
        {
            if (!logger.IsErrorEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Error, null, exception, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Error(Exception exception, LogTags tags, string message)
        {
            if (!logger.IsErrorEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Error, tags, exception, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Error(Exception exception, string message, params object[] args)
        {
            if (!logger.IsErrorEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Error, null, exception, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Error(Exception exception, LogTags tags, string message, params object[] args)
        {
            if (!logger.IsErrorEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Error, tags, exception, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        #endregion

        #region Warn
        public bool IsWarnEnabled
        {
            get { return logger.IsWarnEnabled; }
        }

        public LogEventInfo Warn(string message)
        {
            if (!logger.IsWarnEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Warn, null, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Warn(LogTags tags, string message)
        {
            if (!logger.IsWarnEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Warn, tags, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Warn(string message, params object[] args)
        {
            if (!logger.IsWarnEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Warn, null, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Warn(LogTags tags, string message, params object[] args)
        {
            if (!logger.IsWarnEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Warn, tags, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        #endregion

        #region Info
        public bool IsInfoEnabled
        {
            get { return logger.IsInfoEnabled; }
        }

        public LogEventInfo Info(string message)
        {
            if (!logger.IsInfoEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Info, null, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Info(LogTags tags, string message)
        {
            if (!logger.IsInfoEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Info, tags, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Info(string message, params object[] args)
        {
            if (!logger.IsInfoEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Info, null, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Info(LogTags tags, string message, params object[] args)
        {
            if (!logger.IsInfoEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Info, tags, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        #endregion

        #region Debug
        public bool IsDebugEnabled
        {
            get { return logger.IsDebugEnabled; }
        }

        public LogEventInfo Debug(string message)
        {
            if (!logger.IsDebugEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Debug, null, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Debug(LogTags tags, string message)
        {
            if (!logger.IsDebugEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Debug, tags, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Debug(string message, params object[] args)
        {
            if (!logger.IsDebugEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Debug, null, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Debug(LogTags tags, string message, params object[] args)
        {
            if (!logger.IsDebugEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Debug, tags, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        #endregion

        #region Trace
        public bool IsTraceEnabled
        {
            get { return logger.IsTraceEnabled; }
        }

        public LogEventInfo Trace(string message)
        {
            if (!logger.IsTraceEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Trace, null, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Trace(LogTags tags, string message)
        {
            if (!logger.IsTraceEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Trace, tags, null, message);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Trace(string message, params object[] args)
        {
            if (!logger.IsTraceEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Trace, null, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }

        public LogEventInfo Trace(LogTags tags, string message, params object[] args)
        {
            if (!logger.IsTraceEnabled)
                return nullLogEventInfo;

            var logEventInfo = EventInfoFor(LogLevel.Trace, tags, null, message, args);
            logger.Log(logEventInfo);
            return logEventInfo;
        }
        #endregion

        #region Log

        public LogEventInfo Log(LogType level, LogTags tags, string message, params object[] args)
        {
            LogEventInfo logEventInfo = null;

            switch (level)
            {
                case LogType.Debug:
                    logEventInfo = Debug(tags, message, args);
                    break;

                case LogType.Trace:
                    logEventInfo = Trace(tags, message, args);
                    break;

                case LogType.Info:
                    logEventInfo = Info(tags, message, args);
                    break;

                case LogType.Warn:
                    logEventInfo = Warn(tags, message, args);
                    break;

                case LogType.Error:
                    logEventInfo = Error(null, tags, message, args);
                    break;

                case LogType.Fatal:
                    logEventInfo = Fatal(null, tags, message, args);
                    break;
            }

            return logEventInfo;
        }

        public LogEventInfo Log(LogType level, LogTags tags, string message)
        {
            return Log(level, tags, message, null);
        }

        public LogEventInfo Log(LogType level, string message, params object[] args)
        {
            return Log(level, null, message, args);
        }

        public LogEventInfo Log(LogType level, string message)
        {
            return Log(level, null, message, null);
        }

        #endregion
       
        #endregion

        private static LogLevel ToLogLevel(LogType level)
        {
            switch (level)
            {
                case LogType.Fatal:
                    return LogLevel.Fatal;
                case LogType.Error:
                    return LogLevel.Error;
                case LogType.Warn:
                    return LogLevel.Warn;
                case LogType.Info:
                    return LogLevel.Info;
                case LogType.Debug:
                    return LogLevel.Debug;
                case LogType.Trace:
                    return LogLevel.Trace;
                default:
                    throw new ArgumentException("Invalid LogType");
            }
        }

        private LogEventInfo EventInfoFor(
            LogLevel logLevel,
            IEnumerable<KeyValuePair<object, object>> tags,
            Exception ex,
            string message,
            params object[] args)
        {
            var logEventInfo = new LogEventInfo(logLevel, logger.Name, Thread.CurrentThread.CurrentCulture, message, args);

            if (ex != null)
                logEventInfo.Exception = ex;

            if (preSetTags != null)
            {
                var tagCollection = preSetTags();
                if (tagCollection != null)
                    foreach (var addTags in tagCollection)
                        logEventInfo.Properties.Add(addTags);
            }

            if (tags != null)
                foreach (var tag in tags)
                    logEventInfo.Properties[tag.Key] = tag.Value;

            object eid;
            if (!logEventInfo.Properties.TryGetValue(EventId, out eid))
                eid = info.EventId;
            logEventInfo.Properties[EventId] = eid;

            return logEventInfo;
        }

        private static LogInfoData GetLoggingInfo(Type type)
        {
            // check for an attribute
            var attributes = type.GetCustomAttributes(typeof(LogInfoAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                var logInfoAttribute = attributes[0] as LogInfoAttribute;
                if (logInfoAttribute != null)
                {
                    // found an attribute
                    return logInfoAttribute.Info;
                }
            }

            // Finally try and calculate an eventID between 10,000 <= X < 65536
          return new LogInfoData(type.ToEventId());
        }
    }
}
