using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintage.Rabbit.Interfaces.Messaging;
using Vintage.Rabbit.Interfaces.Serialization;
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
                new Category() { Id = 3, Name = "games", DisplayName = "Games" }
            };
        }

        public Category GetCategory(string name)
        {
            return this.GetCategories().First(o => o.Name == name);
        }
    }
}
