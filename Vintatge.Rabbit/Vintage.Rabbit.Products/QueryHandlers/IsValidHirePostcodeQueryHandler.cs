using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Repository;

namespace Vintage.Rabbit.Products.QueryHandlers
{
    public class IsValidHirePostcodeQuery
    {
        public string Postcode { get; private set; }
        public IsValidHirePostcodeQuery(string postcode)
        {
            this.Postcode = postcode;
        }
    }

    internal class IsValidHirePostcodeQueryHandler : IQueryHandler<bool, IsValidHirePostcodeQuery>
    {
        private IPostcodeRepository _postcodeRepository;

        public IsValidHirePostcodeQueryHandler(IPostcodeRepository postcodeRepository)
        {
            this._postcodeRepository = postcodeRepository;
        }

        public bool Handle(IsValidHirePostcodeQuery query)
        {
            return _postcodeRepository.GetValidPostcodes().Contains(query.Postcode);
        }
    }
}
