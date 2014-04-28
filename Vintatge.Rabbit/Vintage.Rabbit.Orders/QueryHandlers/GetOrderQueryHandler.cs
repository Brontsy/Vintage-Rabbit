using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Orders.Repository;
using Vintage.Rabbit.Caching;

namespace Vintage.Rabbit.Orders.QueryHandlers
{
    public class GetOrderQuery
    {
        public Guid OrderId { get; private set; }
    }

    internal class GetOrderQueryHandler : IQueryHandler<Order, GetOrderQuery>
    {
        private IOrderRepository _OrderRepository;

        public GetOrderQueryHandler(IOrderRepository OrderRepository)
        {
            this._OrderRepository = OrderRepository;
        }

        public Order Handle(GetOrderQuery query)
        {
            return this._OrderRepository.GetOrder(query.OrderId);
        }
    }
}
