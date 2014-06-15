using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class ChangeCartsMemberGuidCommand
    {
        public Cart Cart { get; private set; }

        public Guid MemberGuid { get; private set; }

        public ChangeCartsMemberGuidCommand(Cart cart, Guid memberGuid)
        {
            this.Cart = cart;
            this.MemberGuid = memberGuid;
        }
    }

    internal class ChangeCartsMemberGuidCommandHandler : ICommandHandler<ChangeCartsMemberGuidCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public ChangeCartsMemberGuidCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(ChangeCartsMemberGuidCommand command)
        {
            Cart cart = command.Cart;
            cart.ChangeMemberGuid(command.MemberGuid);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
