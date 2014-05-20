using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public static string ById(Guid orderId)
            {
                return string.Format("Member-ById-{0}", orderId);
            }
            public static string ByEmail(string email)
            {
                return string.Format("Member-ByEmail-{0}", email);
            }
        }
    }
}
