using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.Messaging.Messages;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Carts.CommandHandlers
{
    public class AddThemeToCartCommand
    {
        public Guid OwnerId { get; private set; }

        public Theme Theme { get; private set; }

        public DateTime PartyDate { get; private set; }

        public AddThemeToCartCommand(Guid ownerId, Theme theme, DateTime partyDate)
        {
            this.OwnerId = ownerId;
            this.Theme = theme;
            this.PartyDate = partyDate;
        }
    }

    internal class AddThemeToCartCommandHandler : ICommandHandler<AddThemeToCartCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;

        public AddThemeToCartCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IMessageService messageService)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
        }

        public void Handle(AddThemeToCartCommand command)
        {
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(command.OwnerId));

            cart.AddTheme(command.Theme, command.PartyDate);

            this._commandDispatcher.Dispatch(new SaveCartCommand(cart));
        }
    }
}
