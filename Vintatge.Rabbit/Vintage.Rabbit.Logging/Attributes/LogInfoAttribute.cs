using System;

namespace Vintage.Rabbit.Logging.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LogInfoAttribute : Attribute
    {
        private readonly LogInfoData info;

        public LogInfoAttribute(UInt16 eventId)
        {
            info = new LogInfoData(eventId);
        }

        public LogInfoData Info
        {
            get { return info; }
        }
    }

    public class LogInfoData
    {
        private readonly UInt16 eventId;

        public LogInfoData(UInt16 eventId)
        {
            this.eventId = eventId;
        }

        public UInt16 EventId
        {
            get { return eventId; }
        }
    }
}
