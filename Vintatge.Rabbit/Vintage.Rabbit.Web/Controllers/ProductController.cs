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

	}
}