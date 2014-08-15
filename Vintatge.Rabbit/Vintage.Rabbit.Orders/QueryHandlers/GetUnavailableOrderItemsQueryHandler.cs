using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;

namespace Vintage.Rabbit.Orders.QueryHandlers
{
    public class GetUnavailableOrderItemsQuery
    {
        public Order Order { get; private set; }

        public DateTime? PartyDate { get; private set; }

        public GetUnavailableOrderItemsQuery(Order order, DateTime? partyDate)
        {
            this.Order = order;
            this.PartyDate = partyDate;
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
                if (orderItem.Product.Type == Common.Enums.ProductType.Buy)
                {
                    IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(orderItem.Product.Guid));

                    if (inventory.Count(o => o.IsAvailable()) < orderItem.Quantity)
                    {
                        unavailableOrderItems.Add(orderItem);
                    }
                }
                else if (orderItem.Product.Type == Common.Enums.ProductType.Hire)
                {
                    IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(orderItem.Product.Guid));

                    if (inventory.Count(o => o.IsAvailable(query.PartyDate.Value)) < orderItem.Quantity)
                    {
                        unavailableOrderItems.Add(orderItem);
                    }
                }
                else if (orderItem.Product.Type == Common.Enums.ProductType.Theme)
                {
                    Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(orderItem.Product.Guid));

                    Dictionary<Guid, int> products = theme.GetProductGuids();

                    // go through each of the themes products and make sure that there is enough inventory available
                    foreach (var productGuid in products)
                    {
                        IList<InventoryItem> inventory = this._queryDispatcher.Dispatch<IList<InventoryItem>, GetInventoryForProductQuery>(new GetInventoryForProductQuery(productGuid.Key));

                        if (inventory.Count(o => o.IsAvailable(query.PartyDate.Value)) < productGuid.Value)
                        {
                            unavailableOrderItems.Add(orderItem);
                        }
                    }
                }
            }

            return unavailableOrderItems;
        }
    }
}
