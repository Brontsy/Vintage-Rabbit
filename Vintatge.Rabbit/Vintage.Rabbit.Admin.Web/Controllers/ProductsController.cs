using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Categories;
using Vintage.Rabbit.Admin.Web.Models.Products;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Products.CommandHandlers;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Products.Services;
using Vintage.Rabbit.Common.Extensions;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;
        private IUploadProductImageService _uploadPhotoService;

        public ProductsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IUploadProductImageService uploadPhotoService)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
            this._uploadPhotoService = uploadPhotoService;
        }

        public ActionResult List()
        {
            IList<Product> products = this._queryDispatcher.Dispatch<IList<Product>, GetProductsQuery>(new GetProductsQuery()).OrderBy(o => o.Title).ToList();

            ProductListViewModel viewModel = new ProductListViewModel(products);

            return View("BuyList", viewModel);
        }
      
        public ActionResult Add()
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery());

            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Categories = categories.Select(o => new CategoryViewModel(o)).ToList();

            return this.View("Add", viewModel);
        }

        public ActionResult Edit(int productId)
        {
            Product product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery());

            ProductViewModel viewModel = new ProductViewModel(product, categories);


            return this.View("Add", viewModel);
        }

        public ActionResult Save(ProductViewModel viewModel, Member member)
        {
            if (this.ModelState.IsValid)
            {
                IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery());

                Product product;
                if(viewModel.ProductId == 0)
                {
                    Guid productGuid = Guid.NewGuid();
                    this._commandDispatcher.Dispatch(new CreateProductCommand(productGuid, viewModel.Inventory.Value, member));
                    product = this._queryDispatcher.Dispatch<Product, GetProductByGuidQuery>(new GetProductByGuidQuery(productGuid));
                }
                else
                {
                    product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(viewModel.ProductId));
                }

                product.Code = viewModel.Code;
                product.Title = viewModel.Title;
                product.Description = viewModel.Description;
                product.Keywords = viewModel.Keywords;
                product.Cost = viewModel.Cost.Value;
                product.Type = viewModel.Type.Value;
                product.IsFeatured = viewModel.IsFeatured;
                product.InventoryCount = viewModel.Inventory.Value;

                IEnumerable<int> categoryIds = viewModel.Categories.Where(o => o.Selected).Select(o => o.Id);

                product.Categories = categories.Where(o => categoryIds.Contains(o.Id)).ToList();

                this._commandDispatcher.Dispatch(new SaveProductCommand(product, member));
                  
                // Get Product By Code if productId = 0
                return this.RedirectToRoute(Routes.Products.Edit, new { productId = product.Id, name = product.Title.ToUrl() });

            }

            return this.Add();
        }

        public ActionResult UploadPhoto(int productId, Member member)
        {
            var result = this._uploadPhotoService.UploadFiles(this.Request.Files);

            var product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));
            foreach (var image in result)
            {
                product.Images.Add(image);
            }

            this._commandDispatcher.Dispatch(new SaveProductCommand(product, member));

            return this.PartialView("Photo", new ProductImageViewModel(result[0]));
        }

        public ActionResult RemovePhoto(int productId, Guid photoId, Member member)
        {
            var product = this._queryDispatcher.Dispatch<Product, GetProductByIdQuery>(new GetProductByIdQuery(productId));
            this._commandDispatcher.Dispatch(new RemovePhotoCommand(product, member, photoId));

            return this.RedirectToRoute(Routes.Products.Edit, new { productId = productId, name = product.Title.ToUrl() });
        }
	}
}