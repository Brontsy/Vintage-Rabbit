
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToUrl(this string value)
        {
            return value.Replace(" ", "-").Replace("\"", string.Empty).ToLower();
        }
    }
}
