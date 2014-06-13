using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return new List<Category>()
            {
                new Category() { Id = 1, Name = "party-supplies", DisplayName = "Party supplies" },
                new Category() { Id = 2, Name = "gifts", DisplayName = "Gifts" },
                new Category() { Id = 3, Name = "games", DisplayName = "Games" },
                new Category() { Id = 4, Name = "tables-and-chairs", DisplayName = "Tables and chairs" },
                new Category() { Id = 5, Name = "photo-booth", DisplayName = "Photo booth" },
                new Category() { Id = 6, Name = "decorations", DisplayName = "Decorations" },
                new Category() { Id = 7, Name = "glassware-and-crockery", DisplayName = "Glassware & Crockery" },
                new Category() { Id = 8, Name = "props", DisplayName = "Props" }
            };
        }
        
        public Category GetCategory(string name)
        {
            return this.GetCategories().First(o => o.Name == name);
        }
    }
}
