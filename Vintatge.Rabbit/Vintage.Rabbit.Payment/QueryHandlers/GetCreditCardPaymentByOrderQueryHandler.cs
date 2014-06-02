using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Payment.Repository;
using Vintage.Rabbit.Payment.Entities;
using Vintage.Rabbit.Interfaces.Orders;

namespace Vintage.Rabbit.Payment.QueryHandlers
{
    public class GetCreditCardPaymentByOrderQuery
    {
        public IOrder Order { get; private set; }

        public GetCreditCardPaymentByOrderQuery(IOrder order)
        {
            this.Order = order;
        }
    }

    internal class GetCreditCardPaymentByOrderQueryHandler : IQueryHandler<CreditCardPayment, GetCreditCardPaymentByOrderQuery>
    {
        private ICreditCardRepository _creditCardRepository;

        public GetCreditCardPaymentByOrderQueryHandler(ICreditCardRepository creditCardRepository)
        {
            this._creditCardRepository = creditCardRepository;
        }

        public CreditCardPayment Handle(GetCreditCardPaymentByOrderQuery query)
        {
            return this._creditCardRepository.GetCreditCardPaymentByOrder(query.Order);
        }
    }
}
