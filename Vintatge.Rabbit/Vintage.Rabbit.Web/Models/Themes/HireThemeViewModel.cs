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
    public class HireThemeViewModel : ThemeViewModel
    {
        public bool? IsAvailable {get; private set;}

        public DateTime? PartyDate { get; private set; }

        public HireThemeViewModel(Theme theme, IList<Product> products, IList<InventoryItem> inventory, HireDatesViewModel hireDates)
            : base(theme, products)
        {
            this.PartyDate = hireDates.PartyDate;
            this.IsAvailable = true;

            if (hireDates.PartyDate.HasValue)
            {
                if (inventory.Count(o => o.IsAvailable(hireDates.PartyDate.Value)) == 0)
                {
                    this.IsAvailable = false;
                }
            }
        }
    }
}