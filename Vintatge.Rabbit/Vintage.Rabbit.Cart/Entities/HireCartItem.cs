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
        public DateTime PartyDate { get; private set; }

        public HireCartItem() { }

        public HireCartItem(int quantity, Product product, DateTime partyDate)
            : base(quantity, product)
        {
            this.Properties.Add("PartyDate", partyDate);
        }
    }
}
