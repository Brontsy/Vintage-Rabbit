
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Admin.Web.Models.Products;

namespace Vintage.Rabbit.Admin.Web.Models.Hire
{
    public class HireProductViewModel
    {
        public int Id { get; private set; }

        public IList<ProductImageViewModel> Images { get; private set; }

        public string Cost { get; private set; }

        public string Title { get; private set; }

        public bool? IsAvailable { get; private set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? StartDate { get; private set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EndDate { get; private set; }

        public string UrlTitle
        {
            get { return this.Title.Replace(" ", "-").ToLower(); }
        }

        public HireProductViewModel(HireProduct product, bool? available, HireDatesViewModel hireDates)
        {
            this.Id = product.Id;
            this.Images = product.Images.Select(o => new ProductImageViewModel(o)).ToList();
            this.Cost = product.Cost.ToString("C");
            this.Title = product.Title;
            this.StartDate = hireDates.StartDate;
            this.EndDate = hireDates.EndDate;
            this.IsAvailable = available;
        }
    }
}