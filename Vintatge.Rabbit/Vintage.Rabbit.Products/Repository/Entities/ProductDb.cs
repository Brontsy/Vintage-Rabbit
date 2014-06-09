using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Products.Enums;

namespace Vintage.Rabbit.Products.Repository.Entities
{
    internal class ProductDb
    {
        public int Id { get; internal set; }

        public Guid Guid { get; internal set; }

        public string Code { get; internal set; }

        public ProductType Type { get; internal set; }

        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public string Keywords { get; internal set; }

        public decimal Price { get; internal set; }

        public bool Featured { get; internal set; }

        public int Inventory { get; internal set; }

        public bool IsFeatured { get; internal set; }

        public DateTime DateCreated { get; internal set; }

        public DateTime DateLastModified { get; internal set; }

        public string UpdatedBy { get; internal set; }

        public string Categories { get; internal set; }

        public string Images { get; internal set; }

        public string HireAvailability { get; internal set; }
    }
}
