using System;
using Vintage.Rabbit.Logging.Attributes;
using NLog;

namespace Vintage.Rabbit.Logging
{
    public interface ILogger
    {
        Type Type { get; }
        
        bool IsEnabled(LogType level);

        /// <summary>
        /// Gets a value indicating whether logging is enabled for the <c>Fatal</c> level.
        /// </summary>
        bool IsFatalEnabled { get; }
        /// <summary>
        /// very serious errors - cannot recover
        /// </summary>
        LogEventInfo Fatal(Exception exception, string message);
        /// <summary>
        /// very serious errors - cannot recover
        /// </summary>
        LogEventInfo Fatal(Exception exception, LogTags tags, string message);
        /// <summary>
        /// very serious errors - cannot recover
        /// </summary>
        LogEventInfo Fatal(Exception exception, string message, params object[] args);
        /// <summary>
        /// very serious errors - cannot recover
        /// </summary>
        LogEventInfo Fatal(Exception exception, LogTags tags, string message, params object[] args);


        /// <summary>
        /// Gets a value indicating whether logging is enabled for the <c>Error</c> level.
        /// </summary>
        bool IsErrorEnabled { get; }
        /// <summary>
        /// error messages - can recover
        /// </summary>
        LogEventInfo Error(Exception exception, string message);
        /// <summary>
        /// error messages - can recover
        /// </summary>
        LogEventInfo Error(Exception exception, LogTags tags, string message);
        /// <summary>
        /// error messages - can recover
        /// </summary>
        LogEventInfo Error(Exception exception, string message, params object[] args);
        /// <summary>
        /// error messages - can recover
        /// </summary>
        LogEventInfo Error(Exception exception, LogTags tags, string message, params object[] args);


        /// <summary>
        /// Gets a value indicating whether logging is enabled for the <c>Warn</c> level.
        /// </summary>
        bool IsWarnEnabled { get; }     
        /// <summary>
        /// temporary failures - can recover
        /// </summary>
        LogEventInfo Warn(string message);
        /// <summary>
        /// temporary failures - can recover
        /// </summary>
        LogEventInfo Warn(LogTags tags, string message);
        /// <summary>
        /// temporary failures - can recover
        /// </summary>
        LogEventInfo Warn(string message, params object[] args);
        /// <summary>
        /// temporary failures - can recover
        /// </summary>
        LogEventInfo Warn(LogTags tags, string message, params object[] args);


        /// <summary>
        /// Gets a value indicating whether logging is enabled for the <c>Info</c> level.
        /// </summary>
        bool IsInfoEnabled { get; }     
        /// <summary>
        /// notifications that clients should see and understand
        /// </summary>
        LogEventInfo Info(string message);
        /// <summary>
        /// notifications that clients should see and understand
        /// </summary>
        LogEventInfo Info(LogTags tags, string message);
        /// <summary>
        /// notifications that clients should see and understand
        /// </summary>
        LogEventInfo Info(string message, params object[] args);
        /// <summary>
        /// notifications that clients should see and understand
        /// </summary>
        LogEventInfo Info(LogTags tags, string message, params object[] args);


        /// <summary>
        /// Gets a value indicating whether logging is enabled for the <c>Debug</c> level.
        /// </summary>
        bool IsDebugEnabled { get; }
        /// <summary>
        /// internal tracing for control flow etc
        /// </summary>
        LogEventInfo Debug(string message);
        /// <summary>
        /// internal tracing for control flow etc
        /// </summary>
        LogEventInfo Debug(LogTags tags, string message);
        /// <summary>
        /// internal tracing for control flow etc
        /// </summary>
        LogEventInfo Debug(string message, params object[] args);
        /// <summary>
        /// internal tracing for control flow etc
        /// </summary>
        LogEventInfo Debug(LogTags tags, string message, params object[] args);


        /// <summary>
        /// Gets a value indicating whether logging is enabled for the <c>Trace</c> level.
        /// </summary>
        bool IsTraceEnabled { get; }
        /// <summary>
        /// detailed internal tracing for control flow etc
        /// </summary>
        LogEventInfo Trace(string message);
        /// <summary>
        /// detailed internal tracing for control flow etc
        /// </summary>
        LogEventInfo Trace(LogTags tags, string message);
        /// <summary>
        /// detailed internal tracing for control flow etc
        /// </summary>
        LogEventInfo Trace(string message, params object[] args);
        /// <summary>
        /// detailed internal tracing for control flow etc
        /// </summary>
        LogEventInfo Trace(LogTags tags, string message, params object[] args);


        /// <summary>
        /// alternative where you pass the log level
        /// </summary>
        LogEventInfo Log(LogType level, string message);
        /// <summary>
        /// alternative where you pass the log level
        /// </summary>
        LogEventInfo Log(LogType level, LogTags tags, string message);
        /// <summary>
        /// alternative where you pass the log level
        /// </summary>
        LogEventInfo Log(LogType level, string message, params object[] args);
        /// <summary>
        /// alternative where you pass the log level
        /// </summary>
        LogEventInfo Log(LogType level, LogTags tags, string message, params object[] args);

    }
}