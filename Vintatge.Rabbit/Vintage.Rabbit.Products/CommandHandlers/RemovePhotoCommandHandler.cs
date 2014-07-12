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
using Vintage.Rabbit.Products.Services;

namespace Vintage.Rabbit.Products.CommandHandlers
{
    public class RemovePhotoCommand
    {
        public Product Product { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public Guid PhotoId { get; private set; }

        public RemovePhotoCommand(Product product, IActionBy actionBy, Guid photoId)
        {
            this.Product = product;
            this.ActionBy = actionBy;
            this.PhotoId = photoId;
        }
    }

    internal class RemovePhotoCommandHandler : ICommandHandler<RemovePhotoCommand>
    {
        private IFileStorage _fileStorage;
        private ICommandDispatcher _commandDispatcher;

        public RemovePhotoCommandHandler(ICommandDispatcher commandDispatcher, IMessageService messageService, IFileStorage fileStorage)
        {
            this._fileStorage = fileStorage;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(RemovePhotoCommand command)
        {
            var photo = command.Product.Images.FirstOrDefault(o => o.Id == command.PhotoId);

            if (photo != null)
            {
                this._fileStorage.DeleteFile(photo.Url);
                this._fileStorage.DeleteFile(photo.Thumbnail);

                command.Product.Images.Remove(photo);

                this._commandDispatcher.Dispatch(new SaveProductCommand(command.Product, command.ActionBy));
            }
        }
    }
}
