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
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class ThemeAddedToCartViewModel
    {
        public bool AddedToCart {get; private set;}

        public string Title { get; private set; }

        public ThemeAddedToCartViewModel(Theme theme, bool addedToCart)
        {
            this.AddedToCart = addedToCart;
            this.Title = theme.Title;
        }
    }
}