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
                    themes.Add(this.ConvertToTheme(theme));
                }

            }

            return themes;
        }
        public Theme GetThemesByGuid(Guid guid)
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
            if (this.GetThemesByGuid(theme.Guid) == null)
            {
                string sql = "Insert Into VintageRabbit.Themes (Guid, Title, Description, Cost, MainImage, Images, Products, DateCreated, DateLastModified) Values (@Guid, @Title, @Description, @Cost, @MainImage, @Images, @Products, @DateCreated, @DateLastModified)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Query(sql, new
                    {
                        Guid = theme.Guid,
                        Title = theme.Title,
                        Description = theme.Description,
                        Cost = theme.Cost,
                        MainImage = this._serializer.Serialize(theme.MainImage),
                        Images = this._serializer.Serialize(theme.Images),
                        Products = this._serializer.Serialize(theme.Products),
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now
                    });
                }
            }
            else
            {
                string sql = "Update VintageRabbit.Themes Set Title = @Title, Description = @Description, Cost = @Cost, MainImage = @MainImage, Images = @Images, Products = @Products, DateLastModified = @DateLastModified Where Guid = @Guid";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Query(sql, new
                    {
                        Guid = theme.Guid,
                        Title = theme.Title,
                        Description = theme.Description,
                        Cost = theme.Cost,
                        MainImage = this._serializer.Serialize(theme.MainImage),
                        Images = this._serializer.Serialize(theme.Images),
                        Products = this._serializer.Serialize(theme.Products),
                        DateLastModified = DateTime.Now,
                    });
                }
            }

            return theme;
        }

        public Theme ConvertToTheme(dynamic item)
        {
            Theme theme = new Theme();

            theme.Id = item.Id;
            theme.Guid = item.Guid;
            theme.Cost = item.Cost;
            theme.Title = item.Title;
            theme.Description = item.Description;

            theme.MainImage = this._serializer.Deserialize<ThemeImage>(item.MainImage);
            theme.Images = this._serializer.Deserialize<IList<ThemeImage>>(item.Images);
            theme.Products = this._serializer.Deserialize<IList<ThemeProduct>>(item.Products);

            return theme;
        }
    }
}
