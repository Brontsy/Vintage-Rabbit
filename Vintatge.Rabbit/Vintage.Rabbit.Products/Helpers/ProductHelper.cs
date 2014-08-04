using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Products.Helpers
{
    public static class ProductHelper
    {
        public static bool IsCustomisableInvitation(IPurchaseable product)
        {
            return product.Title.ToLower().Contains("customised") && product.Title.ToLower().Contains("invitation");
        }
    }
}

