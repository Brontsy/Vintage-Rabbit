using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Extensions;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Web.Models.Categories;

namespace Vintage.Rabbit.Web.Models.Themes
{
    public class ThemeLinkViewModel
    {
        public string Title { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.ToUrl(); }
        }

        public bool IncludeHostName { get; private set; }

        public ThemeLinkViewModel(Theme theme, bool includeHostName)
        {
            this.Title = theme.Title;
            this.IncludeHostName = includeHostName;
        }
    }
}