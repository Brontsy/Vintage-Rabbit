using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Admin.Models.Pagination
{
    public class PaginationViewModel
    {
        public int TotalResults { get; private set; }

        public int Page { get; private set; }

        public int TotalPages { get; private set; }

        public string RouteName { get; private set; }

        public PaginationViewModel(int page, int totalResults, int itemsPerPage, string routeName)
        {
            this.TotalResults = totalResults;
            this.Page = page;
            this.RouteName = routeName;

            if (itemsPerPage > 0)
            {
                this.TotalPages = totalResults / itemsPerPage;

                if (totalResults % itemsPerPage > 0)
                {
                    this.TotalPages++;
                }
            }
        }
    }
}