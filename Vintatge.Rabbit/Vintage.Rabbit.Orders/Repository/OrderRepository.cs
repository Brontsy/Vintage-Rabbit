using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
using Vintage.Rabbit.Orders.Entities;

namespace Vintage.Rabbit.Orders.Repository
{
    internal interface IOrderRepository
    {
        Order GetOrder(Guid Orderid);
    }

    internal class OrderRepository : IMessageHandler<SaveOrderMessage>
    {
        private ISerializer _serializer;

        public OrderRepository(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public Order GetOrder(Guid Orderid)
        {
            return new Entities.Order();
        }

        public void Handle(SaveOrderMessage message)
        {
            string serialized = this._serializer.Serialize(message);

            string sql = "Insert Into Query.Order";
        }
    }
}
