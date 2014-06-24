using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Blogs.Repository;

namespace Vintage.Rabbit.Blogs.QueryHandlers
{
    public class GetBlogByKeyQuery
    {
        public string Key { get; private set; }

        public GetBlogByKeyQuery(string key)
        {
            this.Key = key;
        }
    }

    internal class GetBlogByKeyQueryHandler : IQueryHandler<Blog, GetBlogByKeyQuery>
    {
        private IBlogRepository _blogRepository;

        public GetBlogByKeyQueryHandler(IBlogRepository blogRepository)
        {
            this._blogRepository = blogRepository;
        }

        public Blog Handle(GetBlogByKeyQuery query)
        {
            return this._blogRepository.GetBlogByKey(query.Key);
        }
    }
}
