using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Orders.Entities
{
    public class LoyaltyCard : IPurchaseable
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Number { get; set; }

        public decimal Discount { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateConsumed { get; set; }

        public LoyaltyCardStatus Status { get; set; }


        public ProductType Type
        {
            get { return ProductType.Discount; }
        }

        public string Title
        {
            get { return "Loyalty Card Discount"; }
        }

        public decimal Cost
        {
            get { return this.Discount * -1; }
        }

        public void Consumed()
        {
            this.DateConsumed = DateTime.Now;
            this.Status = LoyaltyCardStatus.Used;
        }
    }
}
