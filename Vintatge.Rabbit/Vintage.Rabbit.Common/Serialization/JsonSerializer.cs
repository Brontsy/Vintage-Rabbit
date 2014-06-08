using Newtonsoft.Json;
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
            JsonSerializerSettings jss = new JsonSerializerSettings();

            Newtonsoft.Json.Serialization.DefaultContractResolver dcr = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            dcr.DefaultMembersSearchFlags |= System.Reflection.BindingFlags.NonPublic;
            jss.ContractResolver = dcr;
            jss.TypeNameHandling = TypeNameHandling.All;

            return JsonConvert.SerializeObject(obj, jss);
        }

        public T Deserialize<T>(string s)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings();

            Newtonsoft.Json.Serialization.DefaultContractResolver dcr = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            dcr.DefaultMembersSearchFlags |= System.Reflection.BindingFlags.NonPublic;
            jss.ContractResolver = dcr;
            jss.TypeNameHandling = TypeNameHandling.All;

            return JsonConvert.DeserializeObject<T>(s, jss);
        }
    }
}
