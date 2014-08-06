//using Autofac.Integration.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Vintage.Rabbit.Carts.Entities;
//using Vintage.Rabbit.Carts.QueryHandlers;
//using Vintage.Rabbit.Interfaces.CQRS;
//using Vintage.Rabbit.Membership.Entities;
//using Vintage.Rabbit.Membership.QueryHandlers;
//using Vintage.Rabbit.Orders.Entities;
//using Vintage.Rabbit.Orders.QueryHandlers;
//using Vintage.Rabbit.Products.Entities;
//using Vintage.Rabbit.Products.QueryHandlers;

//namespace Vintage.Rabbit.Web.ModelBinders
//{
//    [ModelBinderType(typeof(Category))]
//    public class CategoryModelBinder : IModelBinder
//    {
//        private IQueryDispatcher _queryDispatcher;

//        public CategoryModelBinder(IQueryDispatcher queryDispatcher)
//        {
//            this._queryDispatcher = queryDispatcher;
//        }

//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            var categoryName = controllerContext.Controller.ValueProvider.GetValue("categoryName");

//            if (categoryName != null)
//            {
//                return this._queryDispatcher.Dispatch<Category, GetCategoryQuery>(new GetCategoryQuery(categoryName.AttemptedValue));
//            }

//            return null;
//        }
//    }
//}