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
    public class GetThemesQuery
    {
        public GetThemesQuery()
        {
        }
    }

    internal class GetThemesQueryHandler : IQueryHandler<IList<Theme>, GetThemesQuery>
    {
        private IThemeRepository _themeRepository;

        public GetThemesQueryHandler(IThemeRepository themeRepository)
        {
            this._themeRepository = themeRepository;
        }

        public IList<Theme> Handle(GetThemesQuery query)
        {
            return this._themeRepository.GetThemes();
        }
    }
}
