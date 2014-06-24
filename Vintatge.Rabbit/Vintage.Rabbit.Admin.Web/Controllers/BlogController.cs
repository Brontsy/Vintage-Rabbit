using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Admin.Web.Models.Blogs;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Blogs.QueryHandlers;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Blogs.CommandHandlers;
using Vintage.Rabbit.Membership.Entities;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class BlogController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public BlogController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public ActionResult Index()
        {
            IList<Blog> blogs = this._queryDispatcher.Dispatch<IList<Blog>, GetBlogsQuery>(new GetBlogsQuery());

            IList<BlogViewModel> viewModel = blogs.Select(o => new BlogViewModel(o)).ToList();

            return View("Index", viewModel);
        }

        public ActionResult Add()
        {
            return this.View("Add", new BlogViewModel());
        }

        public ActionResult Edit(int blogId)
        {
            Blog blog = this._queryDispatcher.Dispatch<Blog, GetBlogByIdQuery>(new GetBlogByIdQuery(blogId));

            return this.View("Add", new BlogViewModel(blog));
        }

        public ActionResult Save(BlogViewModel viewModel, Member member)
        {
            if(this.ModelState.IsValid)
            {
                Blog blog = new Blog();

                if(viewModel.Id != 0)
                {
                    blog = this._queryDispatcher.Dispatch<Blog, GetBlogByIdQuery>(new GetBlogByIdQuery(viewModel.Id));
                }

                blog.Title = viewModel.Title;
                blog.Key = viewModel.Title.ToUrl();
                blog.Content = viewModel.Content;
                blog.Author = viewModel.Author;

                this._commandDispatcher.Dispatch(new SaveBlogCommand(blog, member));
                
                blog = this._queryDispatcher.Dispatch<Blog, GetBlogByKeyQuery>(new GetBlogByKeyQuery(blog.Key));

                return this.RedirectToRoute(Routes.Blog.Edit, new { blogId = blog.Id, key = blog.Key });
            }

            return this.Add();
        }
	}
}