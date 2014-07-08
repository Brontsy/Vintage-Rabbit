using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;
using NLog.Targets;

namespace Vintage.Rabbit.Logging.Targets.Syslog
{
    [Target("Syslog")]
    public class Syslog : TargetWithLayout
    {
        private readonly ISyslogMessageFormatter formatter;

        private IPAddress address;
        private string syslogServer;
        public string SyslogServer 
        { 
            get { return syslogServer; }
            set
            {
                syslogServer = value;
                address = Dns.GetHostAddresses(syslogServer).FirstOrDefault();
                if (address == null)
                    throw new ApplicationException("Unknown host: " + syslogServer);
            }
        }
        public bool OptimiseOutput { get; set; }
        public int Port { get; set; }
        public SyslogFacility Facility { get; set; }

        public string TargetLayout {get { return Layout.ToString(); } set { Layout = value; } }

        public Syslog()
        {
            SyslogServer = "127.0.0.1";
            Port = 514;
            Facility = SyslogFacility.User;

// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Layout = "${message}";
// ReSharper restore DoNotCallOverridableMethodsInConstructor

            formatter = new KiwiSyslogFormatter();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var severity = SeverityFor(logEvent.Level);
            var msg = WrapUpMessage(Facility, severity, Layout.Render(logEvent), logEvent);
            SendMessage(msg);
        }

        private void SendMessage(byte[] msg)
        {
            using (var udpClient = new UdpClient(address.ToString(), Port))
            {
                udpClient.Send(msg, msg.Length);
            }
        }

        private static SyslogSeverity SeverityFor(LogLevel logLevel)
        {
            if (logLevel == LogLevel.Fatal)
                return SyslogSeverity.Emergency;
            if (logLevel >= LogLevel.Error)
                return SyslogSeverity.Error;
            if (logLevel >= LogLevel.Warn)
                return SyslogSeverity.Warning;
            if (logLevel >= LogLevel.Info)
                return SyslogSeverity.Informational;
            if (logLevel >= LogLevel.Debug)
                return SyslogSeverity.Debug;

            return SyslogSeverity.Notice;
        }

        private byte[] WrapUpMessage(SyslogFacility facility, SyslogSeverity severity, string renderedLayout, LogEventInfo logEventInfo)
        {
            if (OptimiseOutput)
                renderedLayout = renderedLayout.Replace(" ", "");

            var message = formatter.Format(facility, severity, logEventInfo, renderedLayout);

            return Encoding.ASCII.GetBytes(message);
        }

        public enum StatusCode
        {
            OK = 0,
            NotFound = 1,
            Timeout = 2,
            Rejected = 3,
            Unauthorized = 4,
            Failed = 5
        }

        public enum SyslogSeverity
        {
            Emergency,
            Alert,
            Critical,
            Error,
            Warning,
            Notice,
            Informational,
            Debug,
        }

        public enum SyslogFacility
        {
            Kernel,
            User,
            Mail,
            Daemons,
            Authorization,
            Syslog,
            Printer,
            News,
            Uucp,
            Clock,
            Authorization2,
            Ftp,
            Ntp,
            Audit,
            Alert,
            Clock2,
            Local0,
            Local1,
            Local2,
            Local3,
            Local4,
            Local5,
            Local6,
            Local7,
        }
    }
}
