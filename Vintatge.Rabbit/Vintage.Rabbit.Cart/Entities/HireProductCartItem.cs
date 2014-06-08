using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class HireProductCartItem : ProductCartItem
    {
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public HireProductCartItem() { }

        public HireProductCartItem(Product product, DateTime startDate, DateTime endDate) 
            :base(product)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}
