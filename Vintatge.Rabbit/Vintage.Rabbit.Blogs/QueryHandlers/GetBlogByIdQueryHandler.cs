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
        private IBlogRepository _blogRepository;

        public GetBlogByIdQueryHandler(IBlogRepository blogRepository)
        {
            this._blogRepository = blogRepository;
        }

        public Blog Handle(GetBlogByIdQuery query)
        {
            return this._blogRepository.GetBlogById(query.Id);
        }
    }
}
