using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Products.Repository;
using Vintage.Rabbit.Caching;

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class GetCategoriesQuery
    {
    }

    internal class GetCategoriesQueryHandler : IQueryHandler<IList<Category>, GetCategoriesQuery>
    {
        private ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public IList<Category> Handle(GetCategoriesQuery query)
        {
            return this._categoryRepository.GetCategories();
        }
    }
}
