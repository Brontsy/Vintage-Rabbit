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
    public class GetPayPalPaymentByGuidQuery
    {
        public Guid Guid { get; private set; }

        public GetPayPalPaymentByGuidQuery(Guid payPalGuid)
        {
            this.Guid = payPalGuid;
        }
    }

    internal class GetPayPalPaymentByGuidQueryHandler : IQueryHandler<PayPalPayment, GetPayPalPaymentByGuidQuery>
    {
        private IPayPalRepository _payPalRepository;

        public GetPayPalPaymentByGuidQueryHandler(IPayPalRepository payPalRepository)
        {
            this._payPalRepository = payPalRepository;
        }

        public PayPalPayment Handle(GetPayPalPaymentByGuidQuery query)
        {
            return this._payPalRepository.GetPayPalPayment(query.Guid);
        }
    }
}
