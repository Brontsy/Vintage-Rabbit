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
    public class AddProductToThemeCommand
    {
        public Guid ThemeGuid { get; private set; }

        public Guid ThemeProductGuid { get; private set; }

        public Guid ThemeImageGuid { get; private set; }

        public Guid ProductGuid { get; private set; }

        public int Qty { get; private set; }

        public decimal X { get; private set; }

        public decimal Y { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public AddProductToThemeCommand(Guid themeGuid, Guid themeImageGuid, Guid themeProductGuid, Guid productGuid, int qty, decimal x, decimal y, IActionBy actionBy)
        {
            this.ThemeGuid = themeGuid;
            this.ThemeImageGuid = themeImageGuid;
            this.ThemeProductGuid = themeProductGuid;
            this.ProductGuid = productGuid;
            this.Qty = qty;
            this.X = x;
            this.Y = y;
            this.ActionBy = actionBy;
        }
    }

    internal class AddProductToThemeCommandHandler : ICommandHandler<AddProductToThemeCommand>
    {
        private IQueryDispatcher _queryDispatcher;
        private ICommandDispatcher _commandDispatcher;

        public AddProductToThemeCommandHandler(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this._queryDispatcher = queryDispatcher;
            this._commandDispatcher = commandDispatcher;
        }

        public void Handle(AddProductToThemeCommand command)
        {
            Theme theme = this._queryDispatcher.Dispatch<Theme, GetThemeByGuidQuery>(new GetThemeByGuidQuery(command.ThemeGuid));
            ThemeImage image = theme.Images.First(o => o.Guid == command.ThemeImageGuid);

            image.AddProduct(command.ThemeProductGuid, command.ProductGuid, command.Qty, command.X, command.Y);

            this._commandDispatcher.Dispatch(new SaveThemeCommand(theme, command.ActionBy));
        }
    }
}
