﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T obj);

        T Deserialize<T>(string s);
    }
}
