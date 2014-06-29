
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Inventory.Repository;
using Vintage.Rabbit.Inventory.Entities;

namespace Vintage.Rabbit.Inventory.CommandHandlers
{
    public class AddInventoryCommand
    {
        public Guid ProductGuid { get; private set; }

        public int Quantity { get; private set; }

        public AddInventoryCommand(Guid productGuid, int qty)
        {
            this.ProductGuid = productGuid;
            this.Quantity = qty;
        }
    }

    internal class AddInventoryCommandHandler : ICommandHandler<AddInventoryCommand>
    {
        private ICommandDispatcher _commandDispatcher;

        public AddInventoryCommandHandler(ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddInventoryCommand command)
        {
            for (int i = 0; i < command.Quantity; i++)
            {
                InventoryItem item = new InventoryItem()
                {
                    ProductGuid = command.ProductGuid
                };

                this._commandDispatcher.Dispatch(new SaveInventoryCommand(item));
            }
        }
    }
}
