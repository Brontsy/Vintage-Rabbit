using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Payment.Entities;

namespace Vintage.Rabbit.Payment.Repository
{
    internal interface ICreditCardRepository
    {
        CreditCardPayment GetCreditCardPayment(Guid creditCardId);

        CreditCardPayment GetCreditCardPaymentByOrder(IOrder order);

        CreditCardPayment SaveCreditCardPayment(CreditCardPayment creditCardPayment);
    }

    internal class CreditCardRepository : ICreditCardRepository
    {
        private static Dictionary<Guid, CreditCardPayment> _creditCardPayments;

        static CreditCardRepository()
        {
            _creditCardPayments = new Dictionary<Guid, CreditCardPayment>();
        }

        public CreditCardPayment GetCreditCardPayment(Guid creditCardId)
        {
            if (_creditCardPayments.ContainsKey(creditCardId))
            {
                return _creditCardPayments[creditCardId];
            }

            return null;
        }

        public CreditCardPayment GetCreditCardPaymentByOrder(IOrder order)
        {
            foreach(KeyValuePair<Guid, CreditCardPayment> keyValue in _creditCardPayments)
            {
                if(keyValue.Value.Order.Id == order.Id)
                {
                    return keyValue.Value;
                }
            }
          
            return null;
        }

        public CreditCardPayment SaveCreditCardPayment(CreditCardPayment creditCardPayment)
        {
            if (_creditCardPayments.ContainsKey(creditCardPayment.Id))
            {
                _creditCardPayments[creditCardPayment.Id] = creditCardPayment;
            }
            else
            {
                _creditCardPayments.Add(creditCardPayment.Id, creditCardPayment);
            }

            return creditCardPayment;
        }
    }
}
