using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;

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
        public bool Handle(IsValidHirePostcodeQuery query)
        {
            return true;
        }
    }
}
