using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Common.Enums;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Messaging.Messages;

namespace Vintage.Rabbit.Products.Repository
{
    internal interface ICategoryRepository
    {
        IList<Category> GetCategories();

        Category GetCategory(string name);
    }

    internal class CategoryRepository : ICategoryRepository
    {

        public CategoryRepository()
        {
        }

        public IList<Category> GetCategories()
        {
            Category partySupplies = new Category() { Id = 1, Name = "party-supplies", DisplayName = "Party supplies", ProductTypes = new List<ProductType>() { ProductType.Buy } };
            Category gifts = new Category() { Id = 2, Name = "gifts", DisplayName = "Gifts", ProductTypes = new List<ProductType>() { ProductType.Buy } };
            Category games = new Category() { Id = 3, Name = "games", DisplayName = "Games", ProductTypes = new List<ProductType>() { ProductType.Buy, ProductType.Hire } };
            Category tablesAndChairs = new Category() { Id = 4, Name = "tables-and-chairs", DisplayName = "Tables and chairs", ProductTypes = new List<ProductType>() { ProductType.Hire } };
            Category photoBooth = new Category() { Id = 5, Name = "photo-booth", DisplayName = "Photo booth", ProductTypes = new List<ProductType>() { ProductType.Hire } };
            Category decorations = new Category() { Id = 6, Name = "decorations", DisplayName = "Decorations", ProductTypes = new List<ProductType>() { ProductType.Hire } };
            Category glassware = new Category() { Id = 7, Name = "glassware-and-crockery", DisplayName = "Glassware & Crockery", ProductTypes = new List<ProductType>() { ProductType.Hire } };
            Category props = new Category() { Id = 8, Name = "props", DisplayName = "Props", ProductTypes = new List<ProductType>() { ProductType.Hire } };
            Category backdrops = new Category() { Id = 14, Name = "backdrops", DisplayName = "Backdrops", ProductTypes = new List<ProductType>() { ProductType.Hire } };


            Category balloons = new Category() { Id = 9, Name = "balloons", DisplayName = "Balloons", ProductTypes = new List<ProductType>() { ProductType.Buy } };
            Category onTheTable = new Category() { Id = 10, Name = "on-the-table", DisplayName = "On the table", ProductTypes = new List<ProductType>() { ProductType.Buy } };
            Category invitations = new Category() { Id = 11, Name = "invitations", DisplayName = "Invitations", ProductTypes = new List<ProductType>() { ProductType.Buy } };
            Category decorationsBuy = new Category() { Id = 12, Name = "decorations", DisplayName = "Decorations", ProductTypes = new List<ProductType>() { ProductType.Buy } };
            Category partyBags = new Category() { Id = 13, Name = "party-bags", DisplayName = "Party Bags", ProductTypes = new List<ProductType>() { ProductType.Buy } };

            partySupplies.Children.Add(balloons);
            partySupplies.Children.Add(onTheTable);
            partySupplies.Children.Add(invitations);
            partySupplies.Children.Add(decorationsBuy);
            partySupplies.Children.Add(partyBags);

            return new List<Category>()
            {
                partySupplies,
                gifts,
                games,
                tablesAndChairs,
                photoBooth,
                decorations,
                glassware,
                props,
                backdrops
            };
        }
        
        public Category GetCategory(string name)
        {
            foreach(Category category in this.GetCategories())
            {
                if(category.Name == name)
                {
                    return category;
                }

                if(category.Children.Any())
                {
                    foreach (Category child in category.Children)
                    {
                        if (child.Name == name)
                        {
                            return child;
                        }
                    }
                }
            }

            return null;
        }
    }
}
