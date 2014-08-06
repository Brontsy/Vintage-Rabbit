using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Controllers
{
    public class ProductController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public ProductController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult ProductLink(Guid productGuid, string categoryName, bool includeHostName = false)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(productGuid));
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName, product.Type));

            if (category == null && product.Categories.Any())
            {
                category = product.Categories.First();
            }

            return this.PartialView("Link", new ProductLinkViewModel(product, category, includeHostName));
        }

        public ActionResult ProductPreviewLink(Guid productGuid, string categoryName)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(productGuid));
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName, product.Type));

            if (category == null && product.Categories.Any())
            {
                category = product.Categories.First();
            }

            return this.PartialView("PreviewLink", new ProductLinkViewModel(product, category, false));
        }
	}
}