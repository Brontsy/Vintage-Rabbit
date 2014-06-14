using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Carts.Repository;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Orders.QueryHandlers
{
    public class GetUnavailableOrderItemsQuery
    {
        public Order Order { get; private set; }

        public GetUnavailableOrderItemsQuery(Order order)
        {
            this.Order = order;
        }
    }

    internal class GetUnavailableOrderItemsQueryHandler : IQueryHandler<IList<IOrderItem>, GetUnavailableOrderItemsQuery>
    {
        private IQueryDispatcher _queryDispatcher;

        public GetUnavailableOrderItemsQueryHandler(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public IList<IOrderItem> Handle(GetUnavailableOrderItemsQuery query)
        {
            IList<IOrderItem> unavailableOrderItems = new List<IOrderItem>();

            foreach (var orderItem in query.Order.Items)
            {
                if (orderItem.Product.Type == ProductType.Buy)
                {
                    if (!this._queryDispatcher.Dispatch<bool, IsProductAvailableQuery>(new IsProductAvailableQuery(orderItem.Product.Guid, orderItem.Quantity)))
                    {
                        unavailableOrderItems.Add(orderItem);
                    }
                }
                else if(orderItem.Product.Type == ProductType.Hire)
                {
                    DateTime startDate = DateTime.Parse(orderItem.Properties["StartDate"].ToString());
                    DateTime endDate = DateTime.Parse(orderItem.Properties["EndDate"].ToString());
                    if (!this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(orderItem.Product.Guid, orderItem.Quantity, startDate, endDate)))
                    {
                        unavailableOrderItems.Add(orderItem);
                    }
                }
            }

            return unavailableOrderItems;
        }
    }
}
