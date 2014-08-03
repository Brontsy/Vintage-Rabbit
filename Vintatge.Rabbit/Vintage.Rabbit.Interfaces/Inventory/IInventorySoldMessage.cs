using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Interfaces.Inventory
{
    public interface IInventorySoldMessage : IMessage
    {
        IInventoryItem InventorySold { get; }
    }
}
