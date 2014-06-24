using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Blogs.Entities;

namespace Vintage.Rabbit.Blogs.Repository
{
    internal interface IBlogRepository
    {
        IList<Blog> GetBlogs();

        Blog GetBlogById(int blogId);

        Blog GetBlogByKey(string key);

       Blog SaveBlog(Blog blog);
    }

    internal class BlogRepository : IBlogRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public BlogRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public IList<Blog> GetBlogs()
        {
            List<Blog> blogs = new List<Blog>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var blogResults = connection.Query<dynamic>("Select * From VintageRabbit.Blogs Order By DateCreated Desc");

                foreach (var blog in blogResults)
                {
                    var item = this.ConvertToBlog(blog);
                    blogs.Add(item);
                }

            }

            return blogs;
        }

        public Blog GetBlogById(int blogId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var blogResults = connection.Query<dynamic>("Select * From VintageRabbit.Blogs Where Id = @BlogId Order By DateCreated Desc", new { BlogId = blogId });

                if (blogResults.Any())
                {
                    return this.ConvertToBlog(blogResults.First());
                }
            }

            return null;
        }

        public Blog GetBlogByKey(string key)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var blogResults = connection.Query<dynamic>("Select * From VintageRabbit.Blogs Where [Key] = @Key Order By DateCreated Desc", new { Key = key });

                if (blogResults.Any())
                {
                    return this.ConvertToBlog(blogResults.First());
                }
            }

            return null;
        }

        public Blog SaveBlog(Blog blog)
        {
            string images = this._serializer.Serialize(blog.Images);

            if (blog.Id == 0)
            {
                string sql = @"Insert Into VintageRabbit.Blogs
                                ([Guid], [Key], Title, Content, Author, Images, DateCreated, DateLastModified, LastModifiedBy)
                                Values
                                (@Guid, @Key, @Title, @Content, @Author, @Images, @DateCreated, @DateLastModified, @LastModifiedBy);
                                Select SCOPE_IDENTITY() as BlogId";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Query(sql, new
                    {
                        Guid = blog.Guid,
                        Title = blog.Title,
                        Key = blog.Key,
                        Content = blog.Content,
                        Author = blog.Author,
                        Images = images,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        LastModifiedBy = blog.LastModifiedBy
                    });
                }
            }
            else
            {
                string sql = @"Update VintageRabbit.Products Set [Guid] = @Guid, [Key] = @Key, Title = @Title, Content = @Content, Author = @Author, Images = @Images, DateLastModified = @DateLastModified, LastModifiedBy = @LastModifiedBy
                                Where Id = @BlogId";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        BlogId = blog.Id,
                        Guid = blog.Guid,
                        Title = blog.Title,
                        Key = blog.Key,
                        Content = blog.Content,
                        Author = blog.Author,
                        Images = images,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        LastModifiedBy = blog.LastModifiedBy
                    });
                }
            }

            return blog;
        }
        private Blog ConvertToBlog(dynamic item)
        {
            IList<BlogImage> images = this._serializer.Deserialize<List<BlogImage>>(item.Images);

            Blog blog = new Blog()
            {
                Id = item.Id,
                Guid = item.Guid,
                Key = item.Key,
                Title = item.Title,
                Content = item.Content,
                Images = images,
                Author = item.Author,
                DateCreated = item.DateCreated,
                DateLastModified = item.DateLastModified,
                LastModifiedBy = item.LastModifiedBy
            };

            return blog;
        }

    }
}
