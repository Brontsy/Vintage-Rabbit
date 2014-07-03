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
using Vintage.Rabbit.Products.Repository;

namespace Vintage.Rabbit.Products.CommandHandlers
{
    public class SaveProductCommand
    {
        public Product Product { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public SaveProductCommand(Product product, IActionBy actionBy)
        {
            this.Product = product;
            this.ActionBy = actionBy;
        }
    }

    internal class SaveProductCommandHandler : ICommandHandler<SaveProductCommand>
    {
        private ICacheService _cacheService;
        private IProductRepository _productRepository;

        public SaveProductCommandHandler(ICacheService cacheService, IProductRepository productRepository)
        {
            this._cacheService = cacheService;
            this._productRepository = productRepository;
        }

        public void Handle(SaveProductCommand command)
        {
            Product product = this._productRepository.SaveProduct(command.Product, command.ActionBy);

            IList<string> keys = CacheKeyHelper.Product.Keys(product);

            foreach(string key in keys)
            {
                this._cacheService.Remove(key);
            }
        }
    }
}
