using NLog;

namespace Vintage.Rabbit.Logging.Targets.Syslog
{
    interface ISyslogMessageFormatter
    {
        string Format(Syslog.SyslogFacility facility, Syslog.SyslogSeverity severity, LogEventInfo logEventInfo, string messageContent);
    }
}