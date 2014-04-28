using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Serialization;

namespace Vintage.Rabbit.Common.Serialization
{
    internal class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T obj)
        {
            return string.Empty;
        }

        public T Deserialize<T>(string s)
        {
            return default(T);
        }
    }
}
