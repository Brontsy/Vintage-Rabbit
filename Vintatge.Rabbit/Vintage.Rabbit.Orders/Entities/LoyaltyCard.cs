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

        public Guid? OrderGuid { get; set; }

        public decimal Discount { get; set; }

        public LoyaltyCardType LoyaltyCardType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateConsumed { get; set; }

        public LoyaltyCardStatus Status { get; set; }

        public int Inventory { get { return 1; } }

        public ProductType Type
        {
            get { return ProductType.Discount; }
        }

        public string Title { get; set; }

        public decimal Cost { get; set; }

        public LoyaltyCard() { }

        public LoyaltyCard(Guid guid) 
        {
            this.Guid = guid;
            this.DateCreated = DateCreated;
        }

        public void Consumed(Guid orderGuid)
        {
            this.DateConsumed = DateTime.Now;
            this.Status = LoyaltyCardStatus.Used;
            this.OrderGuid = orderGuid;
        }
    }
}
