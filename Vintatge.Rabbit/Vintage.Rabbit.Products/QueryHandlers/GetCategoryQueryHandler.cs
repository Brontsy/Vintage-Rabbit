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
using Vintage.Rabbit.Common.Enums;

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class GetCategoryQuery
    {
        public string Name { get; set; }

        public ProductType ProductType { get; set; }

        public GetCategoryQuery(string name, ProductType productType)
        {
            this.Name = name;
            this.ProductType = productType;
        }
    }

    internal class GetCategoryQueryHandler : IQueryHandler<Category, GetCategoryQuery>
    {
        private ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public Category Handle(GetCategoryQuery query)
        {
            return this._categoryRepository.GetCategory(query.Name, query.ProductType);
        }
    }
}
