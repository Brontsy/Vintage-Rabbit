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
        public bool? IsAvailable {get; private set;}

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PartyDate { get; private set; }

        public HireProductViewModel(Product product, IList<InventoryItem> inventory, HireDatesViewModel hireDates)
            :base (product)
        {
            this.PartyDate = hireDates.PartyDate;
            this.InventoryCount = new List<SelectListItem>();

            if (hireDates.PartyDate.HasValue)
            {
                int available = inventory.Count(o => o.IsAvailable(hireDates.PartyDate.Value));

                for (int i = 1; i <= available; i++)
                {
                    this.InventoryCount.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                this.IsAvailable = available > 0;
            }
        }
    }
}