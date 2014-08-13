using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Web.Models.Breadcrumbs;
using Vintage.Rabbit.Web.Models.Categories;
using Vintage.Rabbit.Web.Models.Products;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Inventory.Entities;
using Vintage.Rabbit.Web.Models.Hire;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class HireProductViewModel : ProductViewModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PartyDate { get; private set; }

        public HireProductViewModel(Product product, HireDatesViewModel hireDates)
            :base (product)
        {
            this.PartyDate = hireDates.PartyDate;
        }
    }
}