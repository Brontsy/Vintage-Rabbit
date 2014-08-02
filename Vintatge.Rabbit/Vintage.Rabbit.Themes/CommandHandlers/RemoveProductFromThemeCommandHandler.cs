using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.QueryHandlers;
using Vintage.Rabbit.Themes.Repository;

namespace Vintage.Rabbit.Themes.CommandHandlers
{
    public class RemoveProductFromThemeCommand
    {
        public Guid ThemeGuid { get; private set; }

        public Guid ThemeProductGuid { get; private set; }

        public Guid ThemeImageGuid { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public RemoveProductFromThemeCommand(Guid themeGuid, Guid themeImageGuid, Guid themeProductGuid, IActionBy actionBy)
        {
            this.ThemeGuid = themeGuid;
            this.ThemeImageGuid = themeImageGuid;
            this.ThemeProductGuid = themeProductGuid;
            this.ActionBy = actionBy;
        }
    }

    internal class RemoveProductFromThemeCommandHandler : ICommandHandler<RemoveProductFromThemeCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public RemoveProductFromThemeCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(RemoveProductFromThemeCommand command)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(command.ThemeGuid));
            ThemeImage image = theme.Images.First(o => o.Guid == command.ThemeImageGuid);
            var product = image.Products.First(o => o.Guid == command.ThemeProductGuid);

            image.Products.Remove(product);

            this._commandDispatcher.Dispatch(new SaveThemeCommand(theme, command.ActionBy));
        }
    }
}
