using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Admin.Web.Models.Products;

namespace Vintage.Rabbit.Admin.Web.Models.Buy
{
    public class BuyProductListViewModel
    {
        public IList<ProductListItemViewModel> Products { get; private set; }

        public BuyProductListViewModel(IList<BuyProduct> products)
        {
            this.Products = products.Select(o => new ProductListItemViewModel(o)).ToList();
        }
    }
}