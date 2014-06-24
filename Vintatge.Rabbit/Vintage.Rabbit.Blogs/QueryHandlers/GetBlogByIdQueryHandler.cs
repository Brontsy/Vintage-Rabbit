using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Common.Http;
using Vintage.Rabbit.Common.Extensions;

namespace Vintage.Rabbit.Blogs.QueryHandlers
{
    public class GetBlogByIdQuery
    {
        public int Id { get; private set; }

        public GetBlogByIdQuery(int blogId)
        {
            this.Id = blogId;
        }
    }

    internal class GetBlogByIdQueryHandler : IQueryHandler<Blog, GetBlogByIdQuery>
    {
        private IHttpWebUtility _httpWebUtility;

        public GetBlogByIdQueryHandler(IHttpWebUtility httpWebUtility)
        {
            this._httpWebUtility = httpWebUtility;
        }

        public Blog Handle(GetBlogByIdQuery query)
        {
            IList<Blog> blogs = new List<Blog>();

            string url = string.Format("http://public-api.wordpress.com/rest/v1/sites/vintagerabbitblog.wordpress.com/posts/{0}", query.Id);

            var response = this._httpWebUtility.Get<WordpressPost>(url, 1000);
            var post = response.Response;

            Blog blog = new Blog();
            blog.Id = post.Id;
            blog.Content = post.Content;
            blog.Title = post.Title;
            blog.Key = post.Title.ToUrl();
            blog.Summary = post.Excerpt;
            blog.Author = post.Author.Name;
            blog.DateCreated = post.Date;
            blog.DateLastModified = post.Modified;

            return blog;
        }
    }
}
