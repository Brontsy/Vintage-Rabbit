using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Common.Extensions;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class ThemeListItemViewModel
    {
        public Guid Guid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string ImageUrl { get; set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public ThemeListItemViewModel(Theme theme)
        {
            this.Guid = theme.Guid;
            this.Title = theme.Title;
            this.Description = theme.Description;
            this.Cost = theme.Cost;
            this.ImageUrl = theme.Images.First().Url;
        }
    }
}