﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.QueryHandlers;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Controllers
{
    public class BuyController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public BuyController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index(int? page)
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetCategoriesQuery>(new GetCategoriesQuery());

            IList<CategoryViewModel> viewModel = categories.Select(o => new CategoryViewModel(o)).ToList();

            return View("Index", viewModel);
        }

        public ActionResult Category(string categoryName)
        {
            Category category = this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName));
            IList<Product> product = this._queryDispatcher.Dispatch<IList<Product>, GetProductsByCategoryQuery>(new GetProductsByCategoryQuery(category));

            IList<ProductListItemViewModel> viewModel = product.Select(o => new ProductListItemViewModel(o)).ToList();

            return View("ProductList", viewModel);
        }
	}
}