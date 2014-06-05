using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;

namespace Vintage.Rabbit.Products.CommandHandlers
{
    public class UploadProductPhotoCommand
    {
        public Product Product { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public UploadProductPhotoCommand(Product product, IActionBy actionBy)
        {
            this.Product = product;
            this.ActionBy = actionBy;
        }
    }

    internal class UploadProductPhotoCommandHandler : ICommandHandler<UploadProductPhotoCommand>
    {

        public UploadProductPhotoCommandHandler(ICacheService cacheService, IMessageService messageService)
        {
        }

        public void Handle(UploadProductPhotoCommand command)
        {
            
        }
    }
}
