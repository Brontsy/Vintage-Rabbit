using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Hire
{
    public class HireProductListViewModel
    {
        public IList<HireProductListItemViewModel> Products { get; private set; }

        public HireProductListViewModel(IList<HireProduct> products)
        {
            this.Products = products.Select(o => new HireProductListItemViewModel(o)).ToList();
        }
    }
}