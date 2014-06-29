using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Admin.Web.Models.Inventory
{
    public class AddInventoryViewModel
    {
        [Display(Name = "Add inventory")]
        [Required(ErrorMessage = "Please enter a how much inventory you would like added")]
        public int? Qty { get; set; }

        public int ProductId { get; set; }

        public AddInventoryViewModel() { }
    }
}