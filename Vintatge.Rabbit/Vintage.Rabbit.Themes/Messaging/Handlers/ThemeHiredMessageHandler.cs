
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Inventory;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Orders;
using Vintage.Rabbit.Parties.Entities;
using Vintage.Rabbit.Parties.QueryHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.Messaging.Messages;
using Vintage.Rabbit.Themes.QueryHandlers;


namespace Vintage.Rabbit.Themese.Messaging.Handlers
{
    internal class ThemeHiredMessageHandler : IMessageHandler<IOrderPaidMessage>
    {
        private ICommandDispatcher _commandDispatcher;
        private IMessageService _messageService;
        private IQueryDispatcher _queryDispatcher;

        public ThemeHiredMessageHandler(IMessageService messageService, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
            this._messageService = messageService;
            this._queryDispatcher = queryDispatcher;
        }

        public void Handle(IOrderPaidMessage message)
        {
            foreach (var orderItem in message.Order.Items.Where(o => o.Product.Type == Common.Enums.ProductType.Theme))
            {
                Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(orderItem.Product.Guid));

                IList<ThemeProduct> products = new List<ThemeProduct>();

                foreach (var image in theme.Images)
                {
                    foreach (var product in image.Products)
                    {
                        if (!products.Any(o => o.ProductGuid == product.Guid))
                        {
                            products.Add(product);
                        }
                    }
                }

                Party party = this._queryDispatcher.Dispatch<Party, GetPartyByOrderGuidQuery>(new GetPartyByOrderGuidQuery(message.Order.Guid));

                if (party == null)
                {
                    // TODO: HANDLE BIG PROBLEM BECAUSE WE HAVE HIRED A THEME BUT NO PARTY RECORD WAS CREATED
                }
                else
                {
                    foreach (var themeProduct in products)
                    {
                        this._messageService.AddMessage<IProductHiredMessage>(new ProductHiredMessage() { ProductGuid = themeProduct.ProductGuid, Qty = themeProduct.Qty, PartyDate = party.PartyDate });
                    }
                }
            }
        }

        private IList<Guid> GetProductGuids(Theme theme)
        {
            IList<Guid> productGuids = new List<Guid>();

            foreach (var image in theme.Images)
            {
                foreach (var product in image.Products)
                {
                    productGuids.Add(product.ProductGuid);
                }
            }

            return productGuids;
        }
    }
}
