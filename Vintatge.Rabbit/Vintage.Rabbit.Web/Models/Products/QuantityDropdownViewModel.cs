using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vintage.Rabbit.Web.Models.Products
{
    public class QuantityDropdownViewModel
    {
        public int Qty { get; set; }

        public IList<SelectListItem> InventoryCount { get; set; }

        public QuantityDropdownViewModel(int totalInventoryAvailable)
        {
            this.InventoryCount = new List<SelectListItem>();
            for(int i = 1; i <= totalInventoryAvailable; i++)
            {
                this.InventoryCount.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
        }
    }
}