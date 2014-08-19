using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Blogs.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Web.Models.Blogs;

namespace Vintage.Rabbit.Web.Controllers
{
    public class BlogController : Controller
    {
        private IQueryDispatcher _queryDispatcher;

        public BlogController(IQueryDispatcher queryDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
        }

        public ActionResult Index()
        {
            IList<Blog> blogs = this._queryDispatcher.Dispatch<IList<Blog>, GetBlogsQuery>(new GetBlogsQuery());

            IList<BlogViewModel> viewModel = blogs.Select(o => new BlogViewModel(o)).ToList();

            return View("Index", viewModel);
        }

        public ActionResult Categories()
        {
            IList<Category> categories = this._queryDispatcher.Dispatch<IList<Category>, GetBlogCategoriesQuery>(new GetBlogCategoriesQuery());

            IList<CategoryViewModel> viewModel = categories.Select(o => new CategoryViewModel(o)).ToList();

            return this.PartialView("Categories", viewModel);
        }
	}
}