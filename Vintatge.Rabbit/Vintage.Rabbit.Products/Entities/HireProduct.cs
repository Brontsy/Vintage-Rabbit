using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class HireProduct : Product
    {
        public IList<DateTime> UnavailableDates { get; private set; }

        public HireProduct() 
            :base()
        {
            this.UnavailableDates = new List<DateTime>();
        }
    }
}
