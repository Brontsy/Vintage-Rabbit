using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Products.Entities;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class ThemeViewModel
    {
        public Guid Guid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IncludedItems { get; set; }

        public IList<ThemeImageViewModel> Images { get; set; }

        public decimal Cost { get; set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public ThemeViewModel(Theme theme, IList<Product> products)
        {
            this.Guid = theme.Guid;
            this.Title = theme.Title;
            this.Description = theme.Description;
            this.IncludedItems = theme.IncludedItems;
            this.Cost = theme.Cost;
            this.Images = theme.Images.Select(o => new ThemeImageViewModel(o, products)).ToList();

        }
    }
}