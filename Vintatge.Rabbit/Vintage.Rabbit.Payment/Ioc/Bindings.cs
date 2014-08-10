using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Payment.CommandHandlers;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Payment.QueryHandlers;
using Vintage.Rabbit.Payment.Repository;
using Vintage.Rabbit.Payment.Services;

namespace Vintage.Rabbit.Payment.Ioc
{
    public class Bindings : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GetPayPalPaymentByGuidQueryHandler>().As<IQueryHandler<PayPalPayment, GetPayPalPaymentByGuidQuery>>();
            builder.RegisterType<GetEwayPaymentByAccessCodeQueryHandler>().As<IQueryHandler<EWayPayment, GetEwayPaymentByAccessCodeQuery>>();

            builder.RegisterType<SavePayPalPaymentCommandHandler>().As<ICommandHandler<SavePayPalPaymentCommand>>();

            builder.RegisterType<EwayPaymentProcessedCommandHandler>().As<ICommandHandler<EwayPaymentProcessedCommand>>();
            builder.RegisterType<InitialiseEwayCreditCardPaymentCommandHandler>().As<ICommandHandler<InitialiseEwayCreditCardPaymentCommand>>();
            builder.RegisterType<SaveEwayPaymentCommandHandler>().As<ICommandHandler<SaveEwayPaymentCommand>>();

            builder.RegisterType<EWayPaymentRepository>().As<IEWayPaymentRepository>();
            builder.RegisterType<PayPalRepository>().As<IPayPalRepository>();


            builder.RegisterType<EwayPaymentService>().As<IPaymentGateway>();
            builder.RegisterType<CreditCardService>().As<ICreditCardService>();
            builder.RegisterType<PayPalService>().As<IPayPalService>();

        }
    }
}
