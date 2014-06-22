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
        int Id { get; }

        Guid Guid { get; }

        Guid MemberGuid { get; }

        IList<IOrderItem> Items { get; }

        decimal Total { get; }
    }

    public interface IOrderItem
    {
        Guid Guid { get; }

        IPurchaseable Product { get; }

        int Quantity { get; }

        decimal Total { get; }

        DateTime DateCreated { get; }

        Dictionary<string, object> Properties { get; }
    }
}
