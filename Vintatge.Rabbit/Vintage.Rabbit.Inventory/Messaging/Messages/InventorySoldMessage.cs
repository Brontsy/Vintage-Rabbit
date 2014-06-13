using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Inventory;

namespace Vintage.Rabbit.Inventory.Messaging.Messages
{
    internal class InventorySoldMessage : IInventorySoldMessage
    {
        public IList<IInventoryItem> InventorySold { get; set; }

        public InventorySoldMessage() { }

        public InventorySoldMessage(IList<IInventoryItem> inventory)
        {
            this.InventorySold = inventory;
        }
    }
}
