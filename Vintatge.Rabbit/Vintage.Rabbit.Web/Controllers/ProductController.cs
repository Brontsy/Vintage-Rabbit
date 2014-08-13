using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Carts.Entities;
using Vintage.Rabbit.Carts.QueryHandlers;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Inventory.QueryHandlers;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Hire;
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

        public ActionResult QuantityDropdown(Member member, HireDatesViewModel hireDates, Guid productGuid)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(productGuid));
            Cart cart = this._queryDispatcher.Dispatch<Cart, GetCartByOwnerIdQuery>(new GetCartByOwnerIdQuery(member.Guid));

            int quantityInCart = 0;
            if(cart.Items.Any(o => o.Product.Guid == productGuid))
            {
                quantityInCart = cart.Items.Where(o => o.Product.Guid == productGuid).Sum(o => o.Quantity);
            }

            int totalInventoryAvailable = product.Inventory;

            if(product.Type == ProductType.Hire && hireDates != null && hireDates.PartyDate.HasValue)
            {
                totalInventoryAvailable = this._queryDispatcher.Dispatch<int, CountInventoryAvailableForHireQuery>(new CountInventoryAvailableForHireQuery(productGuid, hireDates.PartyDate.Value));
            }

            QuantityDropdownViewModel viewModel = new QuantityDropdownViewModel(totalInventoryAvailable - quantityInCart);

            return this.PartialView("QuantityDropdown", viewModel);
        }
	}
}