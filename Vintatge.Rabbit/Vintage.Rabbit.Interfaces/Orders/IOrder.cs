using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Interfaces.Orders
{
    public interface IOrder
    {
        Guid Id { get; }

        IList<IOrderItem> Items { get; }

        decimal Total { get; }
    }

    public interface IOrderItem
    {
        Guid Id { get; }

        IPurchaseable Product { get; }

        int Quantity { get; }

        decimal Total { get; }

        Dictionary<string, object> Properties { get; }
    }
}
