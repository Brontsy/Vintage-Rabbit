using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Entities
{
    public class BuyProduct : Product
    {
        public int InventoryCount { get; set; }

        public BuyProduct() 
            :base()
        {  

        }
    }
}
