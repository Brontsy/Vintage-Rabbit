using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.Messaging.Messages;
using Vintage.Rabbit.Membership.Repository;

namespace Vintage.Rabbit.Membership.CommandHandlers
{
    public class SaveAddressCommand
    {
        public Address Address { get; private set; }

        public SaveAddressCommand(Address address)
        {
            this.Address = address;
        }
    }

    internal class SaveAddressCommandHandler : ICommandHandler<SaveAddressCommand>
    {
        private IAddressRepository _addressRepository;
        private IMessageService _messageService;

        public SaveAddressCommandHandler(IAddressRepository addressRepository, IMessageService messageService)
        {
            this._addressRepository = addressRepository;
            this._messageService = messageService;
        }

        public void Handle(SaveAddressCommand command)
        {
            this._addressRepository.SaveAddress(command.Address);

            this._messageService.AddMessage(new AddressAddedMessage(command.Address));
        }
    }
}
