
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Web;
//using Vintage.Rabbit.Themes.Entities;

//namespace Vintage.Rabbit.Admin.Web.Models.Themes
//{
//    public class ThemeViewModel
//    {
//        public Guid Guid { get; set; }

//        [Required(ErrorMessage = "Please enter the title for the theme")]
//        public string Title { get; set; }

//        [Required(ErrorMessage = "Please enter a description for the theme")]
//        public string Description { get; set; }

//        [Required(ErrorMessage = "Please enter the cost of the theme")]
//        public decimal? Cost { get; set; }

//        public IList<ThemeImageViewModel> Images { get; set; }

//        public ThemeViewModel()
//        {
//            this.Guid = Guid.NewGuid();
//            this.Images = new List<ThemeImageViewModel>();
//        }

//        public ThemeViewModel(Theme theme)
//        {
//            this.Guid = theme.Guid;
//            this.Title = theme.Title;
//            this.Description = theme.Description;
//            this.Cost = theme.Cost;
//            this.Images = theme.Images.Select(o => new ThemeImageViewModel(o)).ToList();
//        }
//    }
//}