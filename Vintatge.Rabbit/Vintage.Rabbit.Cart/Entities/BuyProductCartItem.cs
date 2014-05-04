using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class BuyProductCartItem : ProductCartItem
    {
        public BuyProductCartItem(BuyProduct product)
            : base(product)
        {  

        }
    }
}
