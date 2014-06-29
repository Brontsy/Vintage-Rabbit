
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Themes
{
    public class ThemeProductViewModel
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Guid ProductGuid { get; set; }

        public ThemeProductViewModel(ThemeProduct product)
        {
            this.X = product.X;
            this.Y = product.Y;
            this.ProductGuid = product.ProductGuid;
        }
    }
}