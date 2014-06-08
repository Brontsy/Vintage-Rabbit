﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Models.Buy
{
    public class BuyProductListViewModel
    {
        public IList<ProductListItemViewModel> Products { get; private set; }

        public BreadcrumbsViewModel Breadcrumbs { get; private set; }

        public BuyProductListViewModel(IList<Product> products, BreadcrumbsViewModel breadcrumbs)
        {
            this.Products = products.Select(o => new ProductListItemViewModel(o)).ToList();
            this.Breadcrumbs = breadcrumbs;
        }
    }
}