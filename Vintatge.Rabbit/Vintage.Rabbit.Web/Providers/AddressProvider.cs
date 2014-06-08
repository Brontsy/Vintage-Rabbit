using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Web.Models.Payment;

namespace Vintage.Rabbit.Web.Providers
{
    public interface IAddressProvider
    {
        Address SaveShippingAddress(Member member, AddressViewModel viewModel);

        Address SaveBillingAddress(Member member, AddressViewModel viewModel);
    }


    public class AddressProvider : IAddressProvider
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AddressProvider(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public Address SaveShippingAddress(Member member, AddressViewModel viewModel)
        {
            Guid addressId = Guid.NewGuid();

            Address address = new Address(member.Id, addressId, viewModel.Address, viewModel.SuburbCity, viewModel.State, viewModel.Postcode.Value, viewModel.FirstName, viewModel.LastName, viewModel.CompanyName);
            address.IsShippingAddress = true;

            this._commandDispatcher.Dispatch<SaveAddressCommand>(new SaveAddressCommand(address));

            return address;
        }

        public Address SaveBillingAddress(Member member, AddressViewModel viewModel)
        {
            Guid addressId = Guid.NewGuid();

            Address address = new Address(member.Id, addressId, viewModel.Address, viewModel.SuburbCity, viewModel.State, viewModel.Postcode.Value, viewModel.FirstName, viewModel.LastName, viewModel.CompanyName);

            this._commandDispatcher.Dispatch<SaveAddressCommand>(new SaveAddressCommand(address));

            return address;
        }
    }
}