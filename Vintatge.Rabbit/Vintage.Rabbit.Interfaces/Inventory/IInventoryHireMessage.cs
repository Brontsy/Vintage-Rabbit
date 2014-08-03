using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Inventory
{
    public interface IProductHiredMessage
    {
        Guid ProductGuid { get; }

        int Qty { get; }

        DateTime PartyDate { get; }
    }
}
