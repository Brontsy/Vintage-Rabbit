using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.QueryHandlers
{
    public class GetAddressByGuidQuery
    {
        public Guid Id { get; private set; }

        public GetAddressByGuidQuery(Guid id)
        {
            this.Id = id;
        }
    }

    internal class GetAddressByGuidQueryHandler : IQueryHandler<Address, GetAddressByGuidQuery>
    {
        private IAddressRepository _addressRepository;

        public GetAddressByGuidQueryHandler(IAddressRepository addressRepository)
        {
            this._addressRepository = addressRepository;
        }

        public Address Handle(GetAddressByGuidQuery query)
        {
            return this._addressRepository.GetAddress(query.Id);
        }
    }
}
