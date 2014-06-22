using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Emails.Messaging.Handlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Emails.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<SendInvoiceEmailMessageHandler>().As<IMessageHandler<IOrderPaidMessage>>();
        }
    }
}
