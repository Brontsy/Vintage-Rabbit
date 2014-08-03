
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Inventory
{
    public interface IInventoryItem
    {
        Guid ProductGuid { get; }

        bool IsAvailable(DateTime partyDate);
       
        bool IsAvailable();
    }
}
