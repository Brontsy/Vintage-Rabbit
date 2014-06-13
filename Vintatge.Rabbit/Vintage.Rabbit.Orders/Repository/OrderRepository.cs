using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Common.Serialization;

namespace Vintage.Rabbit.Orders.Repository
{
    internal interface IOrderRepository
    {
        Order GetOrder(Guid Orderid);
    }

    internal class OrderRepository : IOrderRepository, IMessageHandler<SaveOrderMessage>
    {
        private ISerializer _serializer;
        private static Dictionary<Guid, Order> _orders;

        static OrderRepository()
        {
            _orders = new Dictionary<Guid, Order>();
        }

        public OrderRepository(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public Order GetOrder(Guid orderId)
        {
            if (_orders.ContainsKey(orderId))
            {
                return _orders[orderId];
            }

            return null;
        }

        public void Handle(SaveOrderMessage message)
        {
            if (_orders.ContainsKey(message.Order.Id))
            {
                _orders[message.Order.Id] = message.Order;
            }
            else
            {
                _orders.Add(message.Order.Id, message.Order);
            }
        }
    }
}
