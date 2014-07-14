using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Admin.Web.Models.Products;
using Vintage.Rabbit.Web.Admin.Models.Pagination;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class ProductListViewModel
    {
        public IList<ProductListItemViewModel> Products { get; private set; }

        public PaginationViewModel Pagination { get; set; }

        public ProductListViewModel(IList<Product> products)
        {
            this.Products = products.Select(o => new ProductListItemViewModel(o)).OrderBy(o => o.UrlTitle).ToList();
        }
    }
}