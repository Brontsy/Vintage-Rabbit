
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Products.CommandHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;


namespace Vintage.Rabbit.Products.Messaging.Handlers
{
    internal class InventorySoldMessageHandler : IMessageHandler<IInventorySoldMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        public InventorySoldMessageHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IInventorySoldMessage message)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(message.InventorySold.ProductGuid));
            product.Inventory--;

            this._commandDispatcher.Dispatch(new SaveProductCommand(product, new InventoryUpdater()));
        }
    }

    public class InventoryUpdater : IActionBy
    {
        public string Email 
        {
            get { return "inventory.updater"; }
        }
    }

}
