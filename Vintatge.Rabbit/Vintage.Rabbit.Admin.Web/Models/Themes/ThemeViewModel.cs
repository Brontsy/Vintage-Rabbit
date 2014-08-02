using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Products.Entities;
using System.ComponentModel.DataAnnotations;

namespace Vintage.Rabbit.Admin.Web.Models.Themes
{
    public class ThemeViewModel
    {
        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Please enter the title for the theme")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a description for the theme")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the included items for the theme")]
        public string IncludedItems { get; set; }

        public IList<ThemeImageViewModel> Images { get; set; }

        [Required(ErrorMessage = "Please enter the cost of the theme")]
        public decimal? Cost { get; set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public ThemeViewModel()
        {
            this.Guid = Guid.NewGuid();
            this.Images = new List<ThemeImageViewModel>();
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