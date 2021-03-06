﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Enums;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Web.Models.Payment;

namespace Vintage.Rabbit.Web.Providers
{
    public interface IAddressProvider
    {
        Address SaveShippingAddress(Member member, AddressViewModel viewModel);

        Address SaveBillingAddress(Member member, AddressViewModel viewModel, string email);

        Address SaveDeliveryAddress(Member member, AddressViewModel viewModel, string phoneNumber);
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
            Guid addressId = viewModel.Guid;

            Address address = new Address(member.Guid, AddressType.Shipping, addressId, viewModel.Address, viewModel.SuburbCity, viewModel.State, viewModel.Postcode.Value, viewModel.FirstName, viewModel.LastName, null, null, viewModel.CompanyName);

            this._commandDispatcher.Dispatch<SaveAddressCommand>(new SaveAddressCommand(address));

            return address;
        }

        public Address SaveBillingAddress(Member member, AddressViewModel viewModel, string email)
        {
            Guid addressId = viewModel.Guid;

            Address address = new Address(member.Guid, AddressType.Billing, addressId, viewModel.Address, viewModel.SuburbCity, viewModel.State, viewModel.Postcode.Value, viewModel.FirstName, viewModel.LastName, email, null, viewModel.CompanyName);

            this._commandDispatcher.Dispatch<SaveAddressCommand>(new SaveAddressCommand(address));

            return address;
        }

        public Address SaveDeliveryAddress(Member member, AddressViewModel viewModel, string phoneNumber)
        {
            Guid addressId = viewModel.Guid;

            Address address = new Address(member.Guid, AddressType.Delivery, addressId, viewModel.Address, viewModel.SuburbCity, viewModel.State, viewModel.Postcode.Value, viewModel.FirstName, viewModel.LastName, null, phoneNumber, viewModel.CompanyName);

            this._commandDispatcher.Dispatch<SaveAddressCommand>(new SaveAddressCommand(address));

            return address;
        }
    }
}