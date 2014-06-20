using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class HireDatesViewModel
    {
        //[Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        //public DateTime? StartDate { get; private set; }

        //[Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        //public DateTime? EndDate { get; private set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PartyDate { get; private set; }

        public HireDatesViewModel(DateTime? partyDate)//DateTime? startDate, DateTime? endDate)
        {
            //this.StartDate = startDate;
            //this.EndDate = endDate;
            this.PartyDate = partyDate;
        }
    }
}