using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Interfaces.Orders
{
    public interface IOrder
    {
        Guid Id { get; }

        decimal Total { get; }
    }
}
