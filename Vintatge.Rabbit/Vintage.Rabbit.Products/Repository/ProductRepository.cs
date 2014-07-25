using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;
using Dapper;
using Vintage.Rabbit.Products.Repository.Entities;
using System.Configuration;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Common.Serialization;
using Vintage.Rabbit.Interfaces.Membership;
using Vintage.Rabbit.Common.Entities;
using Vintage.Rabbit.Interfaces.Products;

namespace Vintage.Rabbit.Products.Repository
{
    internal interface IProductRepository
    {
        IList<Product> GetFeaturedProducts();

        PagedResult<Product> GetProducts(int page, int itemsPerPage);

        IList<Product> GetProductsById(IList<int> productIds);

        Product GetProductByGuid(Guid productGuid);

        Product GetProductById(int productId);

        PagedResult<Product> GetProductsByType(ProductType type, int page, int itemsPerPage);

        PagedResult<Product> GetProductsByCategory(ProductType type, Category category, int page, int itemsPerPage);

        Product SaveProduct(Product product, IActionBy actionBy);
    }

    internal class ProductRepository : IProductRepository
    {
        private ISerializer _serializer;
        private string _connectionString;

        public ProductRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public PagedResult<Product> GetProductsByCategory(ProductType type, Category category, int page, int itemsPerPage)
        {
            PagedResult<Product> result = new PagedResult<Product>();
            result.PageNumber = page;
            result.ItemsPerPage = itemsPerPage;

            IList<int> categoryIds = new List<int>() { category.Id };
            foreach(var child in category.Children)
            {
                categoryIds.Add(child.Id);
            }

            string sql = @"Select * From VintageRabbit.Products
                            Where [Type] = @Type And VintageRabbit.Products.Id In (Select Distinct ProductId From VintageRabbit.ProductCategories Where CategoryId In @CategoryIds)
                            Order By DateCreated Desc 
                            OFFSET @Offset ROWS FETCH NEXT @ResultsPerPage ROWS ONLY;
                               Select Count(*) From VintageRabbit.Products Where [Type] = @Type And VintageRabbit.Products.Id In (Select Distinct ProductId From VintageRabbit.ProductCategories Where CategoryId In @CategoryIds);";

            int offset = (page - 1) * itemsPerPage;

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                using (var multi = connection.QueryMultiple(sql, new { CategoryIds = categoryIds, Type = type.ToString(), Offset = offset, ResultsPerPage = itemsPerPage }))
                {
                    var productResults = multi.Read<ProductDb>();

                    foreach (var product in productResults)
                    {
                        result.Add(this.ConvertToProduct(product));
                    }

                    result.TotalResults = multi.Read<int>().First();
                }

            }


            return result;
        }

        public PagedResult<Product> GetProductsByType(ProductType type, int page, int itemsPerPage)
        {
            PagedResult<Product> result = new PagedResult<Product>();
            result.PageNumber = page;
            result.ItemsPerPage = itemsPerPage;

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = @"Select * From VintageRabbit.Products Where [Type] = @Type Order By DateCreated Desc OFFSET @Offset ROWS FETCH NEXT @ResultsPerPage ROWS ONLY;
                               Select Count(*) From VintageRabbit.Products Where [Type] = @Type;";
                int offset = (page - 1) * itemsPerPage;

                using (var multi = connection.QueryMultiple(sql, new { Type = type.ToString(), Offset = offset, ResultsPerPage = itemsPerPage }))
                {
                    var productResults = multi.Read<ProductDb>();

                    foreach (var product in productResults)
                    {
                        result.Add(this.ConvertToProduct(product));
                    }

                    result.TotalResults = multi.Read<int>().First();
                }
            }


            return result;
        }

        public IList<Product> GetProductsById(IList<int> productIds)
        {
            IList<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var productResults = connection.Query<ProductDb>("Select * From VintageRabbit.Products Where Id In @Ids Order By DateCreated Desc", new { Ids = productIds });

                foreach (var product in productResults)
                {
                    products.Add(this.ConvertToProduct(product));
                }

            }

            return products;
        }

        public Product GetProductById(int productId)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var products = connection.Query<ProductDb>("Select * From VintageRabbit.Products Where [Id] = @ProductId", new { ProductId = productId });

                if (products.Any())
                {
                    return this.ConvertToProduct(products.First());
                }
            }

            return null;
        }

        public Product GetProductByGuid(Guid productGuid)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var products = connection.Query<ProductDb>("Select * From VintageRabbit.Products Where [Guid] = @Guid", new { Guid = productGuid });

                if (products.Any())
                {
                    return this.ConvertToProduct(products.First());
                }
            }

            return null;
        }

        public Product SaveProduct(Product product, IActionBy actionBy)
        {
            string categories = this._serializer.Serialize(product.Categories);
            string images = this._serializer.Serialize(product.Images);

            if (product.Id == 0)
            {
                string sql = @"Insert Into VintageRabbit.Products
                                ([Guid], Code, [Type], Title, Description, Price, Keywords, Inventory, IsFeatured, DateCreated, DateLastModified, UpdatedBy, Categories, Images)
                                Values
                                (@Guid, @Code, @Type, @Title, @Description, @Price, @Keywords, @Inventory, @IsFeatured, @DateCreated, @DateLastModified, @UpdatedBy, @Categories, @Images);
                                Select cast(SCOPE_IDENTITY() as int)";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    IEnumerable<int> productIds = connection.Query<int>(sql, new
                    {
                        Guid = product.Guid,
                        Code = product.Code,
                        Type = product.Type.ToString(),
                        Title = product.Title,
                        Description = product.Description,
                        Price = product.Cost,
                        Keywords = product.Keywords,
                        IsFeatured = product.IsFeatured,
                        DateCreated = DateTime.Now,
                        DateLastModified = DateTime.Now,
                        UpdatedBy = actionBy.Email,
                        Categories = categories,
                        Inventory = product.Inventory,
                        Images = images
                    });

                    if(productIds.Any())
                    {
                        product.Id = productIds.First();
                    }
                }
            }
            else
            {
                string sql = @"Update VintageRabbit.Products Set [Guid] = @Guid, Code = @Code, [Type] = @Type, Title = @Title, Description = @Description, Price = @Price, Keywords = @Keywords,
                                IsFeatured = @IsFeatured, Inventory = @Inventory, DateLastModified = @DateLastModified, UpdatedBy = @UpdatedBy, Categories = @Categories, Images = @Images
                                Where Id = @ProductId";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql, new
                    {
                        Guid = product.Guid,
                        Code = product.Code,
                        Type = product.Type.ToString(),
                        Title = product.Title.Trim(),
                        Description = product.Description.Trim(),
                        Price = product.Cost,
                        Keywords = product.Keywords,
                        IsFeatured = product.IsFeatured,
                        DateLastModified = DateTime.Now,
                        UpdatedBy = actionBy.Email,
                        Categories = categories,
                        Inventory = product.Inventory,
                        Images = images,
                        ProductId = product.Id
                    });
                }
            }


            this.SaveProductCategories(product);

            return product;
        }

        public IList<Product> GetFeaturedProducts()
        {
            IList<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var productResults = connection.Query<ProductDb>("Select * From VintageRabbit.Products Where IsFeatured = 1 Order By Title Desc");

                foreach (var product in productResults)
                {
                    products.Add(this.ConvertToProduct(product));
                }

            }

            return products;
        }

        public PagedResult<Product> GetProducts(int page, int itemsPerPage)
        {
            PagedResult<Product> result = new PagedResult<Product>();
            result.PageNumber = page;
            result.ItemsPerPage = itemsPerPage;

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                string sql = @"Select * From VintageRabbit.Products Order By Title OFFSET @Offset ROWS FETCH NEXT @ResultsPerPage ROWS ONLY;
                               Select Count(*) From VintageRabbit.Products;";
                int offset = (page - 1) * itemsPerPage;

                using (var multi = connection.QueryMultiple(sql, new { Offset = offset, ResultsPerPage = itemsPerPage }))
                {
                    var productResults = multi.Read<ProductDb>();

                    foreach (var product in productResults)
                    {
                        result.Add(this.ConvertToProduct(product));
                    }

                    result.TotalResults = multi.Read<int>().First();
                }
            }


            return result;
        }

        private void SaveProductCategories(Product product)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Execute("Delete From VintageRabbit.ProductCategories Where ProductId = @ProductId", new { ProductId = product.Id });
            }

            string sql = null;

            foreach(var category in product.Categories)
            {
                sql += string.Format("Insert Into VintageRabbit.ProductCategories (CategoryId, ProductId) Values ({0}, {1});", category.Id, product.Id);
                foreach(var childCategory in category.Children)
                {
                    sql += string.Format("Insert Into VintageRabbit.ProductCategories (CategoryId, ProductId) Values ({0}, {1});", childCategory.Id, product.Id);
                }
            }

            if (!string.IsNullOrEmpty(sql))
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Execute(sql);
                }
            }
        }

        private Product ConvertToProduct(ProductDb productDb)
        {
            IList<Category> categories = this._serializer.Deserialize<List<Category>>(productDb.Categories);
            IList<IProductImage> images = this._serializer.Deserialize<List<IProductImage>>(productDb.Images);

            Product product = new Product()
            {
                Id = productDb.Id,
                Guid = productDb.Guid,
                Type = productDb.Type,
                Code = productDb.Code,
                Title = productDb.Title,
                Description = productDb.Description,
                Keywords = productDb.Keywords,
                Cost = productDb.Price,
                IsFeatured = productDb.IsFeatured,
                Inventory = productDb.Inventory,
                Categories = categories,
                Images = images,
            };

            return product;
        }
    }
}
