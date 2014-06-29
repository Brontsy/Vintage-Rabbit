
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
using Vintage.Rabbit.Inventory.QueryHandlers;

namespace Vintage.Rabbit.Inventory.CommandHandlers
{
    public class DeleteInventoryCommand
    {
        public Guid Guid { get; private set; }
        
        public DeleteInventoryCommand(Guid inventoryGuid)
        {
            this.Guid = inventoryGuid;
        }
    }

    internal class DeleteInventoryCommandHandler : ICommandHandler<DeleteInventoryCommand>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public DeleteInventoryCommandHandler(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(DeleteInventoryCommand command)
        {
            InventoryItem item = this._queryDispatcher.Dispatch<InventoryItem, GetInventoryByGuidQuery>(new GetInventoryByGuidQuery(command.Guid));
            if (item != null)
            {
                item.Deleted();
                this._commandDispatcher.Dispatch(new SaveInventoryCommand(item));
            }
        }
    }
}
