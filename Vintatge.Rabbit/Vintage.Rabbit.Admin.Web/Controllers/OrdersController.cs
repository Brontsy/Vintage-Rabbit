using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Admin.Web.Models.Orders;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Membership.Entities;
using Vintage.Rabbit.Membership.QueryHandlers;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Orders.Enums;
using Vintage.Rabbit.Orders.QueryHandlers;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class OrdersController : Controller
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;
        private int _resultsPerPage = 20;

        public OrdersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }
        
        public ActionResult Index(OrderStatus status, int page = 1)
        {
            PagedResult<Order> orders = this._queryDispatcher.Dispatch<PagedResult<Order>, GetOrdersQuery>(new GetOrdersQuery(status, page, _resultsPerPage));
            IList<OrderViewModel> viewModel = new List<OrderViewModel>();

            foreach(Order order in orders)
            {
                Member member = this._queryDispatcher.Dispatch<Member, GetMemberByIdQuery>(new GetMemberByIdQuery(order.MemberGuid));
                viewModel.Add(new OrderViewModel(order, member));
            }

            return View("Orders", new OrdersPageViewModel(viewModel, status));
        }


        public ActionResult View(Guid orderGuid)
        {
            Order order = this._queryDispatcher.Dispatch<Order, GetOrderQuery>(new GetOrderQuery(orderGuid));
            Member member = this._queryDispatcher.Dispatch<Member, GetMemberByIdQuery>(new GetMemberByIdQuery(order.MemberGuid));

            return this.View("View", new OrderViewModel(order, member));
        }
    }
}