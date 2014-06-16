﻿using System;
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

namespace Vintage.Rabbit.Products.Repository
{
    internal interface IProductRepository
    {
        IList<Product> GetFeaturedProducts();

        IList<Product> GetProducts(int page);

        IList<Product> GetProductsByCategory(Category category, ProductType productType);

        Product GetProductByGuid(Guid productGuid);

        Product GetProductById(int productId);

        IList<Product> GetProductsByType(ProductType type);
    }

    internal class ProductRepository : IProductRepository, IMessageHandler<SaveProductMessage>
    {
        private ISerializer _serializer;
        private string _connectionString;

        public ProductRepository(ISerializer serializer)
        {
            this._connectionString = ConfigurationManager.ConnectionStrings["VintageRabbit"].ConnectionString;
            this._serializer = serializer;
        }

        public IList<Product> GetProductsByType(ProductType type)
        {
            IList<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var productResults = connection.Query<ProductDb>("Select * From VintageRabbit.Products Where [Type] = @Type Order By DateCreated Desc", new { Type = type.ToString() });

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

        public void Handle(SaveProductMessage message)
        {
            var product = message.Product;
            string categories = this._serializer.Serialize(product.Categories);
            string images = this._serializer.Serialize(product.Images);

            if (message.Product.Id == 0)
            {
                string sql = @"Insert Into VintageRabbit.Products
                                ([Guid], Code, [Type], Title, Description, Price, Keywords, Inventory, IsFeatured, DateCreated, DateLastModified, UpdatedBy, Categories, Images)
                                Values
                                (@Guid, @Code, @Type, @Title, @Description, @Price, @Keywords, @Inventory, @IsFeatured, @DateCreated, @DateLastModified, @UpdatedBy, @Categories, @Images);
                                Select SCOPE_IDENTITY() as ProductId";

                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Query(sql, new
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
                        UpdatedBy = message.ActionBy.Email,
                        Categories = categories,
                        Inventory = product.Inventory,
                        Images = images
                    });
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
                        UpdatedBy = message.ActionBy.Email,
                        Categories = categories,
                        Inventory = product.Inventory,
                        Images = images,
                        ProductId = product.Id
                    });
                }
            }
        }

        public IList<Product> GetFeaturedProducts()
        {
            return this.GetProducts().Take(12).Select(o => o as Product).ToList();
        }

        public IList<Product> GetProducts(int page)
        {
            return this.GetProducts().ToList();
        }

        public IList<Product> GetProductsByCategory(Category category, ProductType productType)
        {
            return this.GetProducts().Where(o => o.Type == productType && o.Categories.Any(x => x.Name == category.Name)).ToList();
        }

        private IList<Product> GetProducts()
        {
            IList<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                var productResults = connection.Query<ProductDb>("Select * From VintageRabbit.Products Order By DateCreated Desc");

                foreach (var product in productResults)
                {
                    products.Add(this.ConvertToProduct(product));
                }

            }

            return products;
        }

        private Product ConvertToProduct(ProductDb productDb)
        {
            IList<Category> categories = this._serializer.Deserialize<List<Category>>(productDb.Categories);
            IList<ProductImage> images = this._serializer.Deserialize<List<ProductImage>>(productDb.Images);

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
