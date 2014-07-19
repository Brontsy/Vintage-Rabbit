using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Orders.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Orders.Repository;
using Vintage.Rabbit.Caching;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Orders.Enums;

namespace Vintage.Rabbit.Orders.QueryHandlers
{
    public class GetOrdersQuery
    {
        public OrderStatus Status { get; private set; }

        public int Page { get; private set; }

        public int ResultsPerPage { get; private set; }

        public GetOrdersQuery(OrderStatus status, int page, int resultsPerPage)
        {
            this.Status = status;
            this.Page = page;
            this.ResultsPerPage = resultsPerPage;
        }
    }

    internal class GetOrdersQueryHandler : IQueryHandler<PagedResult<Order>, GetOrdersQuery>
    {
        private IOrderRepository _OrderRepository;

        public GetOrdersQueryHandler(IOrderRepository OrderRepository)
        {
            this._OrderRepository = OrderRepository;
        }

        public PagedResult<Order> Handle(GetOrdersQuery query)
        {
            return this._OrderRepository.GetOrders(query.Status, query.Page, query.ResultsPerPage);
        }
    }
}
