using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;

namespace Vintage.Rabbit.Logging.Layouts.Json
{
    [Layout("JsonLayout")]
    public class JsonLayout : Layout
    {
        [ArrayParameter(typeof(JsonField), "field")]
        public IList<JsonField> Fields { get; set; }

        public JsonLayout()
        {
            Fields = new List<JsonField>();
        }

        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            var i = 0;
            foreach (var field in Fields)
            {
                var value = field.Layout.Render(logEvent);
                if (!string.IsNullOrEmpty(value))
                {
                    if (i++ != 0)
                        sb.Append(",");

                    sb.Append(" \"" + field.Name + "\" : \"" + value + "\"");
                }
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}