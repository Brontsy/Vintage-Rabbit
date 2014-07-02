using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Admin.Web.Models.Products
{
    public class SearchResultViewModel
    {
        public string label { get; set; }

        public string value { get; set; }

        public SearchResultViewModel(string label, Guid value)
        {
            this.value = value.ToString();
            this.label = label;
        }
    }
}