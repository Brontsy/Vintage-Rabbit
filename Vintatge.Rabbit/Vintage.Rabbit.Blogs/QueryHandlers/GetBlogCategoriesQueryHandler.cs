using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Blogs.Entities;
using System.ServiceModel.Syndication;
using System.Xml;
using Vintage.Rabbit.Common.Extensions;
using System.Xml.Linq;
using Vintage.Rabbit.Common.Http;

namespace Vintage.Rabbit.Blogs.QueryHandlers
{
    public class GetBlogCategoriesQuery
    {
        public GetBlogCategoriesQuery()
        {
        }
    }

    internal class GetBlogCategoriesQueryHandler : IQueryHandler<IList<Category>, GetBlogCategoriesQuery>
    {
        private IHttpWebUtility _httpWebUtility;

        public GetBlogCategoriesQueryHandler(IHttpWebUtility httpWebUtility)
        {
            this._httpWebUtility = httpWebUtility;
        }

        public IList<Category> Handle(GetBlogCategoriesQuery query)
        {
            IList<Category> categories = new List<Category>();

            string url = "http://public-api.wordpress.com/rest/v1/sites/vintagerabbitblog.wordpress.com/categories?pretty=1";

            var response = this._httpWebUtility.Get<WordpressCategoryCollection>(url, 3000);

            foreach (var wordpressCategory in response.Response.Categories)
            {
                Category category = new Category();
                category.Id = wordpressCategory.Id;
                category.Name = wordpressCategory.Name;
                category.UrlTitle = wordpressCategory.Slug;
                category.PostCount = wordpressCategory.PostCount;

                categories.Add(category);
            }

            return categories;
        }
    }
}
