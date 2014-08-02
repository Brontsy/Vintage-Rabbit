using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Themes.Entities;

namespace Vintage.Rabbit.Themes.Repository
{
    internal interface IThemeRepository
    {
        IList<Theme> GetThemes();

        Theme GetThemeByGuid(Guid theme);

        Theme SaveTheme(Theme theme);
    }

    internal class ThemeRepository : IThemeRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public ThemeRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public IList<Theme> GetThemes()
        {
            IList<Theme> themes = new List<Theme>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var themeResults = connection.Query<dynamic>("Select * From VintageRabbit.Themes", new { });

                foreach (var theme in themeResults)
                {
                    Theme themeItem = this.ConvertToTheme(theme);
                    themes.Add(themeItem);
                }

            }

            return themes;
        }
        public Theme GetThemeByGuid(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var themeResults = connection.Query<dynamic>("Select * From VintageRabbit.Themes Where Guid = @Guid", new { Guid = guid });

                if(themeResults.Any())
                {
                    return this.ConvertToTheme(themeResults.First());
                }

            }

            return null;
        }

        public Theme SaveTheme(Theme theme)
        {
            if (this.GetThemeByGuid(theme.Guid) == null)
            {
                string sql = "Insert Into VintageRabbit.Themes (Guid, Title, Description, IncludedItems, Cost, Images, DateCreated, DateLastModified) Values (@Guid, @Title, @Description, @IncludedItems, @Cost, @Images, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = theme.Guid,
                        Title = theme.Title,
                        Description = theme.Description,
                        IncludedItems = theme.IncludedItems,
                        Cost = theme.Cost,
                        Images = this._serializer.Serialize(theme.Images),
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                    });
                }
            }
            else
            {
                string sql = "Update VintageRabbit.Themes Set Title = @Title, Description = @Description, IncludedItems = @IncludedItems, Cost = @Cost, Images = @Images, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = theme.Guid,
                        Title = theme.Title,
                        Description = theme.Description,
                        IncludedItems = theme.IncludedItems,
                        Cost = theme.Cost,
                        Images = this._serializer.Serialize(theme.Images),
                        DateLastModified = DateTime.Now,
                    });
                }
            }

            return theme;
        }

        public Theme ConvertToTheme(dynamic item)
        {
            Theme theme = new Theme(item.Guid);

            theme.Id = item.Id;
            theme.Cost = item.Cost;
            theme.Title = item.Title;
            theme.Description = item.Description;
            theme.IncludedItems = item.IncludedItems;
            theme.Images = this._serializer.Deserialize<IList<ThemeImage>>(item.Images);

            return theme;
        }
    }
}
