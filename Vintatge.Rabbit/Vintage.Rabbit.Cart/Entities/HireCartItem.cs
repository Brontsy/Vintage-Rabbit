using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Carts.Entities
{
    public class HireCartItem : CartItem
    {
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public HireCartItem() { }

        public HireCartItem(int quantity, Product product, DateTime startDate, DateTime endDate)
            : base(quantity, product)
        {
            this.Properties.Add("StartDate", startDate);
            this.Properties.Add("EndDate", endDate);
        }
    }
}
