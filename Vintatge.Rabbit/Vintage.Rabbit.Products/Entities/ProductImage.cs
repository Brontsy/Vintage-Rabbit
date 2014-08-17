using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Products.Entities
{
    public class ProductImage : IProductImage
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string SecureUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Url))
                {
                    return this.Url.Replace("http://", "https://");
                }

                return null;
            }
        }

        public string Thumbnail { get; set; }

        public string SecureThumbnail
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Thumbnail))
                {
                    return this.Thumbnail.Replace("http://", "https://");
                }

                return null;
            }
        }

        public ProductImage()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
