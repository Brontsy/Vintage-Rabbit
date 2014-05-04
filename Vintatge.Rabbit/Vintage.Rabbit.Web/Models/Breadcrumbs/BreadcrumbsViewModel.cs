using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Models.Breadcrumbs
{
    public class BreadcrumbsViewModel
    {
        public IList<Breadcrumb> Breadcrumbs { get; private set; }

        public BreadcrumbsViewModel()
        {
            this.Breadcrumbs = new List<Breadcrumb>();
        }

        public void Add (string url, string name, bool selected = false)
        {
            this.Breadcrumbs.Add(new Breadcrumb(url, name, selected));
        }
    }

    public class Breadcrumb
    {
        public string Url { get; private set; }

        public string Name { get; private set; }

        public bool Selected { get; private set; }

        public Breadcrumb(string url, string name, bool selected)
        {
            this.Url = url;
            this.Name = name;
            this.Selected = selected;
        }
    }
}