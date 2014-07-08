using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using NLog;

namespace Vintage.Rabbit.Logging.Targets.Syslog
{
    // http://en.wikipedia.org/wiki/ReportResponse
    // http://tools.ietf.org/html/rfc5424
    public class KiwiSyslogFormatter : ISyslogMessageFormatter
    {
        private static readonly string processName;
        private static readonly int processId;
        private static readonly string dnsHostName;

        static KiwiSyslogFormatter()
        {
            using (var proc = Process.GetCurrentProcess())
            {
                processName = proc.ProcessName;
                processId = proc.Id;
            }
            dnsHostName = Dns.GetHostName();
        }

        public string Format(Syslog.SyslogFacility facility, Syslog.SyslogSeverity severity, LogEventInfo logEventInfo, string messageContent)
        {
            var sb = new StringBuilder(1024);

            // *** PRI VERSION SP TIMESTAMP SP HOSTNAME ***
            var pri = ((int)facility * 8) + (int)severity;
            sb.AppendFormat("<{0}>1 {1} {2}", pri, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK"), dnsHostName);

            // *** SP APP-NAME SP PROCID SP MSGID ***
            sb.AppendFormat(" {0} {1} NLog", processName, processId);

            // *** SP STRUCTURED-DATA ***
            var content = GetFormattedMessage(logEventInfo);
            sb.AppendFormat(" [Data {0}]", content);

            // *** SP MSG ***
            sb.AppendFormat(" {0}", messageContent);

            return sb.ToString().Trim();
        }

        private string GetFormattedMessage(LogEventInfo logEventInfo)
        {
            var content = "Level=\"" + logEventInfo.Level + "\"";
            content += " StatusCode=\"" + StatusCodeFor(logEventInfo) + "\"";

            if (logEventInfo.Properties != null)
            {
                foreach (var kvp in logEventInfo.Properties)
                {
                    content += " " + kvp.Key + "=\"" + kvp.Value + "\"";
                }
            }

            return content;
        }

        private static Syslog.StatusCode StatusCodeFor(LogEventInfo logEventInfo)
        {
            if (logEventInfo.Level == LogLevel.Fatal || logEventInfo.Level == LogLevel.Error)
                return Syslog.StatusCode.Failed;

            return Syslog.StatusCode.OK;
        }
    }
}