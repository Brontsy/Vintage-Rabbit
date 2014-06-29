
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Themes
{
    public class ThemeImageViewModel
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public ThemeImageViewModel(ThemeImage image)
        {
            this.Guid = image.Guid;
            this.Url = image.Url;
            this.ThumbnailUrl = image.ThumbnailUrl;
        }
    }
}