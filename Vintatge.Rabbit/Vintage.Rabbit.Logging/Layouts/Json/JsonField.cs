using NLog.Config;
using NLog.Layouts;

namespace Vintage.Rabbit.Logging.Layouts.Json
{
    [NLogConfigurationItem]
    public class JsonField
    {
        public JsonField()
        {
        }

        public JsonField(string name, Layout layout)
        {
            Name = name;
            Layout = layout;
        }

        [RequiredParameter]
        public string Name { get; set; }

        [RequiredParameter]
        public Layout Layout { get; set; }
    }
}