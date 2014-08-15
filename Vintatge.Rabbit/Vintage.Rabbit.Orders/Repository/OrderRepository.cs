using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Common.Serialization;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Interfaces.Products;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Orders.Repository
{
    internal interface IOrderRepository
    {
        Order GetOrder(Guid Orderid);

        PagedResult<Order> GetOrders(OrderStatus status, int page, int resultsPerPage);
    }

    internal class OrderRepository : IOrderRepository, IMessageHandler<SaveOrderMessage>
    {
        private ISerializer _serializer;
        private string _connectionString;
        public OrderRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public PagedResult<Order> GetOrders(OrderStatus status, int page, int resultsPerPage)
        {
            PagedResult<Order> result = new PagedResult<Order>();
            result.PageNumber = page;
            result.ItemsPerPage = resultsPerPage;

            string sql = @"Select * From VintageRabbit.Orders
                            Where [Status] = @Status
                            Order By DateCreated Desc 
                            OFFSET @Offset ROWS FETCH NEXT @ResultsPerPage ROWS ONLY;
                            Select Count(*) From VintageRabbit.Orders Where [Status] = @Status;";

            int offset = (page - 1) * resultsPerPage;

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, new { Status = status.ToString(), Offset = offset, ResultsPerPage = resultsPerPage }))
                {
                    result.AddRange(multi.Read<Order>());
                    result.TotalResults = multi.Read<int>().First();
                }
            }

            return result;
        }

        public Order GetOrder(Guid orderId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var orders = connection.Query<Order>("Select * From VintageRabbit.Orders Where Guid = @Guid", new { Guid = orderId });

                if (orders.Any())
                {
                    Order order = orders.First();
                    order.Items = this.GetOrderItems(order.Guid);
                    return order;
                }
            }

            return null;
        }

        public OrderItem GetOrderItem(Guid orderItemGuid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var orderItems = connection.Query<dynamic>("Select * From VintageRabbit.OrderItems Where Guid = @Guid", new { Guid = orderItemGuid });

                if (orderItems.Any())
                {
                    return this.ConvertToOrderItem(orderItems.First());
                }
            }

            return null;
        }

        public IList<IOrderItem> GetOrderItems(Guid orderGuid)
        {
            IList<IOrderItem> orderItems = new List<IOrderItem>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var items = connection.Query<dynamic>("Select * From VintageRabbit.OrderItems Where OrderGuid = @Guid", new { Guid = orderGuid });

                foreach (var orderItem in items)
                {
                    OrderItem item = this.ConvertToOrderItem(orderItem);

                    orderItems.Add(item);
                }
            }

            return orderItems;
        }

        public void Handle(SaveOrderMessage message)
        {
            Order order = message.Order;

            if (this.GetOrder(order.Guid) == null)
            {
                // insert
                string sql = @"Insert Into VintageRabbit.Orders (Guid, MemberGuid, PaymentMethod, ShippingAddressId, BillingAddressId, DeliveryAddressId, Total, Status, DateCreated, DateLastModified, DatePaid) Values 
                            (@Guid, @MemberGuid, @PaymentMethod, @ShippingAddressId, @BillingAddressId, @DeliveryAddressId, @Total, @Status, @DateCreated, @DateLastModified, @DatePaid)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = order.Guid,
                        MemberGuid = order.MemberGuid,
                        PaymentMethod = order.PaymentMethod.ToString(),
                        ShippingAddressId = order.ShippingAddressId,
                        BillingAddressId = order.BillingAddressId,
                        DeliveryAddressId = order.DeliveryAddressId,
                        Total = order.Total,
                        Status = order.Status.ToString(),
                        DateCreated = order.DateCreated,
                        DateLastModified = DateTime.Now,
                        DatePaid = order.DatePaid

                    });
                }
            }
            else
            {
                //update
                string sql = @"Update VintageRabbit.Orders Set 
                                MemberGuid = @MemberGuid, PaymentMethod = @PaymentMethod, ShippingAddressId = @ShippingAddressId, BillingAddressId = @BillingAddressId, 
                                DeliveryAddressId = @DeliveryAddressId, Total = @Total, Status = @Status, DateLastModified = @DateLastModified, DatePaid = @DatePaid
                                Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = order.Guid,
                        MemberGuid = order.MemberGuid,
                        PaymentMethod = order.PaymentMethod.ToString(),
                        ShippingAddressId = order.ShippingAddressId,
                        BillingAddressId = order.BillingAddressId,
                        DeliveryAddressId = order.DeliveryAddressId,
                        Total = order.Total,
                        Status = order.Status.ToString(),
                        DateLastModified = DateTime.Now,
                        DatePaid = order.DatePaid
                    });
                }
            }

            this.SaveOrderItems(order.Guid, order.Items);
        }

        private void SaveOrderItems(Guid orderGuid, IList<IOrderItem> orderItems)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Execute("Delete From VintageRabbit.OrderItems Where OrderGuid = @OrderGuid", new { OrderGuid = orderGuid });
            }

            foreach (IOrderItem orderItem in orderItems)
            {

                // insert
                string sql = @"Insert Into VintageRabbit.OrderItems (Guid, OrderGuid, ProductTitle, Product, Cost, Quantity, Properties, DateCreated, DateLastModified) Values 
                            (@Guid, @OrderGuid, @ProductTitle, @Product, @Cost, @Quantity, @Properties, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = orderItem.Guid,
                        OrderGuid = orderGuid,
                        ProductTitle = orderItem.Product.Title,
                        Product = this._serializer.Serialize(orderItem.Product),
                        Cost = orderItem.Product.Cost,
                        Quantity = orderItem.Quantity,
                        DateCreated = orderItem.DateCreated,
                        DateLastModified = DateTime.Now,

                    });
                }

            }
        }

        private OrderItem ConvertToOrderItem(dynamic orderItem)
        {
            OrderItem item = new OrderItem()
            {
                Id = orderItem.Id,
                Guid = orderItem.Guid,
                Quantity = orderItem.Quantity,
                Product = this._serializer.Deserialize<IPurchaseable>(orderItem.Product),
                DateCreated = orderItem.DateCreated
            };

            return item;
        }
    }
}
