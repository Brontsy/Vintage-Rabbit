using System;
using Vintage.Rabbit.Logging.Layouts.Json;
using Vintage.Rabbit.Logging.Targets.Syslog;
using NLog.Config;

namespace Vintage.Rabbit.Logging
{
    /// <summary>
    /// factory for getting instances of <see cref="ILogger"/>
    /// </summary>
    public static class GetLogger
    {
        static GetLogger()
        {
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("Syslog", typeof(Syslog));
            ConfigurationItemFactory.Default.Layouts.RegisterDefinition("JsonLayout", typeof(JsonLayout));
        }

        /// <summary>
        /// returns an <see cref="ILogger"/> instance for the given type
        /// </summary>
        public static ILogger For(Type type, GetContext ctxCallback = null)
        {
            return new NLogger(type, ctxCallback);
        }

        /// <summary>
        /// returns an <see cref="ILogger"/> instance for the given type
        /// </summary>
        public static ILogger For<T>(GetContext ctxCallback = null)
        {
            return new NLogger(typeof(T), ctxCallback);
        }
    }
}