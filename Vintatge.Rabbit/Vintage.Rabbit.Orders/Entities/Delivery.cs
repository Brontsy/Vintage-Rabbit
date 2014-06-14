using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Orders.Entities
{
    public class Delivery : IPurchaseable
    {
        public int Id
        {
            get { return 0; }
        }

        public Guid Guid
        {
            get { return new Guid("721F2AF3-4E03-41E0-BA0A-5CAFC0290651"); }
        }

        public ProductType Type
        {
            get { return ProductType.Delivery; }
        }

        public string Title { get; private set; }

        public int Inventory
        {
            get { return 0; }
        }

        public decimal Cost { get; private set; }

        public Delivery(string title, decimal cost)
        {
            this.Title = title;
            this.Cost = cost;
        }
    }
}
