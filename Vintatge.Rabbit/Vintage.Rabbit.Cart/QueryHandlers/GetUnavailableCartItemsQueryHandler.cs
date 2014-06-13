//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Vintage.Rabbit.Interfaces.CQRS;
//using Vintage.Rabbit.Carts.Entities;
//using Vintage.Rabbit.Interfaces.Cache;
//using Vintage.Rabbit.Carts.Repository;
//using Vintage.Rabbit.Caching;
//using Vintage.Rabbit.Inventory.QueryHandlers;

//namespace Vintage.Rabbit.Carts.QueryHandlers
//{
//    public class GetUnavailableCartItemsQuery
//    {
//        public Cart Cart { get; private set; }

//        public GetUnavailableCartItemsQuery(Cart cart)
//        {
//            this.Cart = cart;
//        }
//    }

//    internal class GetUnavailableCartItemsQueryHandler : IQueryHandler<IList<CartItem>, GetUnavailableCartItemsQuery>
//    {
//        private IQueryDispatcher _queryDispatcher;

//        public GetUnavailableCartItemsQueryHandler(IQueryDispatcher queryDispatcher)
//        {
//            this._queryDispatcher = queryDispatcher;
//        }

//        public IList<CartItem> Handle(GetUnavailableCartItemsQuery query)
//        {
//            IList<CartItem> unavailableCartItems = new List<CartItem>();

//            foreach (var cartItem in query.Cart.Items)
//            {
//                if (cartItem.Product.Type == Products.Enums.ProductType.Buy)
//                {
//                    if (!this._queryDispatcher.Dispatch<bool, IsProductAvailableQuery>(new IsProductAvailableQuery(cartItem.Product.Guid)))
//                    {
//                        unavailableCartItems.Add(cartItem);
//                    }
//                }
//                else
//                {
//                    DateTime startDate = DateTime.Parse(cartItem.Properties["StartDate"].ToString());
//                    DateTime endDate = DateTime.Parse(cartItem.Properties["EndDate"].ToString());
//                    if (!this._queryDispatcher.Dispatch<bool, IsProductAvailableForHireQuery>(new IsProductAvailableForHireQuery(cartItem.Product.Guid, startDate, endDate)))
//                    {
//                        unavailableCartItems.Add(cartItem);
//                    }
//                }
//            }

//            return unavailableCartItems;
//        }
//    }
//}
