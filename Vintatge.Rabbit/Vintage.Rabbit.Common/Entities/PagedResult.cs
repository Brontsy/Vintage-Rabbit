using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Common.Entities
{
    public class PagedResult<T> : List<T>
    {
        public int TotalResults { get; set; }

        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
