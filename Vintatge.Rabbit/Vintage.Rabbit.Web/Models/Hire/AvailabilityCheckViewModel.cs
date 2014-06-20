using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Web.Models.Products;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class AvailabilityCheckViewModel
    {
        public ProductViewModel Product { get; private set; }
        public bool? IsAvailable { get; private set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        //public DateTime? StartDate { get; private set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        //public DateTime? EndDate { get; private set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PartyDate { get; private set; }

        public AvailabilityCheckViewModel(ProductViewModel product, bool? isAvailable, HireDatesViewModel hireDates)
        {
            this.Product = product;
            this.IsAvailable = isAvailable;
            //this.StartDate = hireDates.StartDate;
            //this.EndDate = hireDates.EndDate;
            this.PartyDate = hireDates.PartyDate;
        }
    }
}