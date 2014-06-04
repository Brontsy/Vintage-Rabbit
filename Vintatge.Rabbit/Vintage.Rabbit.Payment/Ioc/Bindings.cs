﻿using Autofac;
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
            builder.RegisterType<GetCreditCardPaymentByOrderQueryHandler>().As<IQueryHandler<CreditCardPayment, GetCreditCardPaymentByOrderQuery>>();

            builder.RegisterType<SaveCreditCardPaymentCommandHandler>().As<ICommandHandler<SaveCreditCardPaymentCommand>>();

            builder.RegisterType<CreditCardRepository>().As<ICreditCardRepository>();


            builder.RegisterType<EwayPaymentService>().As<IPaymentGateway>();
            builder.RegisterType<CreditCardService>().As<ICreditCardService>();
        }
    }
}