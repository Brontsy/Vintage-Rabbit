using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class HireProductListViewModel
    {
        public IList<HireProductListItemViewModel> Products { get; private set; }

        public BreadcrumbsViewModel Breadcrumbs { get; private set; }

        public HireProductListViewModel(IList<HireProduct> products, BreadcrumbsViewModel breadcrumbs)
        {
            this.Products = products.Select(o => new HireProductListItemViewModel(o)).ToList();
            this.Breadcrumbs = breadcrumbs;
        }
    }
}