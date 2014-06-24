using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Blogs.Entities;
using Vintage.Rabbit.Blogs.Repository;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Interfaces.Messaging;

namespace Vintage.Rabbit.Blogs.CommandHandlers
{
    public class SaveBlogCommand
    {
        public Blog Blog { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public SaveBlogCommand(Blog blog, IActionBy actionBy)
        {
            this.Blog = blog;
            this.ActionBy = actionBy;
        }
    }

    internal class SaveBlogCommandHandler : ICommandHandler<SaveBlogCommand>
    {
        private IBlogRepository _blogRepository;

        public SaveBlogCommandHandler(IBlogRepository blogRepository)
        {
            this._blogRepository = blogRepository;
        }

        public void Handle(SaveBlogCommand command)
        {
            command.Blog.LastModifiedBy = command.ActionBy.Email;

            this._blogRepository.SaveBlog(command.Blog);
        }
    }
}
