using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Admin.Web.Models.Themes
{
    public class ThemeMainImageViewModel
    {
        public ThemeViewModel Theme { get; private set; }

        public Guid? SelectedProductGuid { get; private set; }

        public ThemeMainImageViewModel(Theme theme, Guid? selectedProductGuid)
        {
            this.Theme = new ThemeViewModel(theme);
            this.SelectedProductGuid = selectedProductGuid;
        }
    }
}