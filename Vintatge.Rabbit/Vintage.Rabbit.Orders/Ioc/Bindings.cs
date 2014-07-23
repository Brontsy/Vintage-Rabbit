using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Orders.CommandHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Messaging.Handlers;
using Vintage.Rabbit.Orders.Messaging.Messages;
using Vintage.Rabbit.Orders.QueryHandlers;
using Vintage.Rabbit.Orders.Repository;
using Vintage.Rabbit.Payment.Messaging.Messages;

namespace Vintage.Rabbit.Orders.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetOrderQueryHandler>().As<IQueryHandler<Order, GetOrderQuery>>();
            builder.RegisterType<GetOrdersQueryHandler>().As<IQueryHandler<PagedResult<Order>, GetOrdersQuery>>();
            builder.RegisterType<GetLoyaltyCardQueryHandler>().As<IQueryHandler<LoyaltyCard, GetLoyaltyCardQuery>>();
            builder.RegisterType<GetUnavailableOrderItemsQueryHandler>().As<IQueryHandler<IList<IOrderItem>, GetUnavailableOrderItemsQuery>>();

            builder.RegisterType<SaveOrderCommandHandler>().As<ICommandHandler<SaveOrderCommand>>();
            builder.RegisterType<AddBillingAddressCommandHandler>().As<ICommandHandler<AddBillingAddressCommand>>();
            builder.RegisterType<AddShippingAddressCommandHandler>().As<ICommandHandler<AddShippingAddressCommand>>();
            builder.RegisterType<AddDeliveryAddressCommandHandler>().As<ICommandHandler<AddDeliveryAddressCommand>>();
            builder.RegisterType<AddCartItemsToOrderCommandHandler>().As<ICommandHandler<AddCartItemsToOrderCommand>>();
            builder.RegisterType<ChangeOrdersMemberGuidCommandHandler>().As<ICommandHandler<ChangeOrdersMemberGuidCommand>>();
            builder.RegisterType<RemoveDeliveryAddressCommandHandler>().As<ICommandHandler<RemoveDeliveryAddressCommand>>();
            builder.RegisterType<ApplyDiscountCommandHandler>().As<ICommandHandler<ApplyDiscountCommand>>();
            builder.RegisterType<SaveLoyaltyCardCommandHandler>().As<ICommandHandler<SaveLoyaltyCardCommand>>();

            builder.RegisterType<OrderRepository>().As<IMessageHandler<SaveOrderMessage>>();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>();
            builder.RegisterType<LoyaltyCardRepository>().As<ILoyaltyCardRepository>();


            builder.RegisterType<OrderPaidMessageHandler>().As<IMessageHandler<PaymentCompleteMessage>>();
            builder.RegisterType<LoyaltyCardMessageHandler>().As<IMessageHandler<IOrderPaidMessage>>();
            builder.RegisterType<PayPalErrorMessageHandler>().As<IMessageHandler<PayPalErrorMessage>>();
            builder.RegisterType<CreditCardPaymentCreatedHandler>().As<IMessageHandler<CreditCardPaymentCreatedMessage>>();
            builder.RegisterType<PayPalPaymentCreatedHandler>().As<IMessageHandler<PayPalPaymentCreatedMessage>>();
        }
    }
}
