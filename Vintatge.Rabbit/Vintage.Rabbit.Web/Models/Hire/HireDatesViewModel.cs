using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Hire
{
    public class HireDatesViewModel
    {

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? PartyDate { get; private set; }

        public HireDatesViewModel(DateTime? partyDate)
        {
            this.PartyDate = partyDate;
        }
    }
}