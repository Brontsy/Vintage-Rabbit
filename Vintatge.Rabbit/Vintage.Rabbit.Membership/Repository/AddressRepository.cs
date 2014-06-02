using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;

namespace Vintage.Rabbit.Membership.Repository
{
    internal interface IAddressRepository
    {
        Address GetAddress(Guid addressId);

        Address SaveAddress(Address address);
    }

    internal class AddressRepository : IAddressRepository
    {
        private ISerializer _serializer;
        private static Dictionary<Guid, Address> _address;

        static AddressRepository()
        {
            _address = new Dictionary<Guid, Address>();
        }

        public AddressRepository(ISerializer serializer)
        {
            this._serializer = serializer;
        }

        public Address GetAddress(Guid addressId)
        {
            if (_address.ContainsKey(addressId))
            {
                return _address[addressId];
            }

            return null;
        }
        
        public Address SaveAddress(Address address)
        {
            if (_address.ContainsKey(address.Id))
            {
                _address[address.Id] = address;
            }
            else
            {
                _address.Add(address.Id, address);
            }

            return address;
        }
    }
}
