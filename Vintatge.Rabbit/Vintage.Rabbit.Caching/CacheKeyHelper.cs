using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Caching
{
    public static class CacheKeyHelper
    {
        public static class Cart
        {
            public static string ById(Guid cartId)
            {
                return string.Format("Cart-ById-{0}", cartId);
            }
            public static string ByOwnerId(Guid ownerId)
            {
                return string.Format("Cart-ByOwnerId-{0}", ownerId);
            }
        }

        public static class Product
        {
            public static string ById(int productId)
            {
                return string.Format("Product-ById-{0}", productId);
            }
            public static string ByGuid(Guid guid)
            {
                return string.Format("Product-ByGuid-{0}", guid);
            }

            public static string ByType(ProductType type)
            {
                return string.Format("Products-{0}", type);
            }

            public static string All()
            {
                return "Products";
            }

            public static string Featured()
            {
                return "Products Featured";
            }

            public static IList<string> Keys(IProduct product)
            {
                IList<string> keys = new List<string>();

                keys.Add(ById(product.Id));
                keys.Add(ByGuid(product.Guid));
                keys.Add(ByType(product.Type));
                keys.Add(All());
                keys.Add(Featured());

                return keys;
            }
        }

        public static class Order
        {
            public static string ById(Guid orderId)
            {
                return string.Format("Order-ById-{0}", orderId);
            }
        }

        public static class Member
        {
            public static string ByGuid(Guid orderId)
            {
                return string.Format("Member-ById-{0}", orderId);
            }
            public static string ByEmail(string email)
            {
                return string.Format("Member-ByEmail-{0}", email);
            }
        }

        public static class Inventory
        {
            public static string ByGuid(Guid inventoryGuid)
            {
                return string.Format("Inventory-ByGuid-{0}", inventoryGuid);
            }

            public static string ByProductGuid(Guid productGuid)
            {
                return string.Format("Inventory-ByProductGuid-{0}", productGuid);
            }
        }

    }
}
