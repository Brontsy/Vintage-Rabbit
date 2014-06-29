using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintage.Rabbit.Interfaces.CQRS;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Themes.Entities;
using Vintage.Rabbit.Themes.Repository;

namespace Vintage.Rabbit.Themes.CommandHandlers
{
    public class SaveThemeCommand
    {
        public Theme Theme { get; private set; }

        public IActionBy ActionBy { get; private set; }

        public SaveThemeCommand(Theme theme, IActionBy actionBy)
        {
            this.Theme = theme;
            this.ActionBy = actionBy;
        }
    }

    internal class SaveThemeCommandHandler : ICommandHandler<SaveThemeCommand>
    {
        private IThemeRepository _themeRepository;

        public SaveThemeCommandHandler(IThemeRepository themeRepository)
        {
            this._themeRepository = themeRepository;
        }

        public void Handle(SaveThemeCommand command)
        {
            this._themeRepository.SaveTheme(command.Theme);
        }
    }
}
