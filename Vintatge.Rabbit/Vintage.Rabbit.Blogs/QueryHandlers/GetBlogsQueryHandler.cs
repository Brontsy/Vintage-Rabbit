using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Blogs.Repository;
using System.ServiceModel.Syndication;
using System.Xml;
using Vintage.Rabbit.Common.Extensions;
using System.Xml.Linq;
using Vintage.Rabbit.Common.Http;

namespace Vintage.Rabbit.Blogs.QueryHandlers
{
    public class GetBlogsQuery
    {
        public GetBlogsQuery()
        {
        }
    }

    internal class GetBlogsQueryHandler : IQueryHandler<IList<Blog>, GetBlogsQuery>
    {
        private IBlogRepository _blogRepository;
        private IHttpWebUtility _httpWebUtility;

        public GetBlogsQueryHandler(IBlogRepository blogRepository, IHttpWebUtility httpWebUtility)
        {
            this._blogRepository = blogRepository;
            this._httpWebUtility = httpWebUtility;
        }

        public IList<Blog> Handle(GetBlogsQuery query)
        {
            IList<Blog> blogs = new List<Blog>();

            string url = "http://public-api.wordpress.com/rest/v1/sites/vintagerabbitblog.wordpress.com/posts?status=any&number=5&pretty=1";

            var response = this._httpWebUtility.Get<WordpressBlog>(url, 1000);

            foreach (var post in response.Response.Posts)
            {
                Blog blog = new Blog();
                blog.Id = post.Id;
                blog.Content = post.Content;
                blog.Title = post.Title;
                blog.Summary = post.Summary;
                blog.Author = post.Author.Name;
                blog.DateCreated = post.Date;
                blog.DateLastModified = post.Modified;

                blogs.Add(blog);
            }

            return blogs;
        }
    }

    internal class WordpressBlog
    {
        public IList<WordpressPost> Posts { get; set; }
    }

    internal class WordpressPost
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public WordpressAuthor Author { get; set; }

        public DateTime Date { get; set; }

        public DateTime Modified { get; set; }
    }

    internal class WordpressAuthor
    {
        public string Name { get; set; }
    }
}
