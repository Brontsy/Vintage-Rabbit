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
    public class GetEwayPaymentByAccessCodeQuery
    {
        public string AccessCode { get; private set; }

        public GetEwayPaymentByAccessCodeQuery(string accessCode)
        {
            this.AccessCode = accessCode;
        }
    }

    internal class GetEwayPaymentByAccessCodeQueryHandler : IQueryHandler<EWayPayment, GetEwayPaymentByAccessCodeQuery>
    {
        private IEWayPaymentRepository _creditCardRepository;

        public GetEwayPaymentByAccessCodeQueryHandler(IEWayPaymentRepository creditCardRepository)
        {
            this._creditCardRepository = creditCardRepository;
        }

        public EWayPayment Handle(GetEwayPaymentByAccessCodeQuery query)
        {
            return this._creditCardRepository.GetEwayPaymentByAccessCode(query.AccessCode);
        }
    }
}
