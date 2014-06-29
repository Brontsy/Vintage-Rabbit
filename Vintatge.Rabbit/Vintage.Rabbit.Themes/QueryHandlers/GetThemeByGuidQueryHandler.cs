using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Interfaces.Cache;
using Vintage.Rabbit.Products.Repository;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.Repository;

namespace Vintage.Rabbit.Themes.QueryHandlers
{
    public class GetThemeByGuidQuery
    {
        public Guid Guid { get; private set; }

        public GetThemeByGuidQuery(Guid guid)
        {
            this.Guid = guid;
        }
    }

    internal class GetThemeByGuidQueryHandler : IQueryHandler<Theme, GetThemeByGuidQuery>
    {
        private IThemeRepository _themeRepository;

        public GetThemeByGuidQueryHandler(IThemeRepository themeRepository)
        {
            this._themeRepository = themeRepository;
        }

        public Theme Handle(GetThemeByGuidQuery query)
        {
            return this._themeRepository.GetThemeByGuid(query.Guid);
        }
    }
}
